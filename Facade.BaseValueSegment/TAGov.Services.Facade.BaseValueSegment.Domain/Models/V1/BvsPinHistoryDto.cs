namespace TAGov.Services.Facade.BaseValueSegment.Domain.Models.V1.Read
{
  public class BvsPinHistoryDto
  {
    public string Pin { get; set; }
    public string Ain { get; set; }
    public string Status { get; set; }
    public int RevenueAccount { get; set; }
    public string Tag { get; set; }
    public SitusAddressDto SitusAddress { get; set; }
  }
}