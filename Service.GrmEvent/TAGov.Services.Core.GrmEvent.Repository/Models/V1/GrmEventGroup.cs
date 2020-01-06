using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  [Table( "GRMEventGroup" )]
  public class GrmEventGroup
  {
    [Key]
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