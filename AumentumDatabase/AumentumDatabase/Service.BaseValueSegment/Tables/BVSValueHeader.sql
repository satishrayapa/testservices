CREATE TABLE [Service.BaseValueSegment].[BVSValueHeader]
(
    [Id]  [int] IDENTITY(1,1) NOT NULL, 
    [BVSTranId] INT NOT NULL, 
    [GRMEventId] INT NULL, 
    [BaseYear] INT NOT NULL, 
    CONSTRAINT [PK_BVSValueHeader] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_BVSValueHeader_BVSTran] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_BVSValueHeader_BVSTranId] ON [Service.BaseValueSegment].[BVSValueHeader]([BVSTranId] ASC)