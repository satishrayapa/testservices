using System;
using TAGov.Services.Core.RevenueObject.Repository.Models.V1;

namespace TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1
{
  public interface IRevenueObjectRepository
  {
    Models.V1.RevenueObject Get( int id, DateTime effectiveDate );

    TAG GetTAGByRevenueObjectId( int id, DateTime effectiveDate );

    Models.V1.RevenueObject GetRevenueObjectByPin( string pin );

    Models.V1.RevenueObject GetRevenueObjectSitusAddressByPin( string pin );
  }
}
