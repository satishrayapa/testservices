using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [Table( "ValueOverride" )]
  public class CaliforniaConsumerPriceIndex
  {
    [Key]
    public int Id { get; set; }

    [Key]
    [Column( "BegEffYear" )]
    public short BeginEffectiveYear { get; set; }

    [Column( "AddlObjectId" )]
    public int AssessmentYear { get; set; }

    [Column( "ValueAmount" )]
    public decimal InflationFactor { get; set; }

    public int ValueTypeId { get; set; }

    public int ObjectId { get; set; }

    [ForeignKey( "ValueTypeId" )]
    public ValueType ValueType { get; set; }

    public string EffStatus { get; set; }
  }
}
