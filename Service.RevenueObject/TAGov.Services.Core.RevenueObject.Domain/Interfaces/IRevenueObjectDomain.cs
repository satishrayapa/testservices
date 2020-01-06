using System;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;

namespace TAGov.Services.Core.RevenueObject.Domain.Interfaces
{
  public interface IRevenueObjectDomain
  {
    RevenueObjectDto Get( int id, DateTime effectiveDate );
    TAGDto GetTAGByRevenueObjectId( int id, DateTime effectiveDate );
    Models.V1.RevenueObjectDto GetRevenueObjectByPin( string pin );
    RevenueObjectDto GetRevenueObjectSitusAddressByPin( string pin );
  }
}
