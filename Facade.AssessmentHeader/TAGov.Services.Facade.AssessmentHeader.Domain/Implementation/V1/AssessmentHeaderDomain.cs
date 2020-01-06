using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Common.Exceptions;
using TAGov.Common.Http;
using TAGov.Services.Core.AssessmentEvent.Domain.Models.V1;
using TAGov.Services.Core.BaseValueSegment.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1;
using TAGov.Services.Core.LegalParty.Domain.Models.V1.Enums;
using TAGov.Services.Core.RevenueObject.Domain.Models.V1;
using TAGov.Services.Facade.AssessmentHeader.Domain.Interfaces.V1;
using TAGov.Services.Facade.AssessmentHeader.Domain.Models.V1;

namespace TAGov.Services.Facade.AssessmentHeader.Domain.Implementation.V1
{
  public class AssessmentHeaderDomain : IAssessmentHeaderDomain
  {
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly IApplicationSettingsHelper _applicationSettingsHelper;
    private readonly string Version = "v1.1";

    public AssessmentHeaderDomain( IHttpClientWrapper httpClientWrapper, IApplicationSettingsHelper applicationSettingsHelper )
    {
      _httpClientWrapper = httpClientWrapper;
      _applicationSettingsHelper = applicationSettingsHelper;
    }

    public async Task<Models.V1.AssessmentHeader> Get( int assessmentEventId )
    {
      if ( assessmentEventId < 1 )
      {
        throw new BadRequestException( "Invalid AssessmentEventId" );
      }

      var assessmentEventDto = await _httpClientWrapper.Get<AssessmentEventDto>( _applicationSettingsHelper.AssessmentEventServiceApiUrl, $"{Version}/assessmentevents/{assessmentEventId}" );

      if ( assessmentEventDto == null ) return null;

      var assessmentEventTran = assessmentEventDto.AssessmentEventTransactions.OrderByDescending( aet => aet.Id ).FirstOrDefault();

      BaseValueSegmentTransactionDto currentBaseValueSegmentTransaction = null;

      BaseValueSegmentDto baseValueSegmentDto = null;

      try
      {
        baseValueSegmentDto = await _httpClientWrapper.Get<BaseValueSegmentDto>( _applicationSettingsHelper.BaseValueSegmentServiceApiUrl,
                                                                                 $"{Version}/basevaluesegments/RevenueObjectId/{assessmentEventDto.RevObjId}/AssessmentEventDate/{assessmentEventDto.EventDate:yyyy-MM-dd}" );
      }
      catch ( NotFoundException )
      {
        // no records found
      }

      if ( baseValueSegmentDto != null )
      {
        var baseValueSegmentTransactions = baseValueSegmentDto.BaseValueSegmentTransactions.OrderByDescending( x => x.Id );
        if ( baseValueSegmentTransactions.Any() )
        {
          currentBaseValueSegmentTransaction = baseValueSegmentTransactions.First();
        }
      }

      var assessmentHeader = new Models.V1.AssessmentHeader
                             {
                               AssessmentEvent = new AssessmentEvent
                                                 {
                                                   AssessmentEventId = assessmentEventDto.Id,
                                                   AssessmentEventType = assessmentEventDto.AsmtEventType,
                                                   AssessmentEventTypeDescription = assessmentEventDto.AsmtEventTypeDescription,
                                                   RevenueObjectId = assessmentEventDto.RevObjId,
                                                   EventDate = assessmentEventDto.EventDate,
                                                   TaxYear = assessmentEventDto.TaxYear,
                                                   EventState = assessmentEventTran?.AsmtEventStateDescription,
                                                   TransactionId = assessmentEventTran?.Id ?? null,
                                                   BVSTranType = ( currentBaseValueSegmentTransaction != null ) ? currentBaseValueSegmentTransaction.BaseValueSegmentTransactionType.Description : string.Empty,
                                                   PrimaryBaseYear = assessmentEventDto.PrimaryBaseYear,
                                                   PrimaryBaseYearMultipleOrSingleDescription = assessmentEventDto.PrimaryBaseYearMultipleOrSingleDescription
                                                 }
                             };
      RevenueObjectDto revenueObjectDto = null;
      try
      {
        revenueObjectDto = await _httpClientWrapper.Get<RevenueObjectDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl,
                                                                           $"{Version}/revenueobjects/{assessmentEventDto.RevObjId}/EffectiveDate/{assessmentEventDto.EventDate:yyyy-MM-dd}" );
      }
      catch ( NotFoundException )
      {
        //if not found it could be because the PIN is inactive so send back a user-friendly message
        throw new NotFoundException( "BVS data is unavailable.  This may be because the PIN is no longer active." );
      }
      catch
      {
        throw new NotFoundException( "Unable to look up the Revenue Object.  This may be the result of an inactive record." );
      }

      var legalPartyRoleDtos = await _httpClientWrapper.Get<IList<LegalPartyRoleDto>>( _applicationSettingsHelper.LegalPartyServiceApiUrl,
                                                                                       $"{Version}/LegalParties/LegalPartyRoles/RevenueObjectId/{assessmentEventDto.RevObjId}/EffectiveDate/{assessmentEventDto.EventDate:O}" );

