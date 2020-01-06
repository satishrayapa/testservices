using System.Collections.Generic;
using System.Linq;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read
{
  public class ComponentDto
  {
    public ComponentDto()
    {
      ComponentDetails = new List<ComponentDetailDto>();
    }

    public int? EventId { get; set; }

    public string EventName { get; set; }

    public string EventDate { get; set; }

    public string EffectiveDate { get; set; }

    public string EventType { get; set; }

    public int BaseYear { get; set; }

    public decimal BaseValue
    {
      get { return ComponentDetails.Sum( x => x.BaseValue ); }
    }

    public decimal Fbyv
    {
      get { return ComponentDetails.Sum( x => x.Fbyv ); }
    }

    public int FbyvAsOfYear { get; set; }

    public int ValueHeaderId { get; set; }

    public List<ComponentDetailDto> ComponentDetails { get; set; }

    public bool? IsOverride { get; set; }
  }
}