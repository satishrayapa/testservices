CREATE TABLE [Service.BaseValueSegment].[BVS]
([Id]                [INT] IDENTITY(1, 1) NOT NULL,
 [RevObjId]          [INT] NOT NULL,
 [AsOf]              [DATE] NOT NULL,
 [SequenceNumber]    [INT] NOT NULL,
 [TranId]            [BIGINT] NOT NULL,
 [DynCalcInstanceId] [INT] NOT NULL,
 CONSTRAINT [PK_BVS] PRIMARY KEY CLUSTERED([Id] ASC)
);
GO
CREATE UNIQUE INDEX [IX_BVS_AsOf_RevObjId_SequenceNumber] ON [Service.BaseValueSegment].[BVS]([AsOf], [RevObjId], [SequenceNumber]);