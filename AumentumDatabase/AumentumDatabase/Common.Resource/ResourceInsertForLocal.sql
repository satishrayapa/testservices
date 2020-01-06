USE [Riverside_CIDB]
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('BaseValueSegmentFeature'
           ,'LOCAL'
           ,'true')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('facade.assessmentheader'
           ,'urlservices:LOCAL'
           ,'http://localhost:50206')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('facade.basevaluesegment'
           ,'urlservices:LOCAL'
           ,'http://localhost:50205')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('service.assessmentevent'
           ,'urlservices:LOCAL'
           ,'http://localhost:50201')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('service.basevaluesgement'
           ,'urlservices:LOCAL'
           ,'http://localhost:50204')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('service.grmevent'
           ,'urlservices:LOCAL'
           ,'http://localhost:50207')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('service.legalparty'
           ,'urlservices:LOCAL'
           ,'http://localhost:50203')
GO

INSERT INTO [Common.Resource].[Resource]
           ([Key]
           ,[Partition]
           ,[Value])
     VALUES
           ('service.revenueobject'
           ,'urlservices:LOCAL'
           ,'http://localhost:50202')
GO