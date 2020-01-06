using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  public class StatutoryReference
  {
    [Key]
    public int Key { get; set; }

    public string Description { get; set; }
  }
}
