using System.ComponentModel.DataAnnotations;

namespace TAGov.Services.Core.RevenueObject.Repository.Models.V1
{
  public class MarketAndRestrictedValue
  {
    [Key]
    public int SubComponent { get; set; }

    public decimal MarketValue { get; set; }
    public decimal RestrictedValue { get; set; }
  }
}
