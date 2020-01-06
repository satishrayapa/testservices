using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  public class SubComponentDetail
  {
    [Key]
    public int SubComponentId { get; set; }

    public string SubComponent { get; set; }
    public int ComponentTypeId { get; set; }
    public string Component { get; set; }
  }
}
