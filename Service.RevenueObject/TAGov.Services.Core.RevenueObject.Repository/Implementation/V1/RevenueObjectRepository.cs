using System;
using System.Linq;
using TAGov.Services.Core.RevenueObject.Repository.Interfaces.V1;
using TAGov.Services.Core.RevenueObject.Repository.Maps.V1;
using TAGov.Services.Core.RevenueObject.Repository.Models.V1;

namespace TAGov.Services.Core.RevenueObject.Repository.Implementation.V1
{
  public class RevenueObjectRepository : IRevenueObjectRepository
  {
    private readonly RevenueObjectContext _revenueObjectContext;
    private const string ActiveEffectiveStatus = "A";

    public RevenueObjectRepository( RevenueObjectContext revenueObjectContext )
    {
      _revenueObjectContext = revenueObjectContext;
    }

    public Models.V1.RevenueObject Get( int id, DateTime effectiveDate )
    {
      return
        GetRevenueObjectsByEffDate( effectiveDate )
          .Where( ro1 => ( ro1.Id == id ) )
          .Select( ro => new Models.V1.RevenueObject()
                         {
                           Id = ro.Id,
                           BeginEffectiveDate = ro.BeginEffectiveDate,
                           EffectiveStatus = ro.EffectiveStatus,
                           TransactionId = ro.TransactionId,
                           Pin = ro.Pin,
                           UnformattedPin = ro.UnformattedPin,
                           Ain = ro.Ain,
                           GeoCd = ro.GeoCd,
                           ClassCd = ro.ClassCd,
                           AreaCd = ro.AreaCd,
                           CountyCd = ro.CountyCd,
                           CensusTract = ro.CensusTract,
                           CensusBlock = ro.CensusBlock,
                           XCoordinate = ro.XCoordinate,
                           YCoordinate = ro.YCoordinate,
                           ZCoordinate = ro.ZCoordinate,
                           RightEstate = ro.RightEstate,
                           RightType = ro.RightType,
                           RightDescription = ro.RightDescription,
                           Type = ro.Type,
                           SubType = ro.SubType,
                           PropertyType =
                             _revenueObjectContext.SysType
                                                  .Single( //There should be exactly one sys type--catastrophic failure if there is not
                                                    st => ( st.Id == ro.Type ) &&
                                                          st.BeginEffectiveDate ==
                                                          _revenueObjectContext.SysType.Where(
                                                                                 sub => sub.Id == st.Id )
                                                                               .Max( new Func<SysType, DateTime?>( sub => sub.BeginEffectiveDate ) ) )
                                                  .ShortDescription.Trim(),
                           SitusAddress = GetSitusAddress( id, effectiveDate ),
                           Description = GetDescription( id, effectiveDate ),
                           ClassCodeDescription =
                             _revenueObjectContext.SysType
                                                  .Single(
                                                    st => ( st.Id == ro.ClassCd ) //There should be exactly one sys type--catastrophic failure if there is not
                                                          && st.BeginEffectiveDate ==
                                                          _revenueObjectContext.SysType.Where(
                                                                                 sub => sub.Id == st.Id )
                                                                               .Max( new Func<SysType, DateTime?>( sub => sub.BeginEffectiveDate ) ) )
                                                  .Description.Trim(),
                           RelatedPins = GetRelatedPins( id, effectiveDate )
                         } )
          .FirstOrDefault();
    }

    /// <summary>
    /// LINQ implementation of table valued function grm_records_RevObjByEffDate
    /// </summary>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    IQueryable<Models.V1.RevenueObject> GetRevenueObjectsByEffDate( DateTime effectiveDate )
    {
      return _revenueObjectContext.RevenueObject
                                  .Where( ro1 => ro1.BeginEffectiveDate ==
                                                 _revenueObjectContext.RevenueObject.Where(
                                                                        ro2 => ( ro2.BeginEffectiveDate < effectiveDate.AddDays( 1 ) )
                                                                               && ( ro1.Id == ro2.Id ) )
                                                                      .Max( new Func<Models.V1.RevenueObject, DateTime?>( ro3 => ro3.BeginEffectiveDate ) )
                                                 && ( ro1.EffectiveStatus == ActiveEffectiveStatus ) );
    }

