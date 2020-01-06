using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Models.V1
{
  [Table( "ValueType" )]
  public class ValueType
  {
    public int Id { get; set; }
    public long TranId { get; set; }
    public string ShortDescr { get; set; }
    public int ValueTypeClass { get; set; }
    public int ValueTypeCat { get; set; }
    public int ValueTypeSubCat { get; set; }
    public int UnitOfMeasure { get; set; }
    public int CalcLevel { get; set; }
    public int LevyBasisType { get; set; }
    public decimal DisplaySequence { get; set; }
    public short AllowOverride { get; set; }
    public short SaveIfZero { get; set; }
    public int AttributeSysTypeCat1 { get; set; }
    public int AttributeSysTypeCat2 { get; set; }
    public int AttributeSysTypeCat3 { get; set; }
    public int AttributeSysTypeCat4 { get; set; }
    public int DisplayPrecision { get; set; }
  }
}
