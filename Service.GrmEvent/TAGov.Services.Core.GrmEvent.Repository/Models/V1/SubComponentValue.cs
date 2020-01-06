using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  public class SubComponentValue
  {
    [Key]
    public int SubComponentId { get; set; }

    public decimal MarketValue { get; set; }

    public decimal DeltaValue { get; set; }

    public string Description { get; set; }
  }
}