    public TAG GetTAGByRevenueObjectId( int id, DateTime effectiveDate )
    {
      var tagRoleByEffDateActive =
        _revenueObjectContext.TAGRole.Where( tr1 => tr1.BeginEffectiveDate ==
                                                    _revenueObjectContext.TAGRole.Where(
                                                                           tr2 =>
                                                                             ( tr2.BeginEffectiveDate < effectiveDate.AddDays( 1 ) ) &&
                                                                             ( tr1.Id == tr2.Id ) )
                                                                         .Max( new Func<TAGRole, DateTime?>( tr3 => tr3.BeginEffectiveDate ) ) &&
                                                    ( tr1.EffectiveStatus == ActiveEffectiveStatus ) );
      var tagByEffYearActive =
        _revenueObjectContext.TAG.Where( t1 => t1.BeginEffectiveYear ==
                                               _revenueObjectContext.TAG.Where(
                                                                      t2 =>
                                                                        ( t2.BeginEffectiveYear <= effectiveDate.Year ) &&
                                                                        ( t1.Id == t2.Id ) )
                                                                    .Max( new Func<TAG, short?>( t3 => t3.BeginEffectiveYear ) ) &&
                                               ( t1.EffectiveStatus == ActiveEffectiveStatus ) );
      var returnTag =
        tagRoleByEffDateActive.Join( tagByEffYearActive,
                                     tagRole => tagRole.TAGId,
                                     tag => tag.Id,
                                     ( tagRole, tag ) => new { TagRole = tagRole, Tag = tag } )
                              .Where( t => ( t.TagRole.ObjectId == id )
                                           && ( t.TagRole.ObjectType == GetRevenueObjectSysTypeId() ) )
                              .Select( t => t.Tag )
                              .SingleOrDefault();
      //get rid of trailing spaces
      if ( returnTag != null )
        returnTag.Description = returnTag.Description.Trim();
      return returnTag;
    }

    public Models.V1.RevenueObject GetRevenueObjectByPin( string pin )
    {
      var revenueObject = _revenueObjectContext.RevenueObject.FirstOrDefault( x => x.Pin == pin );
      if ( revenueObject != null )
      {
        //get the situs address
        revenueObject.SitusAddress = GetSitusAddress( revenueObject.Id, DateTime.Today );
      }
      return revenueObject;
    }

    private SitusAddress GetSitusAddress( int revenueObjectId, DateTime effectiveDate )
    {
      var situsAddressRoleByEffDate =
        _revenueObjectContext.SitusAddressRole.Where(
          sar => sar.BeginEffectiveDate ==
                 _revenueObjectContext.SitusAddressRole.Where(
                                        sarsub =>
                                          ( sarsub.BeginEffectiveDate < effectiveDate.AddDays( 1 ) )
                                          && ( sarsub.Id == sar.Id ) )
                                      .Max( new Func<SitusAddressRole, DateTime?>( sarsub => sarsub.BeginEffectiveDate ) )
                 && ( sar.EffectiveStatus == ActiveEffectiveStatus ) );
      var situsAddressByEffDate =
        _revenueObjectContext.SitusAddress.Where(
          sa => sa.BeginEffectiveDate ==
                _revenueObjectContext.SitusAddress.Where(
                                       sasub => ( sasub.BeginEffectiveDate < effectiveDate.AddDays( 1 ) )
                                                && ( sasub.Id == sa.Id ) )
                                     .Max( new Func<SitusAddress, DateTime?>( sasub => sasub.BeginEffectiveDate ) )
                && ( sa.EffectiveStatus == ActiveEffectiveStatus ) );

      //Must handle the null case because the revenueObjectId itself may not exist.
      var situsAddressRoleJoinedToSitusAddress = situsAddressRoleByEffDate
                                                 .Join( situsAddressByEffDate,
                                                        sar => sar.SitusAddressId,
                                                        sa => sa.Id,
                                                        ( sar, sa ) => new { sar, sa } )
                                                 .SingleOrDefault( t => t.sar.ObjectType == GetRevenueObjectSysTypeId()
                                                                        && t.sar.ObjectId == revenueObjectId
                                                                        && t.sar.PrimeAddr == 1
                                                 );
      //trim
      if ( situsAddressRoleJoinedToSitusAddress?.sa == null ) return situsAddressRoleJoinedToSitusAddress?.sa;
      SitusAddress situsAddress = situsAddressRoleJoinedToSitusAddress.sa;
      situsAddress.FreeFormAddress = situsAddress.FreeFormAddress.Trim();
      situsAddress.City = situsAddress.City.Trim();
      situsAddress.StateCode = situsAddress.StateCode.Trim();
      situsAddress.PostalCode = situsAddress.PostalCode.Trim();
      return situsAddressRoleJoinedToSitusAddress.sa;
    }

