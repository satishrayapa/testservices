using TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read;

namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class BvsComponenDto : BvsDto
  {
    public ComponentDto[] Components { get; set; }

    public HeaderValue[] ValueHeaders { get; set; }

    public OwnerDto[] Owners { get; set; }

  }
}