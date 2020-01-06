namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1
{
  public class BeneificialInterestDto
  {
    public BvsOwnerDto CurrentBaseValueSegment { get; set; }
    public BvsOwnerDto PreviousBaseValueSegment { get; set; }
  }
}