    private int GetRevenueObjectSysTypeId()
    {
      var objectTypeSysTypeCatId =
        _revenueObjectContext.SysTypeCat
                             .Single( t1 => ( t1.EffectiveStatus == ActiveEffectiveStatus ) &&
                                            t1.BeginEffectiveDate
                                            == _revenueObjectContext.SysTypeCat.Where(
                                                                      t2 => t2.Id == t1.Id )
                                                                    .Max( new Func<SysTypeCat, DateTime?>( t3 => t3.BeginEffectiveDate ) ) &&
                                            ( t1.ShortDescription == "Object Type" ) )
                             .Id;
      return _revenueObjectContext.SysType
                                  .Single( t1 => ( t1.EffectiveStatus == ActiveEffectiveStatus ) &&
                                                 t1.BeginEffectiveDate
                                                 == _revenueObjectContext.SysType.Where(
                                                                           t2 => ( t2.Id == t1.Id ) )
                                                                         .Max( new Func<SysType, DateTime?>( t3 => t3.BeginEffectiveDate ) ) &&
                                                 ( t1.SysTypeCatId == objectTypeSysTypeCatId ) &&
                                                 ( t1.ShortDescription == "RevObj" ) )
                                  .Id;
    }

    private string GetDescription( int revenueObjectId, DateTime effectiveDate )
    {

      // Comes from table valued function grm_records_DescrHeaderByEffDate.
      var result =
        _revenueObjectContext.DescriptionHeader.Where(
                               dh => dh.BeginEffectiveDate ==
                                     _revenueObjectContext.DescriptionHeader.Where(
                                                            dhsub => ( dhsub.BeginEffectiveDate < effectiveDate.AddDays( 1 ) )
                                                                     && ( dhsub.Id == dh.Id ) )
                                                          .Max( new Func<DescriptionHeader, DateTime?>( dhsub => dhsub.BeginEffectiveDate ) )
                                     && dh.EffectiveStatus == ActiveEffectiveStatus
                                     && dh.RevenueObjectId == revenueObjectId )
                             .OrderBy( dh => dh.SequenceNumber ).FirstOrDefault();

      return result?.DisplayDescription.Trim();
    }

    /// <summary>
    /// LINQ implementation of table valued function grm_records_RelRevObjByEffDate
    /// </summary>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    private IQueryable<RelatedRevenueObject> GetRelatedRevenueObjectsByEffectiveDate( DateTime effectiveDate )
    {
      return _revenueObjectContext.RelatedRevenueObject.Where(
        rro => rro.BeginEffectiveDate ==
               _revenueObjectContext.RelatedRevenueObject.Where(
                                      rrosub => ( rrosub.BeginEffectiveDate < effectiveDate.AddDays( 1 ) )
                                                && ( rrosub.Id == rro.Id ) )
                                    .Max( new Func<RelatedRevenueObject, DateTime?>( rrosub => rrosub.BeginEffectiveDate ) )
               && ( rro.EffectiveStatus == ActiveEffectiveStatus ) );
    }

    private string GetRelatedPins( int revenueObjectId, DateTime effectiveDate )
    {
      var relatedRevenueObjects = GetRelatedRevenueObjectsByEffectiveDate( effectiveDate )
        .Where(
          r => ( r.RevenueObject1Id == revenueObjectId )
               || ( r.RevenueObject2Id == revenueObjectId ) )
        .Select( r =>
                   new
                   {
                     RevObjId = ( ( r.RevenueObject1Id == revenueObjectId ) ? r.RevenueObject2Id : r.RevenueObject1Id )
                   } )
        .Distinct();
      var relatedPins = relatedRevenueObjects
        .Join( GetRevenueObjectsByEffDate( effectiveDate ),
               rel => rel.RevObjId,
               ro => ro.Id,
               ( rel, ro ) => ro )
        .Join( _revenueObjectContext.ClassCodeMap,
               ro => ro.ClassCd,
               ccm => ccm.ClassCode,
               ( ro, ccm ) => new { Ro = ro, CCM = ccm } )
        .Select( roCcm => new
                          {
                            RevenueObjectId = roCcm.Ro.Id,
                            roCcm.Ro.Pin,
                            roCcm.Ro.ClassCd,
                            roCcm.CCM.RollType
                          } )
        .FirstOrDefault();

      if ( relatedPins != null )
      {
        return $"{relatedPins.RevenueObjectId} , {relatedPins.Pin}, {relatedPins.ClassCd}, {relatedPins.RollType}";
      }
      return "";
    }

    public Models.V1.RevenueObject GetRevenueObjectSitusAddressByPin( string pin )
    {
      var revenueObject = GetRevenueObjectByPin( pin );
      if ( revenueObject != null )
      {
        revenueObject.SitusAddress = GetSitusAddress( revenueObject.Id, DateTime.Today );
      }
      return revenueObject;
    }
  }
}
