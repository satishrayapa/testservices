CREATE TABLE [Service.BaseValueSegment].[BVSTran](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSId] [int] NOT NULL,
	[TranId] [bigint] NOT NULL,
	[EffStatus] [char](1) NOT NULL,
	[BVSTranTypeId] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NULL,
   CONSTRAINT [BVSTran0] PRIMARY KEY CLUSTERED ([Id] ASC), 
   CONSTRAINT [FK_BVSTran_BVSTranType] FOREIGN KEY ([BVSTranTypeId]) REFERENCES [Service.BaseValueSegment].[BVSTranType]([Id]),
   CONSTRAINT [FK_BVSTran_BVS] FOREIGN KEY([BVSId]) REFERENCES [Service.BaseValueSegment].[BVS] ([Id])
) 
GO
CREATE NONCLUSTERED INDEX [IX_BVSTran_BVSId] ON [Service.BaseValueSegment].[BVSTran] ([BVSId] ASC)
GO
CREATE NONCLUSTERED INDEX [IX_BVSTran_BVSTranTypeId] ON [Service.BaseValueSegment].[BVSTran]([BVSTranTypeId] ASC)