using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TAGov.Services.Core.AssessmentEvent.Repository.Interfaces.V1;
using TAGov.Services.Core.AssessmentEvent.Repository.Models.V1;

namespace TAGov.Services.Core.AssessmentEvent.Repository.Implementation.V1
{
  public class StatutoryReferenceRepository : IStatutoryReferenceRepository
  {
    private readonly AssessmentEventContext _assessmentEventContext;

    public StatutoryReferenceRepository( AssessmentEventContext assessmentEventContext )
    {
      _assessmentEventContext = assessmentEventContext;
    }

    public StatutoryReference GetStatutoryReferenceByAssessmentTransactionId( int assessmentTransactionId )
    {
      const string sql = @"      
	DECLARE @SysType         INT; EXEC aa_getSysTypeId   'Object Type',   'SysType', @SysType OUTPUT;
  DECLARE @SysTypeCatId    INT; EXEC aa_GetSysTypeCatId 'StatutoryReason ',   @SysTypeCatId   OUTPUT;
	DECLARE @AsmtEventTranType   INT; 
	EXEC aa_getSysTypeId  'Object Type',   'AsmtEventTran', @AsmtEventTranType OUTPUT;
	DECLARE @GRMModules_AA   INT; 
	EXEC aa_getSysTypeId  'GRMModules',     'AA',      @GRMModules_AA OUTPUT;
    DECLARE @RtCode           varchar(4000)=''
    DECLARE @EventId INT

      create table #EventDtls
      (
      EventId Int,
      dateVal  varchar(4000)
      )
      insert into #EventDtls

      select GEAtt.GRMEventId,GEAtt.DataValue  from GRMEventArtifact GEArt

      inner join GRMEventAttribute GEAtt
      on GEArt.GRMEventId=GEAtt.GRMEventId

      where GEArt.ObjectType=@AsmtEventTranType and GEArt.ObjectId=@AsmtEventTranId --Pass AsmtTranId as parameter

      order by GEAtt.DataValue
      select top(1) @EventId=  EventId from #EventDtls order by dateVal desc

      create table #StatutoryReferences
      (
      SysTypeId int,
      Descr varchar(100),
      ShortDescr varchar(100),
      ObjectScopeId int null,
      IsExcluded bit null,
      Level int null
      )
      insert into #StatutoryReferences
      exec dbo.grm_common_ReasonStatutoryReferences  @SysTypeCatId, @SysType, @GRMModules_AA, '', ''

      create table #RTCodes
      (
      Id int identity(1,1),
      Descr varchar(100),
      )
      insert into #RTCodes
      select  Descr
      from #StatutoryReferences SR
      inner Join (select * from GRMEventArtifact where  GRMEventId= @EventId and ObjectType=@SysType) GEA
      on SR.SysTypeId=GEA.ObjectId

      select @RtCode=
      coalesce (case when @RtCode = ''
      then rtrim(Descr)
      else @RtCode + ' , ' + rtrim(Descr)
      end
      ,'') from #RTCodes where Id <=5
      if((select count(*) from #RTCodes)>5)
      begin
      select @RtCode= @RtCode+' , More...'
      end

	  select 0 AS [key], @RtCode AS [Description]";
      return
        _assessmentEventContext.StatutoryReference.FromSql( sql,
                                                            // ReSharper disable once FormatStringProblem
                                                            new SqlParameter( "@AsmtEventTranId", SqlDbType.Int ) { Value = assessmentTransactionId } ).Single();
    }
  }
}
