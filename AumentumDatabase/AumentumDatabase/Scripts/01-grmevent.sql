SET NOCOUNT ON

  DECLARE @TranId BIGINT = -1001

  DECLARE @GRMEventGroup  INT; EXEC grm_GetSysTypeId 'GRMEventGroup', 'BVSMaint', @GRMEventGroup OUTPUT
  DECLARE @GRMEventType   INT; EXEC grm_GetSysTypeId 'GRMEventType', 'BVSValueHdrOvr', @GRMEventType OUTPUT
  DECLARE @AA             INT; EXEC grm_GetSysTypeId 'GRMModules', 'AA', @AA OUTPUT


  IF NOT EXISTS( SELECT 1 FROM GRMEventGroup WHERE TranId = @TranId AND EventGroupType = @GRMEventGroup)
  BEGIN
          DECLARE @groupId INT; SELECT @groupId = ISNULL(MAX(Id) + 1, 1) FROM GRMEventGroup
       
          INSERT INTO GRMEventGroup (Id, TranId, UserProfileId, StartDate, EndDate, EventGroupType, DocNumber, Info, ParentGRMEventGroupId)
                VALUES  (   @groupId
                          , @TranId
                          , 0
                          , '7-4-1776'
                          , '7-4-1776'
                          , @GRMEventGroup
                          , 0
                          , ''
                          , 0       )

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' GRMEventGroups added'
  END


  IF NOT EXISTS(SELECT 1 FROM GRMEvent WHERE TranId = @TranId AND EventType = @GRMEventType)
  BEGIN
          DECLARE @GRMEventGroupId INT; SELECT @GRMEventGroupId = MAX(Id) FROM GRMEventGroup WHERE TranId = @TranId AND EventGroupType = @GRMEventGroup

          DECLARE @eventId INT; SELECT @eventId = ISNULL(MAX(Id) + 1, 1) FROM GRMEvent
          INSERT INTO GRMEvent (Id, TranId, GRMEventGroupId, ObjectType, ObjectId, TaxBillId, EventType, EventDate, GRMModule, BillTypeId, BillNumber, TaxYear, RevObjId, PIN, Info, EffDate, EffTaxYear)
                VALUES  (   @eventId
                          , @TranId
                          , @GRMEventGroupId
                          , 0
                          , 0
                          , 0
                          , @GRMEventType
                          , GETDATE()
                          , @AA
                          , 0
                          , ''
                          , 0
                          , 0
                          , ''
                          , ''
                          , '7/4/1776'
                          , 0             )

        PRINT '  ' +  CONVERT(VARCHAR(100), @@ROWCOUNT) + ' GRMEvents added'

  END