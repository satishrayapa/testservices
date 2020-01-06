using System;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventDto
  {
    public int Id { get; set; }
    public long TranId { get; set; }
    public int GRMEventGroupId { get; set; }
    public int ObjectType { get; set; }
    public int ObjectId { get; set; }
    public int TaxBillId { get; set; }
    public int EventType { get; set; }
    public string EventTypeShortDescription { get; set; }
    public DateTime EventDate { get; set; }
    public int GRMModule { get; set; }
    public int BillTypeId { get; set; }
    public string BillNumber { get; set; }
    public short TaxYear { get; set; }
    public int RevObjId { get; set; }
    public string PIN { get; set; }
    public string Info { get; set; }
    public DateTime EffDate { get; set; }
    public int EffTaxYear { get; set; }
  }
}