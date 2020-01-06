using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Tests
{
  public class BaseValueSegmentRepositoryTests
  {
    private const int BvsId = 123;
    private const int BvsTranId = 456;
    private const int BvsOwnerId = 789;
    private const int BvsValueHeaderId = 111;
    private const int BvsValueId = 987;
    private const int BvsOwnerValueId = 654;
    private const int RevObjId = 543;
    private const int EventId = 432;
    private DateTime AssessmentEventDate = new DateTime( 1999, 1, 1 );
    private BaseValueSegmentQueryContext _context;

    public BaseValueSegmentRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder<BaseValueSegmentQueryContext>();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      _context = new BaseValueSegmentQueryContext( optionsBuilder.Options );

      var baseValueSegmentTransaction = new BaseValueSegmentTransaction
                                        {
                                          Id = BvsTranId,
                                          BaseValueSegmentId = BvsId,
                                          TransactionId = 0,
                                          EffectiveStatus = "A",
                                          BaseValueSegmentTransactionTypeId = 2,
                                          DynCalcStepTrackingId = -100,
                                          BaseValueSegmentOwners = new List<BaseValueSegmentOwner>(),
                                          BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeader>()
                                        };

      var baseValueSegmentOwner = new BaseValueSegmentOwner
                                  {
                                    Id = BvsOwnerId,
                                    BaseValueSegmentTransactionId = BvsTranId,
                                    LegalPartyRoleId = 0,
                                    BeneficialInterestPercent = 50,
                                    DynCalcStepTrackingId = 0,
                                    GRMEventId = EventId,
                                    AlphaBVSOwnerId = null,
                                    BaseValueSegmentOwnerValueValues = new List<BaseValueSegmentOwnerValue>()
                                  };

      var baseValueSegmentValueHeader = new BaseValueSegmentValueHeader
                                        {
                                          Id = BvsValueHeaderId,
                                          BaseValueSegmentTransactionId = BvsTranId,
                                          BaseYear = 2015,
                                          DynCalcStepTrackingId = 0,
                                          BaseValueSegmentValues = new List<BaseValueSegmentValue>()
                                        };

      var baseValueSegmentValue = new BaseValueSegmentValue
                                  {
                                    Id = BvsValueId,
                                    BaseValueSegmentValueHeaderId = BvsValueHeaderId,
                                    SubComponent = 1,
                                    ValueAmount = 110000,
                                    PercentComplete = 75,
                                    FullValueAmount = 120000,
                                    DynCalcStepTrackingId = 0,
                                  };

      var baseValueSegmentOwnerValue = new BaseValueSegmentOwnerValue
                                       {
                                         Id = BvsOwnerValueId,
                                         BaseValueSegmentOwnerId = BvsOwnerValueId,
                                         BaseValue = 100000,
                                         DynCalcStepTrackingId = 0,
                                       };

      var baseValueSegment = new Models.V1.BaseValueSegment
                             {
                               Id = BvsId,
                               AsOf = AssessmentEventDate,
                               TransactionId = 0,
                               RevenueObjectId = RevObjId,
                               SequenceNumber = 1,
                               DynCalcInstanceId = 0,
                               DynCalcStepTrackingId = 0
                             };

      baseValueSegmentOwner.BaseValueSegmentOwnerValueValues.Add( baseValueSegmentOwnerValue );
      baseValueSegmentValueHeader.BaseValueSegmentValues.Add( baseValueSegmentValue );
      baseValueSegmentTransaction.BaseValueSegmentOwners.Add( baseValueSegmentOwner );
      baseValueSegmentTransaction.BaseValueSegmentValueHeaders.Add( baseValueSegmentValueHeader );
      baseValueSegment.BaseValueSegmentTransactions.Add( baseValueSegmentTransaction );

      _context.Add( baseValueSegment );

      var userDeletedTransaction = new BaseValueSegmentTransactionType
                                   {
                                     Id = 1,
                                     Name = "User Deleted",
                                     Description = "User Deleted"
                                   };
      _context.Add( userDeletedTransaction );
      var userTransactionType = new BaseValueSegmentTransactionType
                                {
                                  Id = 2,
                                  Name = "User",
                                  Description = "User"
                                };
      _context.Add( userTransactionType );


      var beneficialInterestInfo = new BeneficialInterestInfo
                                   {
                                     LegalPartyId = 100,
                                     GrmEventId = 200,
                                     EventDate = new DateTime( 2015, 1, 1 ),
                                     EventType = "Transfer",
                                     EffectiveDate = new DateTime( 2015, 2, 1 ),
                                     DocNumber = "UnitTestDoc",
                                     DocType = "Deed",
                                     DocDate = new DateTime( 2015, 3, 1 ),
                                     OwnerId = 300,
                                     OwnerName = "Unit Test Owner",
                                     LegalPartyRoleId = 400,
                                     BeneficialInterestPercentage = 50,
                                     PercentageInterestGain = 25
                                   };
      _context.Add( beneficialInterestInfo );

      _context.SaveChanges();
    }

    #region Get Tests

    [Fact]
    public void GetBaseValueSegment_MatchesId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.Get( BvsId );

      baseValueSegment.ShouldNotBeNull();
      baseValueSegment.Id.ShouldBe( BvsId );

      baseValueSegment = baseValueSegmentRepository.Get( RevObjId, AssessmentEventDate, 1 );
      baseValueSegment.ShouldNotBeNull();
      baseValueSegment.Id.ShouldBe( BvsId );
      baseValueSegment.RevenueObjectId.ShouldBe( RevObjId );
      baseValueSegment.AsOf.ShouldBe( AssessmentEventDate );
    }

    [Fact]
    public void GetBaseValueSegment_MatchesId_ValidateNoInactiveBvsTransactions()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.Get( BvsId );
      int bvsTransCount = baseValueSegment.BaseValueSegmentTransactions.Count;

      //Add an Active Transaction
      var testBvsTrans = new BaseValueSegmentTransaction
                         {
                           Id = BvsTranId + 1,
                           BaseValueSegmentId = BvsId,
                           TransactionId = 0,
                           EffectiveStatus = "A",
                           BaseValueSegmentTransactionTypeId = 2,
                           DynCalcStepTrackingId = -100,
                           BaseValueSegmentOwners = new List<BaseValueSegmentOwner>(),
                           BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeader>()
                         };
      _context.BaseValueSegmentTransactions.Add( testBvsTrans );
      _context.SaveChanges();
      baseValueSegment = baseValueSegmentRepository.Get( BvsId );
      int updatedBvsTransCount = baseValueSegment.BaseValueSegmentTransactions.Count;
      bvsTransCount.ShouldBeLessThan( updatedBvsTransCount );

      //Add an Inactive Transaction
      testBvsTrans = new BaseValueSegmentTransaction
                     {
                       Id = BvsTranId + 2,
                       BaseValueSegmentId = BvsId,
                       TransactionId = 0,
                       EffectiveStatus = "A",
                       BaseValueSegmentTransactionTypeId = UserDeletedTransactionId(),
                       DynCalcStepTrackingId = -100,
                       BaseValueSegmentOwners = new List<BaseValueSegmentOwner>(),
                       BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeader>()
                     };
      _context.BaseValueSegmentTransactions.Add( testBvsTrans );
      _context.SaveChanges();
      baseValueSegment = baseValueSegmentRepository.Get( BvsId );
      int newBvsTransCount = baseValueSegment.BaseValueSegmentTransactions.Count;
      updatedBvsTransCount.ShouldBe( newBvsTransCount );
    }

    [Fact]
    public void GetBaseValueSegment_InsertAndRetrievesTheRecord_DoesNotMatchId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.Get( 0 );

      baseValueSegment.ShouldBeNull();
    }

    [Fact]
    public void GetBaseValueSegment_MatchesId_ValidateBvsTransactionType()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.Get( BvsId );
      var bvsTran = baseValueSegment.BaseValueSegmentTransactions.FirstOrDefault();

      bvsTran.ShouldNotBeNull();
      bvsTran.BaseValueSegmentTransactionType.Description.Length.ShouldBeGreaterThan( 0 );
      bvsTran.BaseValueSegmentTransactionType.Description.ShouldBe( "User" );
      bvsTran.BaseValueSegmentTransactionType.Name.Length.ShouldBeGreaterThan( 0 );
      bvsTran.BaseValueSegmentTransactionType.Name.ShouldBe( "User" );
    }


    #endregion

    #region List Tests

    [Fact]
    public void GetBaseValueSegmentList_MatchesRevenueObject_RecordsReturned()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentList = baseValueSegmentRepository.List( RevObjId ).ToList();

      baseValueSegmentList.Count.ShouldBeGreaterThan( 0 );
    }

    [Fact]
    public void GetBaseValueSegmentList_InvalidRevenueObject_NoRecordsReturned()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentList = baseValueSegmentRepository.List( RevObjId + 1000 ).ToList();

      baseValueSegmentList.Count.ShouldBe( 0 );
    }

    #endregion

    #region Get By RevenueObjectId and AssessmentEventDate Tests

    [Fact]
    public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_MatchId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.GetByRevenueObjectIdAndAssessmentEventDate( RevObjId, AssessmentEventDate );

      baseValueSegment.ShouldNotBeNull();
    }

    [Fact]
    public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_DoesNotMatchId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.GetByRevenueObjectIdAndAssessmentEventDate( 0, AssessmentEventDate );

      baseValueSegment.ShouldBeNull();
    }

    [Fact]
    public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_DoesNotAsessmentEVentDate()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.GetByRevenueObjectIdAndAssessmentEventDate( BvsId,
                                                                                                    DateTime.Now );

      baseValueSegment.ShouldBeNull();
    }

    [Fact]
    public void GetBaseValueSegmentByRevenueObjectIdAndAssessmentEventDate_MatchRecord()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegment = baseValueSegmentRepository.Get( BvsId );
      int revObjectId = baseValueSegment.RevenueObjectId;
      DateTime asmtEventDate = baseValueSegment.AsOf;

      var baseValueSegmentTestGet = baseValueSegmentRepository
        .GetByRevenueObjectIdAndAssessmentEventDate( revObjectId, asmtEventDate );

      baseValueSegmentTestGet.ShouldNotBeNull();
    }

    #endregion

    #region Get BaseValueSegmentEvent by RevenueObjectId Tests

    [Fact]
    public void GetBaseValueSegmentEventsByRevenueObjectId_MatchId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentEvents = baseValueSegmentRepository.GetBvsEventsByRevenueObjectId( RevObjId ).ToList();

      baseValueSegmentEvents.ShouldNotBeEmpty();
      baseValueSegmentEvents.First().BvsId.ShouldBe( BvsId );
      baseValueSegmentEvents.First().RevenueObjectId.ShouldBe( RevObjId );
    }

    [Fact]
    public void GetBaseValueSegmentEventsByRevenueObjectId_MatchId_ValidateGRMEvent()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentEvents = baseValueSegmentRepository.GetBvsEventsByRevenueObjectId( RevObjId ).ToList();

      baseValueSegmentEvents.ShouldNotBeEmpty();
      baseValueSegmentEvents.First().GRMEventId.ShouldBe( EventId );
    }

    [Fact]
    public void GetBaseValueSegmentEventsByRevenueObjectId_MatchId_ValidateSortOrder()
    {
      //BVS Events shoulld be returned in Descending order based on AsOf Date
      //
      //Add another BVS with today's date. This BVS should be the first in returned listed
      //
      var testBvs = new Models.V1.BaseValueSegment
                    {
                      Id = BvsId - 1,
                      AsOf = DateTime.Now,
                      TransactionId = 0,
                      RevenueObjectId = RevObjId,
                      DynCalcInstanceId = 0,
                      BaseValueSegmentTransactions = new List<BaseValueSegmentTransaction>()
                    };
      var testBvsTrans = new BaseValueSegmentTransaction
                         {
                           Id = BvsTranId + 1,
                           BaseValueSegmentId = BvsId - 1,
                           TransactionId = 0,
                           EffectiveStatus = "A",
                           BaseValueSegmentTransactionTypeId = 2,
                           DynCalcStepTrackingId = -100,
                           BaseValueSegmentOwners = new List<BaseValueSegmentOwner>(),
                           BaseValueSegmentValueHeaders = new List<BaseValueSegmentValueHeader>()
                         };
      var testBvsOwner = new BaseValueSegmentOwner
                         {
                           Id = BvsOwnerId + 1,
                           BaseValueSegmentTransactionId = BvsTranId - 1,
                           LegalPartyRoleId = 0,
                           BeneficialInterestPercent = 50,
                           DynCalcStepTrackingId = 0,
                           GRMEventId = EventId + 1,
                           BaseValueSegmentOwnerValueValues = new List<BaseValueSegmentOwnerValue>()
                         };
      testBvsTrans.BaseValueSegmentOwners.Add( testBvsOwner );
      testBvs.BaseValueSegmentTransactions.Add( testBvsTrans );

      _context.BaseValueSegments.Add( testBvs );
      _context.SaveChanges();

      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );

      var baseValueSegmentEvents = baseValueSegmentRepository.GetBvsEventsByRevenueObjectId( RevObjId ).ToList();

      baseValueSegmentEvents.Count.ShouldBeGreaterThan( 1 );
      DateTime date1 = baseValueSegmentEvents.First().BvsAsOf;
      DateTime date2 = baseValueSegmentEvents.Last().BvsAsOf;

      date1.ShouldBeGreaterThan( date2 );
    }

    [Fact]
    public void GetBaseValueSegmentEventsByRevenueObjectId_InsertAndRetrievesTheRecords_DoesNotMatchId()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentEvents = baseValueSegmentRepository.GetBvsEventsByRevenueObjectId( 0 );

      baseValueSegmentEvents.ShouldBeEmpty();
    }

    #endregion

    #region Get BaseValueSegmentHistory Tests

    [Fact]
    public void GetBaseValueSegmentHistory_ValidRevenueObjectIdAndDateRecordsReturned()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentHistoryList = baseValueSegmentRepository.GetBaseValueSegmentHistory( RevObjId,
                                                                                               AssessmentEventDate.AddYears( -1 ), AssessmentEventDate.AddYears( 1 ) ).ToList();

      baseValueSegmentHistoryList.Count.ShouldBeGreaterThan( 0 );
    }

    [Fact]
    public void GetBaseValueSegmentHistory_InvalidRevenueObjectId_NoRecordsReturned()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentHistoryList = baseValueSegmentRepository.GetBaseValueSegmentHistory( RevObjId + 1000,
                                                                                               AssessmentEventDate.AddYears( -1 ), AssessmentEventDate.AddYears( 1 ) ).ToList();

      baseValueSegmentHistoryList.Count.ShouldBe( 0 );
    }

    [Fact]
    public void GetBaseValueSegmentHistory_InvalidDateRange_NoRecordsReturned()
    {
      IBaseValueSegmentRepository baseValueSegmentRepository = new BaseValueSegmentRepository( _context );
      var baseValueSegmentHistoryList = baseValueSegmentRepository.GetBaseValueSegmentHistory( RevObjId + 1000,
                                                                                               AssessmentEventDate.AddYears( -5 ), AssessmentEventDate.AddYears( -1 ) ).ToList();

      baseValueSegmentHistoryList.Count.ShouldBe( 0 );
    }

    #endregion

    #region Helper methods

    private int UserDeletedTransactionId()
    {
      return _context.BaseValueSegmentTransactionTypes.FirstOrDefault( x => x.Description == "User Deleted" ).Id;
    }

    #endregion

  }
}
