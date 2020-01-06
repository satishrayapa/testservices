using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "SysType" )]
  public class SystemType
  {
    public int Id { get; set; }
    public DateTime begEffDate { get; set; }
    public string shortDescr { get; set; }
    public string descr { get; set; }
    public short displayOrder { get; set; }
    public string effStatus { get; set; }
    public int sysTypeCatId { get; set; }
    public long tranId { get; set; }
    public short editShortDescr { get; set; }
    public short editDescr { get; set; }
    public short canDeleteRow { get; set; }
    public short canSelectRow { get; set; }
    public int depSysTypeId { get; set; }
    public int InternalId { get; set; }
  }
}
