using System;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1
{
  public class RevenueObject
  {
    public int Id { get; set; }
    public DateTime BeginEffectiveDate { get; set; }
    public string EffectiveStatus { get; set; }
    public string Pin { get; set; }
    public string UnformattedPin { get; set; }
    public string Ain { get; set; }
    public string GeoCd { get; set; }
    public int ClassCd { get; set; }
    public string ClassCodeDescription { get; set; }
    public string AreaCd { get; set; }
    public string CountyCd { get; set; }
    public string CensusTrack { get; set; }
    public string CensusBlock { get; set; }
    public string XCoordinate { get; set; }
    public string YCoordinate { get; set; }
    public string ZCoordinate { get; set; }
    public int RightEstate { get; set; }
    public int RightType { get; set; }
    public int RightDescription { get; set; }
    public int Type { get; set; }
    public int SubType { get; set; }
    public string TAG { get; set; }
    public SitusAddress SitusAddress { get; set; }
    public string Description { get; set; }
    public string PropertyType { get; set; }

    public string RelatedPins { get; set; }

    //TODO Determine true source of RevenueCode
    public string RevenueCode { get; set; }

    //TODO Determine true source of TaxCode
    public string TaxCode { get; set; }

    //TODO Determine true source of TotalBaseValue
    public decimal TotalBaseValue { get; set; }
  }
}
