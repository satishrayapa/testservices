namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read
{
  public class BvsDetailDto : BvsDto
  {
    public DetailDto[] Details { get; internal set; }
  }
}