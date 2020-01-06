CREATE PROCEDURE [dbo].[Snapshot_DeleteSnapshot]
    (
      @SnapshotName VARCHAR(256)
    )
AS /*
       Name          :      dbo.Snapshot_DeleteSnapshot
       Description   :      Delete a snapshot of a database
       Author        :      Scott Reister
       Created On    :      05/20/2015
       Method        :      

       Notes         :      
                                         

       */
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT OFF;
        SET NOCOUNT ON;
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        DECLARE @SnapshotID INT;
        DECLARE @tSqlCommand NVARCHAR(MAX);
		        
        BEGIN TRY  
			------------------------------------------------------------------
            --Data Cleansing.
            SET @SnapshotID = DB_ID(@SnapshotName);

            ------------------------------------------------------------------
            --Input Validation logic.
            IF @SnapshotID IS NULL
                RAISERROR(N'Invalid Input: @SnapshotName: Cannot find a snapshot by that name.',16,1);
            IF NOT EXISTS ( SELECT  1
                            FROM    sys.databases
                            WHERE   source_database_id IS NOT NULL
                                    AND database_id = @SnapshotID )
                RAISERROR(N'Invalid Input: @SnapshotName: Target database is not a snapshot database.',16,1);

        END TRY
        BEGIN CATCH
            SELECT  @ErrorMessage = ERROR_MESSAGE();
            SELECT  @ErrorSeverity = ERROR_SEVERITY();
            SELECT  @ErrorState = ERROR_STATE();
                     
            EXECUTE dbo.Log_Error N'dbo.Snapshot_DeleteSnapshot';

            RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
        END CATCH;

        IF @ErrorSeverity IS NULL
            BEGIN
                BEGIN TRY
                    SET @tSqlCommand = ( 'DROP DATABASE '
                                         + QUOTENAME(DB_NAME(@SnapshotID)) );
                    EXECUTE sp_executesql @tSqlCommand;
                END TRY
                BEGIN CATCH

                    SELECT  @ErrorMessage = ERROR_MESSAGE();
                    SELECT  @ErrorSeverity = ERROR_SEVERITY();
                    SELECT  @ErrorState = ERROR_STATE();

                    EXECUTE dbo.Log_Error N'dbo.Snapshot_DeleteSnapshot';



                    RAISERROR (@ErrorMessage, -- Message text.
                   @ErrorSeverity, -- Severity.
                   @ErrorState -- State.
                   );
                END CATCH;
            END;

    END;