      AssessmentRevisionDto assessmentRevisionDto = null;
      if ( assessmentEventTran != null )
      {
        assessmentRevisionDto = await _httpClientWrapper.Get<AssessmentRevisionDto>( _applicationSettingsHelper.AssessmentEventServiceApiUrl,
                                                                                     $"{Version}/AssessmentEvents/AssessmentRevisionEventId/{assessmentEventTran.AsmtRevnEventId}/AssessmentEventDate/{assessmentEventDto.EventDate:yyyy-MM-dd}/AssessmentRevision" );
      }

      var tagDto = await _httpClientWrapper.Get<TAGDto>( _applicationSettingsHelper.RevenueObjectServiceApiUrl,
                                                         $"{Version}/revenueobjects/RevenueObjectId/{assessmentEventDto.RevObjId}/EffectiveDate/{assessmentEventDto.EventDate:yyyy-MM-dd}/TAG" );

      StatutoryReferenceDto statutoryReferenceDto = null;
      if ( assessmentEventTran != null )
      {
        statutoryReferenceDto = await _httpClientWrapper.Get<StatutoryReferenceDto>( _applicationSettingsHelper.AssessmentEventServiceApiUrl,
                                                                                     $"{Version}/StatutoryReference/{assessmentEventTran.Id}" );
      }

      if ( statutoryReferenceDto != null )
      {
        assessmentHeader.AssessmentEvent.RevenueAndTaxCode = statutoryReferenceDto.Description;
      }

      if ( revenueObjectDto != null )
      {
        assessmentHeader.RevenueObject = new RevenueObject
                                         {
                                           Id = revenueObjectDto.Id,
                                           BeginEffectiveDate = revenueObjectDto.BeginEffectiveDate,
                                           EffectiveStatus = revenueObjectDto.EffectiveStatus,
                                           Pin = revenueObjectDto.Pin,
                                           UnformattedPin = revenueObjectDto.UnformattedPin,
                                           Ain = revenueObjectDto.Ain,
                                           GeoCd = revenueObjectDto.GeoCd,
                                           ClassCd = revenueObjectDto.ClassCd,
                                           AreaCd = revenueObjectDto.AreaCd,
                                           CountyCd = revenueObjectDto.CountyCd,
                                           CensusTrack = revenueObjectDto.CensusTrack,
                                           CensusBlock = revenueObjectDto.CensusBlock,
                                           XCoordinate = revenueObjectDto.XCoordinate,
                                           YCoordinate = revenueObjectDto.YCoordinate,
                                           ZCoordinate = revenueObjectDto.ZCoordinate,
                                           RightEstate = revenueObjectDto.RightEstate,
                                           RightType = revenueObjectDto.RightType,
                                           RightDescription = revenueObjectDto.RightDescription,
                                           Type = revenueObjectDto.Type,
                                           SubType = revenueObjectDto.SubType,
                                           TAG = tagDto.Description,
                                           SitusAddress = revenueObjectDto.SitusAddress != null
                                                            ? new SitusAddress
                                                              {
                                                                FreeFormAddress = revenueObjectDto.SitusAddress.FreeFormAddress,
                                                                City = revenueObjectDto.SitusAddress.City,
                                                                StateCode = revenueObjectDto.SitusAddress.StateCode,
                                                                PostalCode = revenueObjectDto.SitusAddress.PostalCode
                                                              }
                                                            : null,
                                           Description = revenueObjectDto.Description,
                                           PropertyType = revenueObjectDto.PropertyType,
                                           RelatedPins = revenueObjectDto.RelatedPins,
                                           ClassCodeDescription = revenueObjectDto.ClassCodeDescription,
                                           //TODO Remove hard coding of TotalBaseValue once true source is determined
                                           TotalBaseValue = 345000m
                                         };

      }
      if ( legalPartyRoleDtos != null && legalPartyRoleDtos.Count > 0 )
      {
        var primeLegalPartyRole = GetPrimeLegalPartyRole( legalPartyRoleDtos );
        if ( primeLegalPartyRole != null )
        {
          var primeLegalParty = primeLegalPartyRole.LegalParty;
          assessmentHeader.LegalParty = new LegalParty
                                        {
                                          LegalPartyId = primeLegalParty.Id,
                                          DisplayName = primeLegalParty.DisplayName,
                                          FirstName = primeLegalParty.FirstName,
                                          MiddleName = primeLegalParty.MiddleName,
                                          LastName = primeLegalParty.LastName,
                                          NameSfx = primeLegalParty.NameSfx,
                                          RevenueAcct = primeLegalPartyRole.AcctId,
                                          LegalPartyRoleObjectType = primeLegalPartyRole.ObjectType
                                        };
        }
      }

      if ( assessmentRevisionDto != null )
      {
        assessmentHeader.AssessmentEvent.RevisionId = assessmentRevisionDto.Id;
        assessmentHeader.AssessmentEvent.ReferenceNumber = assessmentRevisionDto.ReferenceNumber;
        assessmentHeader.AssessmentEvent.ChangeReason = assessmentRevisionDto.ChangeReason;
        assessmentHeader.AssessmentEvent.Note = assessmentRevisionDto.Note;
      }

      return assessmentHeader;
    }

    private static LegalPartyRoleDto GetPrimeLegalPartyRole( IEnumerable<LegalPartyRoleDto> legalPartyRoles )
    {
      //There should always be at least one prime legal party.  If there
      //is more than one, for example in the case of joint tenants, then
      //take the first one in legal party role id order.
      return legalPartyRoles.OrderBy( t => t.Id ).FirstOrDefault( lpr => lpr.PrimeLegalParty == 1 && lpr.EffectiveStatus == EffectiveStatuses.Active );
    }

  }
}
