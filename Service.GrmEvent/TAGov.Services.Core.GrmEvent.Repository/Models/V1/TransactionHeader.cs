using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.GrmEvent.Repository.Models.V1
{
  [Table( "TranHeader" )]
  public class TransactionHeader
  {
    [Key]
    public long Id { get; set; }

    public int UserProfileId { get; set; }
    public DateTime StartTimestamp { get; set; }
    public DateTime ChangeTimestamp { get; set; }
    public DateTime EffDate { get; set; }
    public int EffTaxYear { get; set; }
    public int TranType { get; set; }
    public string TaskName { get; set; }
    public string CallingFunction { get; set; }
    public string WorkstationId { get; set; }
    public string IPAddr { get; set; }
    public string MACAddr { get; set; }
    public long TranId { get; set; }
  }
}