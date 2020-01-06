using System;
using Shouldly;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.RevenueObject.Repository.Implementation.V1;
using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;
using TAGov.Services.Core.RevenueObject.Repository.Maps.V1;
using TAGov.Services.Core.RevenueObject.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.RevenueObject.Repository.Tests
{
  public class RevenueObjectRepositoryTests
  {
    private const Int32 Id = 999999999;
    private const string PinWithSitus = "PIN with Situs";

    private readonly DateTime _newestEffectiveDate = new DateTime( 1999, 3, 2 );
    private readonly DateTime _olderEffectiveDate = new DateTime( 1999, 1, 1 );
    private readonly DateTime _oldestEffectiveDate = new DateTime( 1990, 1, 1 );

    private const short NewestEffectiveYear = 2017;
    private const short OlderEffectiveYear = 2016;
    private const short OldestEffectiveYear = 2015;

    private readonly DateTime _eventDateLaterThanAllDatesInData = new DateTime( 2017, 5, 1 );

    private const int RevObjTypeParcel = 5;

    private const string SysTypeShortDescrParcel = "Parcel";

    private const int SysTypeCatId = 1;
    private const int SysTypeId = 100002;
    private const string SysTypeCatShortDescription = "Object Type";
    private const string SysTypeShortDescription = "RevObj";

    private const int SysTypeClassCdId = 8;
    private const string SysTypeClassCdDescr = "Some class code description";

    private const int TagId = 2;
    private const int OlderTagId = 3;
    private const int OldestTagId = 4;

    private const string TagDescription = "Some TAG description";

    private const int SitusAddressId = 6;

    private readonly SitusAddress _situsAddress = new SitusAddress()
                                                  {
                                                    FreeFormAddress = "Some SITUS address",
                                                    City = "SomeCity",
                                                    StateCode = "CA",
                                                    PostalCode = "99999"
                                                  };

    private const int DescHeaderId = 7;
    private const string DescHeaderDescription = "Some Description Header description";

    private const int RelatedRevenueObjectId = 9;
    private const string RelatedRevenueObjectPin = "10";
    private const int RelatedRevenueObjectRollType = 12;

    private RevenueObjectContext _context;

    public RevenueObjectRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      _context = new RevenueObjectContext( optionsBuilder );

      _context.RevenueObject.Add( new Models.V1.RevenueObject
                                  {
                                    Id = Id,
                                    BeginEffectiveDate = _newestEffectiveDate,
                                    EffectiveStatus = "A",
                                    TransactionId = 0,
                                    Pin = PinWithSitus,
                                    UnformattedPin = "UnformattedPin",
                                    Ain = "Ain",
                                    GeoCd = "GeoCd",
                                    ClassCd = SysTypeClassCdId,
                                    AreaCd = "AreaCd",
                                    CountyCd = "CountyCd",
                                    CensusTract = "Track",
                                    CensusBlock = "Block",
                                    XCoordinate = "XCoordinate",
                                    YCoordinate = "YCoordinate",
                                    ZCoordinate = "ZCoordinate",
                                    RightEstate = 0,
                                    RightType = 0,
                                    RightDescription = 0,
                                    Type = RevObjTypeParcel,
                                    SubType = 0
                                  } );

      _context.RevenueObject.Add( new Models.V1.RevenueObject
                                  {
                                    Id = Id,
                                    BeginEffectiveDate = _olderEffectiveDate,
                                    EffectiveStatus = "A",
                                    TransactionId = 0,
                                    Pin = "pin",
                                    UnformattedPin = "UnformattedPin",
                                    Ain = "Ain",
                                    GeoCd = "GeoCd",
                                    ClassCd = SysTypeClassCdId,
                                    AreaCd = "AreaCd",
                                    CountyCd = "CountyCd",
                                    CensusTract = "Track",
                                    CensusBlock = "Block",
                                    XCoordinate = "XCoordinate",
                                    YCoordinate = "YCoordinate",
                                    ZCoordinate = "ZCoordinate",
                                    RightEstate = 0,
                                    RightType = 0,
                                    RightDescription = 0,
                                    Type = RevObjTypeParcel,
                                    SubType = 0
                                  } );
      _context.RevenueObject.Add( new Models.V1.RevenueObject
                                  {
                                    Id = Id,
                                    BeginEffectiveDate = _oldestEffectiveDate,
                                    EffectiveStatus = "A",
                                    TransactionId = 0,
                                    Pin = "pin",
                                    UnformattedPin = "UnformattedPin",
                                    Ain = "Ain",
                                    GeoCd = "GeoCd",
                                    ClassCd = SysTypeClassCdId,
                                    AreaCd = "AreaCd",
                                    CountyCd = "CountyCd",
                                    CensusTract = "Track",
                                    CensusBlock = "Block",
                                    XCoordinate = "XCoordinate",
                                    YCoordinate = "YCoordinate",
                                    ZCoordinate = "ZCoordinate",
                                    RightEstate = 0,
                                    RightType = 0,
                                    RightDescription = 0,
                                    Type = RevObjTypeParcel,
                                    SubType = 0
                                  } );
      _context.RevenueObject.Add( new Models.V1.RevenueObject()
                                  {
                                    Id = RelatedRevenueObjectId,
                                    BeginEffectiveDate = _newestEffectiveDate,
                                    EffectiveStatus = "A",
                                    Pin = RelatedRevenueObjectPin,
                                    ClassCd = SysTypeClassCdId
                                  } );

      _context.SysTypeCat.Add( new SysTypeCat
                               {
                                 Id = SysTypeCatId,
                                 BeginEffectiveDate = _newestEffectiveDate,
                                 ShortDescription = SysTypeCatShortDescription,
                                 EffectiveStatus = "A"
                               } );
      _context.SysTypeCat.Add( new SysTypeCat
                               {
                                 Id = SysTypeCatId,
                                 BeginEffectiveDate = _olderEffectiveDate,
                                 ShortDescription = "older SysTypeCat description",
                                 EffectiveStatus = "A"
                               } );
      _context.SysTypeCat.Add( new SysTypeCat
                               {
                                 Id = SysTypeCatId,
                                 BeginEffectiveDate = _oldestEffectiveDate,
                                 ShortDescription = "oldest SysTypeCat description",
                                 EffectiveStatus = "A"
                               } );

      _context.SysType.Add( new SysType
                            {
                              Id = SysTypeId,
                              EffectiveStatus = "A",
                              BeginEffectiveDate = _newestEffectiveDate,
                              ShortDescription = SysTypeShortDescription,
                              SysTypeCatId = SysTypeCatId
                            } );
      _context.SysType.Add( new SysType
                            {
                              Id = SysTypeId,
                              EffectiveStatus = "A",
                              BeginEffectiveDate = _olderEffectiveDate,
                              ShortDescription = "older SysType description",
                              SysTypeCatId = SysTypeCatId
                            } );
      _context.SysType.Add( new SysType
                            {
                              Id = SysTypeId,
                              EffectiveStatus = "A",
                              BeginEffectiveDate = _oldestEffectiveDate,
                              ShortDescription = "oldest SysType description",
                              SysTypeCatId = SysTypeCatId
                            } );
      _context.SysType.Add( new SysType
                            {
                              Id = RevObjTypeParcel,
                              EffectiveStatus = "A",
                              BeginEffectiveDate = _oldestEffectiveDate,
                              ShortDescription = SysTypeShortDescrParcel,
                              SysTypeCatId = SysTypeCatId
                            } );
      _context.SysType.Add( new SysType()
                            {
                              Id = SysTypeClassCdId,
                              Description = SysTypeClassCdDescr,
                              EffectiveStatus = "A",
                              BeginEffectiveDate = _newestEffectiveDate
                            } );

      _context.TAG.Add( new TAG
                        {
                          Id = TagId,
                          BeginEffectiveYear = NewestEffectiveYear,
                          EffectiveStatus = "A",
                          Description = TagDescription
                        } );
      _context.TAG.Add( new TAG
                        {
                          Id = TagId,
                          BeginEffectiveYear = OlderEffectiveYear,
                          EffectiveStatus = "A",
                          Description = "older TAG description"
                        } );
      _context.TAG.Add( new TAG
                        {
                          Id = TagId,
                          BeginEffectiveYear = OldestEffectiveYear,
                          EffectiveStatus = "A",
                          Description = "oldest TAG description"
                        } );
      _context.TAGRole.Add( new TAGRole
                            {
                              Id = 133,
                              BeginEffectiveDate = _newestEffectiveDate,
                              EffectiveStatus = "A",
                              ObjectType = SysTypeId,
                              ObjectId = Id,
                              TAGId = TagId
                            } );
      _context.TAGRole.Add( new TAGRole
                            {
                              Id = 133,
                              BeginEffectiveDate = _olderEffectiveDate,
                              EffectiveStatus = "A",
                              ObjectType = SysTypeId,
                              ObjectId = Id,
                              TAGId = OlderTagId
                            } );
      _context.TAGRole.Add( new TAGRole
                            {
                              Id = 133,
                              BeginEffectiveDate = _oldestEffectiveDate,
                              EffectiveStatus = "A",
                              ObjectType = SysTypeId,
                              ObjectId = Id,
                              TAGId = OldestTagId
                            } );
      _context.TAGRole.Add( new TAGRole
                            {
                              Id = 133,
                              BeginEffectiveDate = new DateTime( 2017, 1, 1 ),
                              EffectiveStatus = "A",
                              ObjectType = SysTypeId,
                              ObjectId = Id,
                              TAGId = TagId
                            } );

      _context.SitusAddress.Add( new SitusAddress()
                                 {
                                   Id = SitusAddressId,
                                   BeginEffectiveDate = _newestEffectiveDate,
                                   EffectiveStatus = "A",
                                   FreeFormAddress = _situsAddress.FreeFormAddress,
                                   City = _situsAddress.City,
                                   StateCode = _situsAddress.StateCode,
                                   PostalCode = _situsAddress.PostalCode
                                 } );
      _context.SitusAddressRole.Add( new SitusAddressRole()
                                     {
                                       Id = Id,
                                       BeginEffectiveDate = _newestEffectiveDate,
                                       EffectiveStatus = "A",
                                       ObjectId = Id,
                                       ObjectType = SysTypeId,
                                       SitusAddressId = SitusAddressId,
                                       PrimeAddr = 1
                                     } );

      _context.DescriptionHeader.Add( new DescriptionHeader()
                                      {
                                        Id = DescHeaderId,
                                        BeginEffectiveDate = _newestEffectiveDate,
                                        DisplayDescription = DescHeaderDescription,
                                        EffectiveStatus = "A",
                                        RevenueObjectId = Id
                                      } );
      _context.DescriptionHeader.Add( new DescriptionHeader()
                                      {
                                        Id = DescHeaderId,
                                        BeginEffectiveDate = _olderEffectiveDate,
                                        DisplayDescription = "baloney",
                                        EffectiveStatus = "I",
                                        RevenueObjectId = Id
                                      } );

      _context.RelatedRevenueObject.Add( new RelatedRevenueObject()
                                         {
                                           Id = 23,
                                           EffectiveStatus = "A",
                                           BeginEffectiveDate = _newestEffectiveDate,
                                           RevenueObject1Id = Id,
                                           RevenueObject2Id = RelatedRevenueObjectId
                                         } );

      _context.ClassCodeMap.Add( new ClassCodeMap()
                                 {
                                   Id = Id,
                                   RollType = RelatedRevenueObjectRollType,
                                   ClassCode = SysTypeClassCdId
                                 } );

      _context.SaveChanges();
    }

    [Fact]
    public void Get_InsertsAndRetrievesTheRecord_MatchesId()
    {
      IRevenueObjectRepository revenueObjectRepository = new RevenueObjectRepository( _context );
      var revenueObjects = revenueObjectRepository.Get( Id, _newestEffectiveDate );

      revenueObjects.ShouldNotBeNull();
      revenueObjects.Id.ShouldBe( Id );
      revenueObjects.BeginEffectiveDate.ShouldBe( _newestEffectiveDate );
      revenueObjects.PropertyType.ShouldBe( SysTypeShortDescrParcel );
      SitusAddress situsAddress = revenueObjects.SitusAddress;
      situsAddress.FreeFormAddress.ShouldBe( _situsAddress.FreeFormAddress );
      situsAddress.City.ShouldBe( _situsAddress.City );
      situsAddress.StateCode.ShouldBe( _situsAddress.StateCode );
      situsAddress.PostalCode.ShouldBe( _situsAddress.PostalCode );
      revenueObjects.Description.ShouldBe( DescHeaderDescription );
      revenueObjects.ClassCodeDescription.ShouldBe( SysTypeClassCdDescr );
      revenueObjects.RelatedPins.ShouldContain( RelatedRevenueObjectId.ToString() );
      revenueObjects.RelatedPins.ShouldContain( RelatedRevenueObjectPin );
      revenueObjects.RelatedPins.ShouldContain( SysTypeClassCdId.ToString() );
      revenueObjects.RelatedPins.ShouldContain( RelatedRevenueObjectRollType.ToString() );
    }

    [Fact]
    public void Get_InsertsAndRetrievesTheRecord_DoesNotMatchId()
    {
      IRevenueObjectRepository revenueObjectRepository = new RevenueObjectRepository( _context );
      var revenueObjects = revenueObjectRepository.Get( 0, _newestEffectiveDate );

      revenueObjects.ShouldBeNull();
    }

    [Fact]
    public void Get_InsertsAndRetrievesTAG_MatchesId()
    {
      IRevenueObjectRepository revenueObjectRepository = new RevenueObjectRepository( _context );
      var tag = revenueObjectRepository.GetTAGByRevenueObjectId( Id, _eventDateLaterThanAllDatesInData );

      tag.ShouldNotBeNull();
      tag.Description.ShouldBe( TagDescription );
    }

    [Fact]
    public void Get_InsertsAndRetrievesTAG_DoesNotMatchId()
    {
      IRevenueObjectRepository revenueObjectRepository = new RevenueObjectRepository( _context );
      var tag = revenueObjectRepository.GetTAGByRevenueObjectId( 0, _eventDateLaterThanAllDatesInData );

      tag.ShouldBeNull();
    }

    [Fact]
    public void Get_InsertsAndRetrievesRevenueObjectIdWithSitus()
    {
      IRevenueObjectRepository revenueObjectRepository = new RevenueObjectRepository( _context );
      var revenueObject = revenueObjectRepository.GetRevenueObjectSitusAddressByPin( PinWithSitus );

      SitusAddress situsAddress = revenueObject.SitusAddress;
      situsAddress.FreeFormAddress.ShouldBe( _situsAddress.FreeFormAddress );
      situsAddress.City.ShouldBe( _situsAddress.City );
      situsAddress.StateCode.ShouldBe( _situsAddress.StateCode );
      situsAddress.PostalCode.ShouldBe( _situsAddress.PostalCode );
    }
  }
}