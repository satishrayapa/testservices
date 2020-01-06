using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  public class OwnerDetail
  {
    [Key]
    public int LegalPartyId { get; set; }

    public int? OwnerId { get; set; }

    public string OwnerName { get; set; }

    public int LegalPartyRoleId { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public int GrmEventId { get; set; }

    public decimal PercentageInterestGain { get; set; }
  }
}
