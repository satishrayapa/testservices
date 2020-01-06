namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class ComponentDetailDto
  {
    public int ValueId { get; set; }

    public int ComponentId { get; set; }

    public string Component { get; set; }

    public int SubComponentId { get; set; }

    public string SubComponent { get; set; }

    public decimal BaseValue { get; set; }

    public decimal Fbyv { get; set; }

    public bool? IsOverride { get; set; }
  }
}