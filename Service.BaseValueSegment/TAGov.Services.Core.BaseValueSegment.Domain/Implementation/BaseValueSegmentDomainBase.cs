using System;
using System.Collections.Generic;
using System.Linq;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Domain.Implementation
{
  public abstract class BaseValueSegmentDomainBase
  {
    protected void PrepareTransactionForCreate( BaseValueSegmentTransaction baseValueSegmentTransaction, bool keepBaseValueSegmentReference )
    {
      baseValueSegmentTransaction.Id = 0;

      if ( !keepBaseValueSegmentReference )
        baseValueSegmentTransaction.BaseValueSegmentId = 0;

      foreach ( var baseValueSegmentOwner in baseValueSegmentTransaction.BaseValueSegmentOwners )
      {
        baseValueSegmentOwner.Id = 0;
        baseValueSegmentOwner.BaseValueSegmentTransactionId = 0;
      }

      foreach ( var baseValueSegmentValueHeader in baseValueSegmentTransaction.BaseValueSegmentValueHeaders )
      {
        baseValueSegmentValueHeader.Id = 0;
        baseValueSegmentValueHeader.BaseValueSegmentTransactionId = 0;

        foreach ( var baseValueSegmentValue in baseValueSegmentValueHeader.BaseValueSegmentValues )
        {
          baseValueSegmentValue.Id = 0;
          baseValueSegmentValue.BaseValueSegmentValueHeaderId = 0;
        }
      }
    }

    protected void DiscoverOwnerValuesForSavingInTransaction( Func<BaseValueSegmentTransactionType> getUserType,
                                                              Func<BaseValueSegmentTransactionType> getUserDeletedType,
                                                              BaseValueSegmentTransaction baseValueSegmentTransaction,
                                                              List<BaseValueSegmentOwnerValue> baseValueSegmentOwnerValues )
    {
      // if we are explicitly trying to save a UserDeleted Tran, do NOT change it to user
      var userDeletedTransactionType = getUserDeletedType();
      if ( baseValueSegmentTransaction.BaseValueSegmentTransactionTypeId == userDeletedTransactionType.Id )
      {
        baseValueSegmentTransaction.BaseValueSegmentTransactionType = userDeletedTransactionType;
        baseValueSegmentTransaction.BaseValueSegmentTransactionTypeId = userDeletedTransactionType.Id;
      } else {
        var userTransactionType = getUserType();
        baseValueSegmentTransaction.BaseValueSegmentTransactionType = userTransactionType;
        baseValueSegmentTransaction.BaseValueSegmentTransactionTypeId = userTransactionType.Id;
      }

      foreach ( var baseValueSegmentOwner in baseValueSegmentTransaction.BaseValueSegmentOwners )
      {
        // We are clearing out BaseValueSegmentOwnerValues because it needs a relationship from both
        // the header and the owner. However, both distint parents are not yet created. This causes
        // an issue when saving because EF does not know how to persist this kind of relationship.

        var list = baseValueSegmentOwner.BaseValueSegmentOwnerValueValues
                                        .Where( x => baseValueSegmentOwnerValues.All( y => y.Id != x.Id || y.Id == 0 ) ).ToList(); // Prevent duplicates.

        if ( list.Count > 0 )
        {
          // Store a reference to owner who is about to be persisted.
          list.ForEach( x => x.Owner = baseValueSegmentOwner );
        }

        // Clear out of the list as planned so EF wouldn't deal with it yet.
        baseValueSegmentOwner.BaseValueSegmentOwnerValueValues.Clear();

        if ( list.Count > 0 )
        {
          // Here, we are trying to find a matching header to set in each basevaluesegmentownervalue.
          list.ForEach( x =>
                        {
                          foreach ( var baseValueSegmentValueHeader in baseValueSegmentTransaction.BaseValueSegmentValueHeaders )
                          {
                            if ( baseValueSegmentValueHeader.BaseValueSegmentOwnerValues.Any( y => y.Id == x.Id ) )
                            {
                              x.Header = baseValueSegmentValueHeader;
                              break;
                            }
                          }
                        } );

          baseValueSegmentOwnerValues.AddRange( list );
        }
      }

      foreach ( var baseValueSegmentValueHeader in baseValueSegmentTransaction.BaseValueSegmentValueHeaders )
      {
        // Same reason as above. baseValueSegmentOwnerValues will be used to save.
        baseValueSegmentValueHeader.BaseValueSegmentOwnerValues.Clear();
      }
    }
  }
}
