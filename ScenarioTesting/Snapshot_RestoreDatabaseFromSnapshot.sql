CREATE PROCEDURE [dbo].[Snapshot_RestoreDatabaseFromSnapshot]
    (
      @SnapshotName VARCHAR(256)
    )
AS /*
       Name          :      dbo.Snapshot_RestoreDatabaseFromSnapshot
       Description   :      Restore a database from a snapshot.
       Author        :      Scott Reister
       Created On    :      05/21/2015
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
        DECLARE @SourceDatabaseID INT;
        DECLARE @tSqlCommand NVARCHAR(MAX);
		        
        BEGIN TRY  
			------------------------------------------------------------------
            --Data Cleansing.
            SET @SnapshotID = DB_ID(@SnapshotName);

            SET @SourceDatabaseID = ( SELECT    source_database_id
                                      FROM      sys.databases
                                      WHERE     source_database_id IS NOT NULL
                                                AND database_id = @SnapshotID
                                    );
            ------------------------------------------------------------------
            --Input Validation logic.
            IF @SnapshotID IS NULL
                RAISERROR(N'Invalid Input: @SnapshotName: Cannot find a snapshot database by that name.',16,1);
            IF @SourceDatabaseID IS NULL
                RAISERROR(N'Invalid Input: @SnapshotName: Specified database is not a snapshot.',16,1);
            
            IF EXISTS ( SELECT  1
                        FROM    sys.databases
                        WHERE   source_database_id = @SourceDatabaseID
                                AND database_id <> @SnapshotID )
                RAISERROR(N'Invalid Enviroment: A database may only be reverted to a snapshot if all other of its snapshots have been dropped.',16,1);

        END TRY
        BEGIN CATCH
            SELECT  @ErrorMessage = ERROR_MESSAGE();
            SELECT  @ErrorSeverity = ERROR_SEVERITY();
            SELECT  @ErrorState = ERROR_STATE();
                     
            EXECUTE dbo.Log_Error N'dbo.Snapshot_RestoreDatabaseFromSnapshot';

            RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState);
        END CATCH;

        IF @ErrorSeverity IS NULL
            BEGIN
                BEGIN TRY
                    SET @tSqlCommand = ( 
					'
					ALTER DATABASE '+QUOTENAME(DB_NAME(@SourceDatabaseID))+' SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
					 
					RESTORE DATABASE '+QUOTENAME(DB_NAME(@SourceDatabaseID))+' FROM DATABASE_SNAPSHOT ='''+DB_NAME(@SnapshotID)+''';
					
					ALTER DATABASE '+QUOTENAME(DB_NAME(@SourceDatabaseID))+' SET MULTI_USER;
					
					' );
					--PRINT @tSqlCommand
                    EXECUTE sp_executesql @tSqlCommand;
                END TRY
                BEGIN CATCH

                    SELECT  @ErrorMessage = ERROR_MESSAGE();
                    SELECT  @ErrorSeverity = ERROR_SEVERITY();
                    SELECT  @ErrorState = ERROR_STATE();

                    EXECUTE dbo.Log_Error N'dbo.Snapshot_RestoreDatabaseFromSnapshot';

					SET @tSqlCommand = ( N'ALTER DATABASE '+QUOTENAME(DB_NAME(@SourceDatabaseID))+' SET MULTI_USER;
					
					');
					EXECUTE dbo.Log_Error N'dbo.Snapshot_RestoreDatabaseFromSnapshot';
					EXECUTE sp_executesql @tSqlCommand;
                    
					RAISERROR (@ErrorMessage, -- Message text.
                   @ErrorSeverity, -- Severity.
                   @ErrorState -- State.
                   );
                END CATCH;
            END;

    END;
