CREATE PROCEDURE [dbo].[Snapshot_CreateSnapshot]
    (
      @databaseName VARCHAR(MAX),  --'Aumentum'
	  @SnapshotName VARCHAR(256) OUTPUT
    )
AS /*
       Name          :      dbo.Snapshot_CreateSnapshot
       Description   :      Create a snapshot of a database
       Author        :      Scott Reister
       Created On    :      05/19/2015
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


		DECLARE @Token CHAR(8);
		DECLARE @DatabaseID INT;
        DECLARE @tSqlCommand NVARCHAR(MAX)
		        
        BEGIN TRY  
			------------------------------------------------------------------
            --Data Cleansing.
			SET @Token = CONVERT(VARCHAR(8), HASHBYTES('MD2',CONVERT(VARCHAR(64), NEWID())), 2);
			SET @DatabaseID = DB_ID(@DatabaseName);
			IF @SnapshotName IS NULL 
				SET @SnapshotName = QUOTENAME(DB_NAME(@DatabaseID)+'$'+@Token)



            ------------------------------------------------------------------
            --Input Validation logic.
            IF @DatabaseID IS NULL
                RAISERROR(N'Invalid Input: @databaseName: Database not present on server.',16,1)

			IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @SnapshotName)
				RAISERROR(N'Invlid Input: @SnapshotName: Name already in use.',16,1)
        END TRY
        BEGIN CATCH
            SELECT  @ErrorMessage = ERROR_MESSAGE();
            SELECT  @ErrorSeverity = ERROR_SEVERITY();
            SELECT  @ErrorState = ERROR_STATE();
                     
            EXECUTE dbo.Log_Error N'dbo.Snapshot_CreateSnapshot'

            RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState)
        END CATCH

        IF @ErrorSeverity IS NULL
            BEGIN
                BEGIN TRY
                    SET @tSqlCommand = ( SELECT 'CREATE DATABASE ' + @SnapshotName + ' ON '
                            + CHAR(10) + STUFF(BASE.files_list, 1, 2, '')
                            + CHAR(10) + ' AS SNAPSHOT OF '
                            + QUOTENAME(DB_NAME(@DatabaseID))
                     FROM   ( SELECT    ', (NAME = ' + QUOTENAME(name)
                                        + ', FILENAME = '''
                                        + REVERSE(STUFF(REVERSE(physical_name),
                                                        1, 4, '')) + '$'
                                        + @Token + '.ss' + ''')' + CHAR(10) AS [text()]
                              FROM      sys.master_files
                              WHERE     type = 0
                                        AND database_id = DB_ID(@DatabaseName)
                            FOR
                              XML PATH('')
                            ) BASE ( files_list )
                   );
                    EXECUTE sp_executesql @tSqlCommand
                END TRY
                BEGIN CATCH

                    SELECT  @ErrorMessage = ERROR_MESSAGE();
                    SELECT  @ErrorSeverity = ERROR_SEVERITY();
                    SELECT  @ErrorState = ERROR_STATE();

                    EXECUTE dbo.Log_Error N'dbo.Snapshot_CreateSnapshot'



                    RAISERROR (@ErrorMessage, -- Message text.
                   @ErrorSeverity, -- Severity.
                   @ErrorState -- State.
                   );
                END CATCH
            END

    END
