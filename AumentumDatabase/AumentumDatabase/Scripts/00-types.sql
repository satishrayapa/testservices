

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSStatusType WHERE Name = 'New' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSStatusType (Id, Name, Description)
          VALUES  (   1
                    , 'New'
                    , 'New'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSStatusType WHERE Name = 'Review' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSStatusType (Id, Name, Description)
          VALUES  (   2
                    , 'Review'
                    , 'Review'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSStatusType WHERE Name = 'Completed' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSStatusType (Id, Name, Description)
          VALUES  (   3
                    , 'Completed'
                    , 'Completed'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSTranType WHERE Name = 'Calculated' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSTranType (Id, Name, Description)
          VALUES  (   1
                    , 'Calculated'
                    , 'Calculated'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSTranType WHERE Name = 'User' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSTranType (Id, Name, Description)
          VALUES  (   2
                    , 'User'
                    , 'User'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSTranType WHERE Name = 'Conversion' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSTranType (Id, Name, Description)
          VALUES  (   3
                    , 'Conversion'
                    , 'Conversion'   )
END

IF NOT EXISTS (SELECT 1 FROM [Service.BaseValueSegment].BVSTranType WHERE Name = 'User Deleted' )
BEGIN
    INSERT INTO [Service.BaseValueSegment].BVSTranType (Id, Name, Description)
          VALUES  (   4
                    , 'User Deleted'
                    , 'User Deleted'   )
END
