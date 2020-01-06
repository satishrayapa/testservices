namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  public class GrmEventComponent
  {
    public GrmEvent GrmEvent { get; set; }
    public GrmEventGroup GrmEventGroup { get; set; }
    public TransactionHeader TransactionHeader { get; set; }
    public TransactionDetail TransactionDetail { get; set; }
  }
}