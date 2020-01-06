CREATE TABLE [Service.BaseValueSegment].[AsmtRevnBVS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AsmtRevnId] [int] NOT NULL,
	[BVSId] [int] NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
	[BVSStatusTypeId] [int] NOT NULL,
	[ReviewMessage] [varchar](1024) NOT NULL,
CONSTRAINT [PK_AsmtRevnBvs] PRIMARY KEY CLUSTERED ([Id] ASC), 
CONSTRAINT [FK_BVSAsmtRevn_BVSStatusType] FOREIGN KEY ([BVSStatusTypeId]) REFERENCES [Service.BaseValueSegment].[BVSStatusType]([Id]),
CONSTRAINT [FK_AsmtRevnBvs_AsmtRevn] FOREIGN KEY([AsmtRevnId]) REFERENCES [dbo].[AsmtRevn] ([Id]),
CONSTRAINT [FK_AsmtRevnBvs_BVS] FOREIGN KEY([BVSId]) REFERENCES [Service.BaseValueSegment].[BVS] ([Id])
) 
GO

CREATE INDEX [IX_AsmtRevnBVS_BVSId] ON [Service.BaseValueSegment].[AsmtRevnBVS] ([BVSId])
GO
CREATE NONCLUSTERED INDEX [IX_AsmtRevnBVS_BVSStatusTypeId] ON [Service.BaseValueSegment].[AsmtRevnBVS] ([BVSStatusTypeId])
