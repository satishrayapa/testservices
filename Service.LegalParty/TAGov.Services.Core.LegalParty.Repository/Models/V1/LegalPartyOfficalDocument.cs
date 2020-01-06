using System;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  public class LegalPartyOfficalDocument
  {
    public int LegalPartyId { get; set; }
    public int LegalPartyRoleId { get; set; }
    public short GrantorGrantee { get; set; }
    public decimal PercentageBeneficialInterest { get; set; }
    public DateTime? DocumentDate { get; set; }
    public string DocumentNumber { get; set; }
    public int? DocumentType { get; set; }

    public string LegalPartyDisplayName { get; set; }
    public int? RightTransferId { get; set; }
  }
}