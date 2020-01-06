using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class BvsOwnerDto : BvsDto
  {
    public HeaderValue[] ValueHeaders { get; set; }

    public OwnerDto[] Owners { get; internal set; }
  }
}