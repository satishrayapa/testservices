using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  public class BeneficialInterestEvent
  {
    [Key]
    public int GrmEventId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventType { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string DocNumber { get; set; }

    public DateTime? DocDate { get; set; }

    public string DocType { get; set; }

    public OwnerDetail[] Owners { get; set; }

  }

  public class BeneficialInterestEventComparer : IEqualityComparer<BeneficialInterestEvent>
  {
    public bool Equals( BeneficialInterestEvent x, BeneficialInterestEvent y )
    {
      return x.GrmEventId == y.GrmEventId;
    }

    public int GetHashCode( BeneficialInterestEvent obj )
    {
      return obj.GrmEventId.GetHashCode();
    }

  }
}
