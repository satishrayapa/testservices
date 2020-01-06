CREATE TABLE [Service.BaseValueSegment].[BVSValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSValueHeaderId] [int] NOT NULL,
	[SubComponent] [int] NOT NULL,
	[ValueAmount] [decimal](28, 10) NOT NULL,
	[PctComplete] [decimal](14, 10) NOT NULL,
	[FullValueAmount] [decimal](28, 10) NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
    CONSTRAINT [PK_BVSValue] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_BVSValue_BVSValueHeader] FOREIGN KEY ([BVSValueHeaderId]) REFERENCES [Service.BaseValueSegment].[BVSValueHeader] ([Id])
) 
GO
CREATE NONCLUSTERED INDEX [IX_BVSValue_BVSValueHeaderId] ON [Service.BaseValueSegment].[BVSValue]([BVSValueHeaderId] ASC)