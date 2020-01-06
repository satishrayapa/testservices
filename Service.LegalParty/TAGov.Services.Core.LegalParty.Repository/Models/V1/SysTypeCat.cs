using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  public class SysTypeCat
  {
    [DatabaseGenerated( DatabaseGeneratedOption.None )]
    public int Id { get; set; }

    [Column( "BegEffDate" )]
    public DateTime BeginEffectiveDate { get; set; }

    [Column( "ShortDescr" )]
    [Required]
    [StringLength( 16 )]
    public string ShortDescription { get; set; }

    [NotMapped]
    public EffectiveStatuses EffectiveStatus
    {
      get { return _effectiveStatus; }
      set
      {
        _effectiveStatus = value;
        ModelEffectiveStatus = _effectiveStatus.GetEffectiveStatusStringFromEnum();
      }
    }

    private EffectiveStatuses _effectiveStatus = EffectiveStatuses.InActive;

    [Column( "effStatus" )]
    [Required]
    [StringLength( 1 )]
    public string ModelEffectiveStatus
    {
      get { return _modelEffectiveStatus; }
      set
      {
        string uValue = value.ToUpperInvariant();
        _modelEffectiveStatus = uValue;
        _effectiveStatus = uValue.GetEffectiveStatusEnumFromString();
      }
    }

    private string _modelEffectiveStatus = string.Empty;
  }
}
