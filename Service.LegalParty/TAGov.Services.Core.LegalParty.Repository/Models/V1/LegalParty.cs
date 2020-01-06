using System;
using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  public class LegalParty
  {
    [Key]
    public int Id { get; set; }

    public long TranId { get; set; }
    public int PrimeLPId { get; set; }
    public int CombineToId { get; set; }
    public int LegalPartyType { get; set; }
    public int LPSubType { get; set; }
    public int FunclRole { get; set; }
    public int AliasType { get; set; }
    public string DisplayName { get; set; }
    public string CompressedName { get; set; }
    public string NamePrefix { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string NameSfx { get; set; }
    public short Confidential { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public short ResidVerified { get; set; }
    public string SoundexCd { get; set; }
  }
}
