using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TAGov.Services.Core.BaseValueSegment.Repository.Models.V1
{
  [NotMapped]
  public class BaseValueSegmentHistory
  {
    [Key]
    public int BvsId { get; set; }

    public DateTime AsOf { get; set; }

    public int BaseYear { get; set; }

    public decimal BaseValue { get; set; }

    public decimal BeneficialInterestPercentage { get; set; }

    public string BvsTransactionType { get; set; }

    public int LegalPartyRoleId { get; set; }

    public int OwnerGrmEventId { get; set; }

    public int SubComponentId { get; set; }

    public long TransactionId { get; set; }

    public int ValueHeaderGrmEventId { get; set; }
  }

  public class BaseValueSegmentHistoryComparer : IEqualityComparer<BaseValueSegmentHistory>
  {
    public bool Equals( BaseValueSegmentHistory x, BaseValueSegmentHistory y )
    {
      return x.BvsId == y.BvsId &&
             x.LegalPartyRoleId == y.LegalPartyRoleId &&
             x.OwnerGrmEventId == y.OwnerGrmEventId &&
             x.SubComponentId == y.SubComponentId &&
             x.TransactionId == y.TransactionId &&
             x.ValueHeaderGrmEventId == y.ValueHeaderGrmEventId &&
             x.BaseYear == y.BaseYear;
    }

    public int GetHashCode( BaseValueSegmentHistory obj )
    {
      return obj.BvsId.GetHashCode() +
             obj.LegalPartyRoleId.GetHashCode() +
             obj.OwnerGrmEventId.GetHashCode() +
             obj.SubComponentId.GetHashCode() +
             obj.TransactionId.GetHashCode() +
             obj.ValueHeaderGrmEventId.GetHashCode() +
             obj.BaseYear.GetHashCode();
    }

  }

}
