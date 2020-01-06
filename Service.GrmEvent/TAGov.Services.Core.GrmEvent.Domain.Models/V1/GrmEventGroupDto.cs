using System;

namespace TAGov.Services.Core.GrmEvent.Domain.Models.V1
{
  public class GrmEventGroupDto
  {
    public int Id { get; set; }
    public long TranId { get; set; }
    public int UserProfileId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int EventGroupType { get; set; }
    public string DocNumber { get; set; }
    public string Info { get; set; }
    public int ParentGRMEVentGroupId { get; set; }
  }
}