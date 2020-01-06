using System;
using System.ComponentModel.DataAnnotations.Schema;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums;

namespace TAGov.Services.Core.LegalParty.Repository.Models.V1
{
  [Table( "SysType" )]
  public class SystemType
  {
    public int Id { get; set; }
    public DateTime BegEffDate { get; set; }
    public string ShortDescr { get; set; }
    public string Descr { get; set; }
    public short DisplayOrder { get; set; }

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
    public int SysTypeCatId { get; set; }
    public long TranId { get; set; }
    public short EditShortDescr { get; set; }
    public short EditDescr { get; set; }
    public short CanDeleteRow { get; set; }
    public short CanSelectRow { get; set; }
    public int DepSysTypeId { get; set; }
    public int InternalId { get; set; }
  }
}