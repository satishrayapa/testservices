namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventComponentDto
  {
    public GrmEventDto GrmEvent { get; set; }
    public GrmEventGroupDto GrmEventGroup { get; set; }
    public TransactionHeaderDto TransactionHeader { get; set; }
    public TransactionDetailDto TransactionDetail { get; set; }
  }
}