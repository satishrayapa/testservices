using System;
using TAGov.Services.Core.LegalParty.Repository;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Constants;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums;

namespace Repository.Tests
{
  public static class TestDataBuilder
  {
    public const int LegalPartyId1 = 309431;
    public const int LegalPartyRoleId1 = 284518;
    public const int LegalPartyRoleId2 = 284519;
    public const int LegalPartyRoleId5 = 284522;
    public const int LegalPartyRoleId6 = 284523;

    public const int LegalPartyId2 = 309432;
    public const int LegalPartyRoleId3 = 284519;
    public const int LegalPartyRoleId7 = 284524;

    public const int LegalPartyRoleId4 = 13;

    public const int GrmEventId = 44;
    public const int SystemTypeId = 55;

    private const int SysTypeCatId = 1;

    public const int RevenueObjectId1 = 11;

    public const int RevenueObjectId2 = 12;

    public const int RevenueObjectId3 = 13;

    public static readonly DateTime OldestEffectiveDate = new DateTime( 2010, 01, 01 );
    public static readonly DateTime OlderEffectiveDate = new DateTime( 2011, 01, 01 );
    public static readonly DateTime NewestEffectiveDate = new DateTime( 2014, 01, 01 );

    private const int SysTypeId = 100002;
    private const string SysTypeCatShortDescription = "Object Type";
    private const string SysTypeShortDescription = "RevObj";

    public static void Build( LegalPartyContext legalPartyContext )
    {
      legalPartyContext.LegalParty.Add( new TAGov.Services.Core.LegalParty.Repository.Models.V1.LegalParty
                                        {
                                          Id = LegalPartyId1,
                                          AliasType = 0,
                                          CombineToId = 0,
                                          DisplayName = "Some Test",
                                          FunclRole = 0,
                                          LPSubType = 0,
                                          LegalPartyType = 0,
                                          PrimeLPId = 0,
                                          TranId = 0,
                                          CompressedName = "Compressed Name",
                                          Confidential = 0,
                                          DateOfBirth = DateTime.Now,
                                          DateOfDeath = DateTime.Now,
                                          FirstName = "First Name",
                                          LastName = "Last Name",
                                          MiddleName = "Middle Name",
                                          NamePrefix = "Prefix",
                                          NameSfx = "Suffix",
                                          ResidVerified = 0,
                                          SoundexCd = "SND1"
                                        } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId1,
                                              BegEffDate = OldestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId1,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId1,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 0,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId1,
                                              BegEffDate = NewestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.InActive,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId1,
                                              ObjectType = RevenueObjectId1,
                                              ObjectId = 0,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 0,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.LegalParty.Add( new TAGov.Services.Core.LegalParty.Repository.Models.V1.LegalParty
                                        {
                                          Id = LegalPartyId2,
                                          AliasType = 0,
                                          CombineToId = 0,
                                          DisplayName = "Some Test",
                                          FunclRole = 0,
                                          LPSubType = 0,
                                          LegalPartyType = 0,
                                          PrimeLPId = 0,
                                          TranId = 0,
                                          CompressedName = "Compressed Name",
                                          Confidential = 0,
                                          DateOfBirth = DateTime.Now,
                                          DateOfDeath = DateTime.Now,
                                          FirstName = "First Name",
                                          LastName = "Last Name",
                                          MiddleName = "Middle Name",
                                          NamePrefix = "Prefix",
                                          NameSfx = "Suffix",
                                          ResidVerified = 0,
                                          SoundexCd = "SND1"
                                        } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId3,
                                              BegEffDate = OlderEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId2,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId1,
                                              AcctId = 0,
                                              LegalPartyRoleType = SysType.SysTypeOwner,
                                              PrimeLegalParty = 0,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );
      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId3,
                                              BegEffDate = OldestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId2,
                                              ObjectType = SysType.SysTypeLegalPartyRole,
                                              ObjectId = RevenueObjectId1,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 1,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId4,
                                              BegEffDate = OldestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId1,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId2,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 0,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      //RevObj 3
      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId5,
                                              BegEffDate = OldestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId1,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId3,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 1,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId6,
                                              BegEffDate = OldestEffectiveDate.AddSeconds( 1 ),
                                              EffectiveStatus = EffectiveStatuses.InActive,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId1,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId3,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 1,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.LegalPartyRole.Add( new LegalPartyRole
                                            {
                                              Id = LegalPartyRoleId7,
                                              BegEffDate = OldestEffectiveDate,
                                              EffectiveStatus = EffectiveStatuses.Active,
                                              TranId = 0,
                                              LegalPartyId = LegalPartyId2,
                                              ObjectType = SysType.SysTypeRevObj,
                                              ObjectId = RevenueObjectId3,
                                              AcctId = 0,
                                              LegalPartyRoleType = 0,
                                              PrimeLegalParty = 1,
                                              OwnershipType = 0,
                                              PercentInt = 0,
                                              Numerator = 0,
                                              Denominator = 0,
                                              GroupSequence = 0,
                                              LegalPartyRoleSubtype = 0,
                                              OriginalTransferor = 0,
                                              Survivorship = 0
                                            } );

      legalPartyContext.SystemTypeCat.Add( new SysTypeCat
                                           {
                                             Id = SysTypeCatId,
                                             BeginEffectiveDate = NewestEffectiveDate,
                                             ShortDescription = SysTypeCatShortDescription,
                                             EffectiveStatus = EffectiveStatuses.Active
                                           } );
      legalPartyContext.SystemTypeCat.Add( new SysTypeCat
                                           {
                                             Id = SysTypeCatId,
                                             BeginEffectiveDate = OlderEffectiveDate,
                                             ShortDescription = "older SysTypeCat description",
                                             EffectiveStatus = EffectiveStatuses.Active
                                           } );
      legalPartyContext.SystemTypeCat.Add( new SysTypeCat
                                           {
                                             Id = SysTypeCatId,
                                             BeginEffectiveDate = OldestEffectiveDate,
                                             ShortDescription = "oldest SysTypeCat description",
                                             EffectiveStatus = EffectiveStatuses.Active
                                           } );

      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = SysTypeId,
                                          EffectiveStatus = EffectiveStatuses.Active,
                                          BegEffDate = NewestEffectiveDate,
                                          ShortDescr = SysTypeShortDescription,
                                          SysTypeCatId = SysTypeCatId
                                        } );
      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = SysTypeId,
                                          EffectiveStatus = EffectiveStatuses.Active,
                                          BegEffDate = OlderEffectiveDate,
                                          ShortDescr = "older SysType description",
                                          SysTypeCatId = SysTypeCatId
                                        } );
      legalPartyContext.SystemType.Add( new SystemType
                                        {
                                          Id = SysTypeId,
                                          EffectiveStatus = EffectiveStatuses.Active,
                                          BegEffDate = OldestEffectiveDate,
                                          ShortDescr = "oldest SysType description",
                                          SysTypeCatId = SysTypeCatId
                                        } );

      legalPartyContext.SaveChanges();
    }
  }
}
