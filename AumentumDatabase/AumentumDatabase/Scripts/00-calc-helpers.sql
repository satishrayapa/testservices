IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Service.BaseValueSegment].[aa_GetBVSTranType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
  DROP PROCEDURE [Service.BaseValueSegment].[aa_GetBVSTranType]
GO

    CREATE PROCEDURE [Service.BaseValueSegment].[aa_GetBVSTranType]
        @p_Name   VARCHAR(50)
      , @p_Id     INT OUTPUT
    AS
    BEGIN
      SET NOCOUNT ON
      
      SET @p_Name = RTRIM(@p_Name)
      SET @p_Id = NULL

      SELECT @p_Id = Id FROM [Service.BaseValueSegment].[BVSTranType] WHERE Name = RTRIM(@p_Name)

      IF @p_Id IS NULL
      BEGIN
        RAISERROR (N'BVS tran type: %s.', 11, 1, @p_Name)
      END
    END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Service.BaseValueSegment].[aa_GetBVSStatusType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
  DROP PROCEDURE [Service.BaseValueSegment].[aa_GetBVSStatusType]
GO

    CREATE PROCEDURE [Service.BaseValueSegment].[aa_GetBVSStatusType]
        @p_Name   VARCHAR(50)
      , @p_Id     INT OUTPUT
    AS
    BEGIN
      SET NOCOUNT ON
      
      SET @p_Name = RTRIM(@p_Name)
      SET @p_Id = NULL

      SELECT @p_Id = Id FROM [Service.BaseValueSegment].[BVSStatusType] WHERE Name = RTRIM(@p_Name)

      IF @p_Id IS NULL
      BEGIN
        RAISERROR (N'BVS status type: %s.', 11, 1, @p_Name)
      END
    END
GO