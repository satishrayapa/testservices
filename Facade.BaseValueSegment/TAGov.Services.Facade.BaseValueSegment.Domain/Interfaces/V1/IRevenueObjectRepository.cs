using System;
using System.Threading.Tasks;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Interfaces.V1
{
  public interface IRevenueObjectRepository
  {
    Task<RevenueObjectDto> Get( int revenueObjectId, DateTime effectiveDate );

    Task<TAGDto> GetTag( int revenueObjectId );

    Task<RevenueObjectDto> GetByPin( string pin );

    Task<RevenueObjectDto> GetWithSitusByPin( string pin );
  }
}