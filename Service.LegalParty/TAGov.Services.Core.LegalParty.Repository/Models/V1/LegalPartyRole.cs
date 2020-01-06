using System;
using System.ComponentModel.DataAnnotations.Schema;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  public class LegalPartyRole
  {
    [Column( "Id" )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BegEffDate { get; set; }

    [NotMapped]
    public EffectiveStatuses EffectiveStatus
    {
      get { return _effectiveStatus; }
      set
      {
        _effectiveStatus = value;
        ModelEffectiveStatus = _effectiveStatus.GetEffectiveStatusStringFromEnum();
      }
    }

    private EffectiveStatuses _effectiveStatus = EffectiveStatuses.InActive;

    [Column( "EffStatus" )]
    public string ModelEffectiveStatus
    {
      get { return _modelEffectiveStatus; }
      set
      {
        string uValue = value.ToUpperInvariant();
        _modelEffectiveStatus = uValue;
        _effectiveStatus = uValue.GetEffectiveStatusEnumFromString();
      }
    }

    private string _modelEffectiveStatus = string.Empty;

    [Column( "TranId" )]
    public long TranId { get; set; }

    [Column( "LegalPartyId" )]
    public int LegalPartyId { get; set; }

    [Column( "ObjectType" )]
    public int ObjectType { get; set; }

    [Column( "ObjectId" )]
    public int ObjectId { get; set; }

    [Column( "AcctId" )]
    public int AcctId { get; set; }

    [Column( "LPRoleType" )]
    public int LegalPartyRoleType { get; set; }

    [Column( "PrimeLegalParty" )]
    public short PrimeLegalParty { get; set; }

    [Column( "OwnershipType" )]
    public int OwnershipType { get; set; }

    [Column( "PercentInt" )]
    public decimal PercentInt { get; set; }

    [Column( "Numerator" )]
    public short Numerator { get; set; }

    [Column( "Denominator" )]
    public short Denominator { get; set; }

    [Column( "GroupSequence" )]
    public int GroupSequence { get; set; }

    [Column( "LegalPartyRoleSubtype" )]
    public int LegalPartyRoleSubtype { get; set; }

    [Column( "OriginalTransferor" )]
    public int OriginalTransferor { get; set; }

    [Column( "Survivorship" )]
    public int Survivorship { get; set; }

    [Column( "PercentBeneficialInt" )]
    public decimal PercentBeneficialInterest { get; set; }

    [ForeignKey( "LegalPartyId" )]
    public virtual LegalParty LegalParty { get; set; }
  }
}