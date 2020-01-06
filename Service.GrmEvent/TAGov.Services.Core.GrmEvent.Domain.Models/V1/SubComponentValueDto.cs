namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class SubComponentValueDto
  {
    public int SubComponentId { get; set; }
    public decimal MarketValue { get; set; }

    public decimal DeltaValue { get; set; }

    public string Description { get; set; }
  }
}