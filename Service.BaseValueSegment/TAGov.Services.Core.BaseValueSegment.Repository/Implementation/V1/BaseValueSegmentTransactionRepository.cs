using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1
{
  public class BaseValueSegmentTransactionRepository : IBaseValueSegmentTransactionRepository
  {
    private readonly BaseValueSegmentQueryContext _baseValueSegmentQueryContext;

    public BaseValueSegmentTransactionRepository( BaseValueSegmentQueryContext baseValueSegmentQueryContext )
    {
      _baseValueSegmentQueryContext = baseValueSegmentQueryContext;
    }

    public async Task CreateAsync( BaseValueSegmentTransaction baseValueSegmentTransaction, IEnumerable<BaseValueSegmentOwnerValue> baseValueSegmentOwnerValuesList )
    {
      using ( var transaction = await _baseValueSegmentQueryContext.Database.BeginTransactionAsync() )
      {
        try
        {
          await _baseValueSegmentQueryContext.BaseValueSegmentTransactions.AddAsync( baseValueSegmentTransaction );
          await _baseValueSegmentQueryContext.SaveChangesAsync();

          var baseValueSegmentOwnerValues = baseValueSegmentOwnerValuesList.ToList();
          if ( baseValueSegmentOwnerValues.Count > 0 )
          {
            baseValueSegmentOwnerValues.ForEach( x =>
                                                 {
                                                   if ( x.Header == null )
                                                   {
                                                     // This allows us to determine in the DTO that was submitted.
                                                     throw new NullReferenceException( $"Header cannot be set for Base Value Segment Owner with Id: {x.Id}" );
                                                   }

                                                   x.Id = 0;
                                                   x.BaseValueSegmentOwnerId = x.Owner.Id;
                                                   x.BaseValueSegmentValueHeaderId = x.Header.Id;
                                                 } );

            await _baseValueSegmentQueryContext.BaseValueSegmentOwnerValues.AddRangeAsync( baseValueSegmentOwnerValues );
            await _baseValueSegmentQueryContext.SaveChangesAsync();
          }

          transaction.Commit();
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public Task<BaseValueSegmentTransaction> GetAsync( int id )
    {
      return _baseValueSegmentQueryContext.BaseValueSegmentTransactions.SingleOrDefaultAsync();
    }
  }
}
