using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "DescrHeader" )]
  public class DescriptionHeader
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    public string EffectiveStatus { get; set; }

    [Column( "RevObjId" )]
    public int RevenueObjectId { get; set; }

    [Column( "DisplayDescr" )]
    public string DisplayDescription { get; set; }

    public int SequenceNumber { get; set; }
  }
}
