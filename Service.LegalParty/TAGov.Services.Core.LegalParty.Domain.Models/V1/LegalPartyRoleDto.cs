using System;
using TAGov.Services.Core.LegalParty.Domain.Models.V1.Enums;

namespace TAGov.Services.Core.LegalParty.Domain.Models.V1
{
  public class LegalPartyRoleDto
  {
    public int Id { get; set; }

    public DateTime BegEffDate { get; set; }

    public long TranId { get; set; }

    public EffectiveStatuses EffectiveStatus { get; set; }

    public int LegalPartyId { get; set; }

    public int ObjectType { get; set; }

    public int ObjectId { get; set; }

    public int AcctId { get; set; }

    public int LegalPartyRoleType { get; set; }

    public short PrimeLegalParty { get; set; }

    public int OwnershipType { get; set; }

    public decimal PercentInt { get; set; }

    public short Numerator { get; set; }

    public short Denominator { get; set; }

    public int GroupSequence { get; set; }

    public int LegalPartyRoleSubtype { get; set; }

    public int OriginalTransferor { get; set; }

    public int Survivorship { get; set; }

    public virtual LegalPartyDto LegalParty { get; set; }
  }
}
