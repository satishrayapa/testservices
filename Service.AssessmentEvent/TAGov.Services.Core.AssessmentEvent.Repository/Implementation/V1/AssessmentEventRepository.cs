using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Implementation.V1
{
  public class AssessmentEventRepository : IAssessmentEventRepository
  {
    private readonly AssessmentEventContext _assessmentEventContext;
    private readonly QueryOnlyAssessmentEventContext _queryOnlyAssessmentEventContext;

    private readonly DateTime _maxDateTime = new DateTime( 9999, 12, 31 );

    private const string ActiveEffectiveStatus = "A";

    public AssessmentEventRepository( AssessmentEventContext assessmentEventContext,
                                      QueryOnlyAssessmentEventContext queryOnlyAssessmentEventContext )
    {
      _assessmentEventContext = assessmentEventContext;
      _queryOnlyAssessmentEventContext = queryOnlyAssessmentEventContext;
    }

    public async Task<Models.V1.AssessmentEvent> GetAsync( int id )
    {
      var result = await _assessmentEventContext.AssessmentEvent.Where( x => x.Id == id ).Include( a => a.AssessmentEventTransactions )
                                                .Select( ae => new Models.V1.AssessmentEvent
                                                               {
                                                                 Id = ae.Id,
                                                                 AsmtEventTypeDescription =
                                                                   _assessmentEventContext.SystemType.Where( sysType => sysType.Id == ae.AsmtEventType ) //An assessment event should always have an AsmtEventType so catastropic failure if there is none
                                                                                          .OrderByDescending( orderedSysType => orderedSysType.begEffDate )
                                                                                          .First() //An assessment event type id should always have at least one matching systype so catastrophic failure if there is none
                                                                                          .descr.Trim(),
                                                                 AsmtEventType = ae.AsmtEventType,
                                                                 DynCalcStepTrackingId = ae.DynCalcStepTrackingId,
                                                                 EventDate = ae.EventDate,
                                                                 RevObjId = ae.RevObjId,
                                                                 TaxYear = ae.TaxYear,
                                                                 TranId = ae.TranId,
                                                                 AssessmentEventTransactions = ae.AssessmentEventTransactions
                                                               } ).SingleOrDefaultAsync();

      result?.AssessmentEventTransactions.ForEach( x => x.AsmtEventStateDescription = GetAsmtEventStateDescription( x ) );

      return result;
    }

    private string GetAsmtEventStateDescription( AssessmentEventTransaction aet )
    {
      var desc = _assessmentEventContext.SystemType.Where( sysType => sysType.Id == aet.AsmtEventState )
                                        //An assessment event should always have an AsmtEventState so catastropic failure if there is none
                                        .OrderByDescending( orderedSysType => orderedSysType.begEffDate )
                                        .First()
                                        //An assessment event type id should always have at least one matching systype so catastrophic failure if there is none
                                        .shortDescr.Trim();

      return desc;
    }

    public AssessmentRevision GetAssessmentRevisionByAssessmentRevisionEventId( int assessmentRevisionEventId, DateTime effectiveDate )
    {
      return
        _assessmentEventContext.AssessmentRevision.Join(
                                 _assessmentEventContext.AssessmentRevisionEvent.Where(
                                   assessmentRevisionEvent => assessmentRevisionEvent.Id == assessmentRevisionEventId ),
                                 assessmentRevision => assessmentRevision.Id,
                                 assessmentRevisionEvent => assessmentRevisionEvent.AsmtRevnId,
                                 ( assessmentRevision, assessmentRevisionEvent ) =>
                                   new { ar = assessmentRevision, are = assessmentRevisionEvent } )
                               .Select(
                                 assessmentRevision => new AssessmentRevision
                                                       {
                                                         Id = assessmentRevision.ar.Id,
                                                         ReferenceNumber = assessmentRevision.ar.ReferenceNumber.Trim(),
                                                         ValChangeReason = assessmentRevision.ar.ValChangeReason,
                                                         ChangeReason =
                                                           _assessmentEventContext.SystemType
                                                                                  .Single(
                                                                                    st => ( st.Id == assessmentRevision.ar.ValChangeReason )
                                                                                          && ( st.begEffDate ==
                                                                                               ( _assessmentEventContext.SystemType
                                                                                                                        .Where(
                                                                                                                          sub => ( sub.begEffDate <= _maxDateTime )
                                                                                                                                 && ( sub.Id == st.Id ) )
                                                                                                                        //If no rows are returned then there is nothing to max and we should throw
                                                                                                                        //but Max has to handle this possibility otherwise the LINQ engine won't
                                                                                                                        //convert Max to SQL
                                                                                                                        //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                                                                                                        .Max( new Func<SystemType, DateTime?>( sub => sub.begEffDate ) ) ?? DateTime.MaxValue ) ) )
                                                                                  .descr.Trim(),
                                                         Note = GetNote( assessmentRevision.ar.Id, effectiveDate ),
                                                       } )
                               .SingleOrDefault();
    }

    private string GetNote( int assessmentRevisionId, DateTime effectiveDate )
    {
      //these two locals are the LINQ equivalent of stored procedure aa_GetSysTypeId
      var objectTypeSysTypeCatId =
        _assessmentEventContext.SysTypeCat
                               .Single( t1 => ( t1.EffectiveStatus == ActiveEffectiveStatus ) &&
                                              ( t1.BeginEffectiveDate
                                                == ( _assessmentEventContext.SysTypeCat.Where(
                                                                              t2 => t2.Id == t1.Id )
                                                                            //If no rows are returned then there is nothing to max and we should throw
                                                                            //but Max has to handle this possibility otherwise the LINQ engine won't
                                                                            //convert Max to SQL
                                                                            //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                                                            .Max( new Func<SysTypeCat, DateTime?>( t3 => t3.BeginEffectiveDate ) ) ?? DateTime.MaxValue ) ) &&
                                              ( t1.ShortDescription == "Object Type" ) )
                               .Id;
      var asmtEventSysTypeId =
        _assessmentEventContext.SystemType
                               .Single( t1 => ( t1.effStatus == ActiveEffectiveStatus ) &&
                                              ( t1.begEffDate == ( _assessmentEventContext.SystemType.Where(
                                                                                            t2 => ( t2.Id == t1.Id ) )
                                                                                          //If no rows are returned then there is nothing to max and we should throw
                                                                                          //but Max has to handle this possibility otherwise the LINQ engine won't
                                                                                          //convert Max to SQL
                                                                                          //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                                                                          .Max( new Func<SystemType, DateTime?>( t3 => t3.begEffDate ) ) ?? DateTime.MaxValue ) ) &&
                                              ( t1.sysTypeCatId == objectTypeSysTypeCatId ) &&
                                              ( t1.shortDescr == "AsmtRevn" ) )
                               .Id;

      var publicUseSysTypeId =
        _assessmentEventContext.SystemType
                               .Single( t1 => ( t1.effStatus == ActiveEffectiveStatus ) &&
                                              ( t1.begEffDate == ( _assessmentEventContext.SystemType.Where(
                                                                                            t2 => ( t2.Id == t1.Id ) )
                                                                                          //If no rows are returned then there is nothing to max and we should throw
                                                                                          //but Max has to handle this possibility otherwise the LINQ engine won't
                                                                                          //convert Max to SQL
                                                                                          //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                                                                          .Max( new Func<SystemType, DateTime?>( t3 => t3.begEffDate ) ) ?? DateTime.MaxValue ) ) &&
                                              ( t1.shortDescr == "Public use" ) )
                               .Id;
      var asmtRevnNote = GetNotesByEffectiveDate(
          effectiveDate )
        .SingleOrDefault( note => ( note.ObjectType == asmtEventSysTypeId )
                                  && ( note.Privacy == publicUseSysTypeId )
                                  && ( note.ObjectId == assessmentRevisionId ) );
      if ( asmtRevnNote != null )
      {
        return asmtRevnNote.NoteText;
      }
      else
      {
        return "";
      }
    }

    private IQueryable<Note> GetNotesByEffectiveDate( DateTime effectiveDate )
    {
      return _assessmentEventContext.Note.Where(
        n => ( n.BeginEffectiveDate ==
               ( _assessmentEventContext.Note.Where(
                                          notesub => ( notesub.BeginEffectiveDate <= effectiveDate.AddDays( 1 ) )
                                                     && ( notesub.Id == n.Id ) )
                                        //If no rows are returned then there is nothing to max and we should throw
                                        //but Max has to handle this possibility otherwise the LINQ engine won't
                                        //convert Max to SQL
                                        //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                        .Max( new Func<Note, DateTime?>( notesub => notesub.BeginEffectiveDate ) ) ?? DateTime.MaxValue ) )
             && ( n.EffectiveStatus == ActiveEffectiveStatus ) );
    }

    public async Task<AssessmentEventValue> GetAssessmentEventValueByAssessmentEventTransactionIdAsync( int assessmentEventTransactionId )
    {
      AssessmentEventValue assessmentEventValue = await
                                                    _assessmentEventContext.AssessmentEventValue.SingleOrDefaultAsync(
                                                      aev => aev.AsmtEventTranId == assessmentEventTransactionId
                                                             && aev.ValueType.ShortDescr == "BaseYear4Asmt" );

      if ( assessmentEventValue != null )
      {
        assessmentEventValue.Attribute2Description
          = ( await _assessmentEventContext.SystemType
                                           .SingleOrDefaultAsync( t1 => t1.effStatus == ActiveEffectiveStatus &&
                                                                        t1.begEffDate == ( _assessmentEventContext.SystemType.Where(
                                                                                                                    t2 => ( t2.Id == t1.Id ) )
                                                                                                                  //If no rows are returned then there is nothing to max and we should throw
                                                                                                                  //but Max has to handle this possibility otherwise the LINQ engine won't
                                                                                                                  //convert Max to SQL
                                                                                                                  //see https://github.com/aspnet/EntityFrameworkCore/issues/7901
                                                                                                                  .Max( new Func<SystemType, DateTime?>( t3 => t3.begEffDate ) ) ?? DateTime.MaxValue ) &&
                                                                        t1.Id == assessmentEventValue.Attribute2 ) )?.descr.Trim();
      }

      return assessmentEventValue;
    }

    public IEnumerable<Models.V1.AssessmentEvent> List( int revenueObjectId, DateTime eventDate )
    {
      var startOfDay = eventDate.Date;
      var endOfDay = eventDate.Date.AddDays( 1 ).AddSeconds( -1 );
      return _assessmentEventContext.AssessmentEvent.Where( x => x.RevObjId == revenueObjectId && x.EventDate >= startOfDay && x.EventDate <= endOfDay );
    }

    public async Task<IEnumerable<RevenueObjectBasedAssessmentEvent>> ListAsync( int assessmentEventId )
    {
      return await ( from r in _queryOnlyAssessmentEventContext.RevenueObjectBasedAssessmentEvents
                     join e in _queryOnlyAssessmentEventContext.RevenueObjectBasedAssessmentEvents on r.RevObjId equals e.RevObjId
                     where e.Id == assessmentEventId
                     select r ).ToListAsync();
    }

    public async Task<AssessmentEventIsEditableFlag> GetIsEditableFlag( int assessmentEventId )
    {
      // First get Max(AsmtRevnId) from AsmtRevnEvent
      int asmtRevnId = _assessmentEventContext.AssessmentRevisionEvent
                                              .Where( x => x.AssessmentEventId == assessmentEventId )
                                              .Select( x => x.AsmtRevnId )
                                              .Max();

      // Then return AsmtEventId from this AsmtRevn
      return await (from r in _assessmentEventContext.AssessmentRevision
                    where r.Id == asmtRevnId
                    select new AssessmentEventIsEditableFlag
                    {
                      IsEditable = r.Active == 1,
                      AssessmentEventId = assessmentEventId
                    }).SingleAsync();
    }

    public async Task<int> GetCurrentRevisionId( int assessmentEventId )
    {
      return await _assessmentEventContext.AssessmentRevisionEvent.Where( x => x.AssessmentEventId == assessmentEventId )
                                          .MaxAsync( y => y.AsmtRevnId );
    }

    public async Task<AssessmentEventEffectiveDate> GetEffectiveDate( int assessmentEventId )
    {
       var assessmentRevisionId = _assessmentEventContext.AssessmentRevisionEvent.Where(x => x.AssessmentEventId == assessmentEventId)
                                    .Select(x=>x.AsmtRevnId)
                                    .Max();

       return await (from r in _assessmentEventContext.AssessmentRevision
                          where r.Id == assessmentRevisionId
                          select new AssessmentEventEffectiveDate
                          {
                              EffectiveDate = r.RevisionDate,
                              AssessmentEventId = assessmentEventId
                          }).SingleAsync();
     }
  }
}