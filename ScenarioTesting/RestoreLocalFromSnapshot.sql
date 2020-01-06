 DECLARE @Database VARCHAR(256) = 'LumenGold'
 DECLARE @Snapshot VARCHAR(256) = 'LumenGoldSnapshot'

 DECLARE @Action          VARCHAR(32) = 'restore'       -- Create, Drop, else Restore

 --------


  IF UPPER(@Action) = 'CREATE'
  BEGIN
      IF EXISTS( SELECT 1 FROM sys.databases WHERE name = @Snapshot AND source_database_id IS NOT NULL )
      BEGIN
          EXEC [dbo].[Snapshot_RestoreDatabaseFromSnapshot]
              @SnapshotName = @Snapshot

          EXEC [dbo].[Snapshot_DeleteSnapshot]
            @SnapshotName = @Snapshot
      END      

      EXEC [dbo].[Snapshot_CreateSnapshot]
          @databaseName = @Database,
          @SnapshotName = @Snapshot

      PRINT 'Created'
  END
    
  IF UPPER(@Action) = 'DROP'
  BEGIN
     IF EXISTS( SELECT 1 FROM sys.databases WHERE name = @Snapshot AND source_database_id IS NOT NULL )
      BEGIN
          EXEC [dbo].[Snapshot_RestoreDatabaseFromSnapshot]
              @SnapshotName = @Snapshot

          EXEC [dbo].[Snapshot_DeleteSnapshot]
              @SnapshotName = @Snapshot
      END      

      PRINT 'Dropped'
  END

  IF UPPER(@Action) NOT IN('CREATE', 'DROP')
  BEGIN
      EXEC [dbo].[Snapshot_RestoreDatabaseFromSnapshot]
          @SnapshotName = @Snapshot

      PRINT 'Restored'
  END
  
