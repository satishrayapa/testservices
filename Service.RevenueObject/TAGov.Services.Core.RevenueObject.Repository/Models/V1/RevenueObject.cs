using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  [Table( "RevObj" )]
  public class RevenueObject
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "EffStatus" )]
    [StringLength( 1 )]
    [Required]
    public string EffectiveStatus { get; set; }

    [Column( "TranId" )]
    public long TransactionId { get; set; }

    [StringLength( 32 )]
    [Required]
    public string Pin { get; set; }

    [StringLength( 32 )]
    public string UnformattedPin { get; set; }

    [StringLength( 32 )]
    [Required]
    public string Ain { get; set; }

    [StringLength( 32 )]
    [Required]
    public string GeoCd { get; set; }

    public int ClassCd { get; set; }

    [Column( "AreaCd" )]
    [StringLength( 16 )]
    [Required]
    public string AreaCd { get; set; }

    [StringLength( 16 )]
    [Required]
    public string CountyCd { get; set; }

    [StringLength( 8 )]
    [Required]
    public string CensusTract { get; set; }

    [StringLength( 8 )]
    [Required]
    public string CensusBlock { get; set; }

    [StringLength( 32 )]
    [Column( "XCoord" )]
    [Required]
    public string XCoordinate { get; set; }

    [StringLength( 32 )]
    [Column( "YCoord" )]
    [Required]
    public string YCoordinate { get; set; }

    [StringLength( 32 )]
    [Column( "ZCoord" )]
    [Required]
    public string ZCoordinate { get; set; }

    public int RightEstate { get; set; }

    public int RightType { get; set; }

    [Column( "RightDescr" )]
    public int RightDescription { get; set; }

    [Column( "RevObjType" )]
    public int Type { get; set; }

    [Column( "RevObjSubType" )]
    public int SubType { get; set; }

    [NotMapped]
    public string PropertyType { get; set; }

    [NotMapped]
    public SitusAddress SitusAddress { get; set; }

    [NotMapped]
    public string Description { get; set; }

    [NotMapped]
    public string ClassCodeDescription { get; set; }

    [NotMapped]
    public string RelatedPins { get; set; }
  }
}
