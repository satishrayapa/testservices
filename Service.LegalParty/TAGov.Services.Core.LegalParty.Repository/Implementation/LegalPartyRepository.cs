using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TAGov.Services.Core.LegalParty.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.LegalParty.Repository.Models.V1;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Constants;
using TAGov.Services.Core.LegalParty.Repository.Models.V1.Enums;

namespace TAGov.Services.Core.LegalParty.Repository.Implementation
{
  public class LegalPartyRepository : ILegalPartyRepository
  {
    private readonly LegalPartyContext _legalPartyContext;

    public LegalPartyRepository( LegalPartyContext legalPartyContext )
    {
      _legalPartyContext = legalPartyContext;
    }

    public Models.V1.LegalParty GetByEffectiveDate( int legalPartyRoleId, DateTime effectiveDate )
    {
      return GetByEffectiveDate( legalPartyRoleId, effectiveDate, null );
    }

    private Models.V1.LegalParty GetByEffectiveDate( int legalPartyRoleId, DateTime effectiveDate, EffectiveStatuses? effectiveStatus )
    {
      // translate effective date enum to searchable value
      string effectiveStatusSearchValue = null;

      if ( effectiveStatus.HasValue )
      {
        effectiveStatusSearchValue = effectiveStatus.Value.GetEffectiveStatusStringFromEnum();
      }

      // execute query
      var legalParty = ( from lp in _legalPartyContext.LegalParty
                         join lpr in _legalPartyContext.LegalPartyRole on lp.Id equals lpr.LegalPartyId
                         where lpr.Id == legalPartyRoleId &&
                               ( effectiveStatusSearchValue == null || lpr.ModelEffectiveStatus == EffectiveStatus.Active )
                               && lpr.BegEffDate == ( from lprSub in _legalPartyContext.LegalPartyRole
                                                      where lprSub.Id == lpr.Id && lprSub.BegEffDate < effectiveDate.CalculateEffectiveDate()
                                                      select lprSub.BegEffDate ).DefaultIfEmpty().Max()
                         select lp ).FirstOrDefault();

      return legalParty;
    }

    public IEnumerable<LegalPartyRole> GetLegalPartyRolesByRevenueObjectIdAndEffectiveDate( int revenueObjectId, DateTime effectiveDate )
    {
      // TODO: transfer to linq
      const string sql = @"
      SELECT [Id]
          ,[BegEffDate]
          ,[EffStatus]
          ,[TranId]
          ,[LegalPartyId]
          ,[ObjectType]
          ,[ObjectId]
          ,[AcctId]
          ,[LPRoleType]
          ,[PrimeLegalParty]
          ,[OwnershipType]
          ,[PercentInt]
          ,[Numerator]
          ,[Denominator]
          ,[GroupSequence]
          ,[LegalPartyRoleSubtype]
          ,[OriginalTransferor]
          ,[Survivorship]
          ,[PercentBeneficialInt]
          ,-1 AS LPRId
          ,-1 as LPRObjType
          
          FROM LegalPartyRole LPR
          WHERE LPR.ObjectType =  100002
          AND   LPR.BegEffDate = ( select max(sub.BegEffDate) from LegalPartyRole sub where sub.BegEffDate <= @p_EffDate AND sub.Id = LPR.Id )
      AND lpr.ObjectId=@p_revenuObjectId
      AND LPR.effStatus = @p_EffStatusFilter
          UNION ALL
          SELECT lprsub.Id AS Id
          ,lprsub.BegEffDate
          ,lprsub.EffStatus
          ,lprsub.TranId
          ,lprsub.LegalPartyId
          ,100002 AS ObjectType
          ,lprsub2.ObjectId AS ObjectId
          ,lprsub.AcctId
          ,lprsub.LPRoleType
          ,lprsub.PrimeLegalParty
          ,lprsub.OwnershipType
          ,lprsub.PercentInt
          ,lprsub.Numerator
          ,lprsub.Denominator
          ,lprsub.GroupSequence
          ,lprsub.LegalPartyRoleSubtype
          ,lprsub.OriginalTransferor
          ,lprsub.Survivorship
          ,lprsub.PercentBeneficialInt PercentBeneficialInt
          ,lprsub.ObjectId as LPRId
          ,lprsub.ObjectType as LPRObjType
          FROM LegalPartyRole lprsub
          JOIN (                SELECT LPR.*
          
          FROM LegalPartyRole LPR
          WHERE LPR.ObjectType = 100002 AND LPRoleType = 100701
      AND LPR.ObjectId=@p_revenuObjectId
          AND   LPR.BegEffDate = ( select max(sub.BegEffDate) from LegalPartyRole sub where sub.BegEffDate <= @p_EffDate AND sub.Id = LPR.Id )
      AND LPR.effStatus = @p_EffStatusFilter 
          ) lprsub2
          ON lprsub.ObjectType = 100030
          AND lprsub.ObjectId = lprsub2.Id
          AND   lprsub.BegEffDate = ( select max(sub2.BegEffDate) from LegalPartyRole sub2 where sub2.BegEffDate <= @p_EffDate AND sub2.Id = lprsub.Id )
      AND lprsub.effStatus = @p_EffStatusFilter";

      var legalPartyRoles = _legalPartyContext.LegalPartyRole.FromSql( sql,
                                                                       // ReSharper disable once FormatStringProblem
                                                                       new SqlParameter( "@p_EffDate", SqlDbType.DateTime ) { Value = effectiveDate.CalculateEffectiveDate() },
                                                                       // ReSharper disable once FormatStringProblem
                                                                       new SqlParameter( "@p_revenuObjectId", SqlDbType.Int ) { Value = revenueObjectId },
                                                                       // ReSharper disable once FormatStringProblem
                                                                       new SqlParameter( "@p_EffStatusFilter", SqlDbType.Char ) { Value = 'A' } ).Include( "LegalParty" ).ToList();

      foreach ( var legalPartyRole in legalPartyRoles )
      {
        legalPartyRole.LegalParty.DisplayName = legalPartyRole.LegalParty.DisplayName.Trim();
        legalPartyRole.EffectiveStatus = EffectiveStatuses.Active; // TODO: The mapping is not working. We will need to fix this later.        
      }

      return legalPartyRoles.ToList();
    }

    public IEnumerable<LegalPartyRole> GetLegalPartyRolesById( int[] legalPartyRoleIdList )
    {
      var legalPartyRoles = _legalPartyContext.LegalPartyRole
                                              .Where( x => legalPartyRoleIdList.Contains( x.Id ) )
                                              .Include( "LegalParty" ).Distinct().ToList();

      return legalPartyRoles;
    }
  }
}
