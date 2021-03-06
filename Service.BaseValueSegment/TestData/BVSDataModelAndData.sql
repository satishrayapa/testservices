USE [DevTest]
GO
ALTER TABLE [dbo].[BVSValue] DROP CONSTRAINT [BVSValueBVSTran]
GO
ALTER TABLE [dbo].[BVSTran] DROP CONSTRAINT [FK_BVSTran_BVS]
GO
ALTER TABLE [dbo].[BVSOwnerValue] DROP CONSTRAINT [BVSOwnerValueBVSTran]
GO
ALTER TABLE [dbo].[BVSOwner] DROP CONSTRAINT [FK_BVSValueBVSTran]
GO
ALTER TABLE [dbo].[BVSAsmtEventTran] DROP CONSTRAINT [FK_BVSAsmtEventTran_BVSTran]
GO
ALTER TABLE [dbo].[BVSAsmtEventTran] DROP CONSTRAINT [FK_BVSAsmtEventTran_AsmtEventTran]
GO
/****** Object:  Table [dbo].[BVSValue]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVSValue]
GO
/****** Object:  Table [dbo].[BVSTran]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVSTran]
GO
/****** Object:  Table [dbo].[BVSOwnerValue]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVSOwnerValue]
GO
/****** Object:  Table [dbo].[BVSOwner]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVSOwner]
GO
/****** Object:  Table [dbo].[BVSAsmtEventTran]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVSAsmtEventTran]
GO
/****** Object:  Table [dbo].[BVS]    Script Date: 3/10/2017 9:11:15 AM ******/
DROP TABLE [dbo].[BVS]
GO
/****** Object:  Table [dbo].[BVS]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BVS](
	[Id] [int] NOT NULL,
	[RevobjId] [int] NOT NULL,
	[TranId] [bigint] NOT NULL,
	[DynCalcInstanceId] [int] NOT NULL,
 CONSTRAINT [PK_BVS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BVSAsmtEventTran]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BVSAsmtEventTran](
	[Id] [int] NOT NULL,
	[BVSTranId] [int] NULL,
	[AsmtEventTranId] [int] NULL,
 CONSTRAINT [PK_BVSAsmtEventTran] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BVSOwner]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BVSOwner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSTranId] [int] NOT NULL,
	[BeneficialInterest] [int] NOT NULL,
	[Percent] [decimal](28, 10) NOT NULL,
	[GRMEventId] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
 CONSTRAINT [PK_BVSOwnerID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BVSOwnerValue]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BVSOwnerValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSTranId] [int] NOT NULL,
	[BVSOwnerId] [int] NOT NULL,
	[BVSValueId] [int] NOT NULL,
	[BaseValue] [decimal](28, 10) NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
 CONSTRAINT [PK_BVSOwnerValueID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BVSTran]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BVSTran](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSId] [int] NOT NULL,
	[TranId] [bigint] NOT NULL,
	[EffStatus] [char](1) NOT NULL,
	[AsOf] [datetime] NOT NULL,
	[BVSTranType] [int] NOT NULL,
	[AsmtRevnBVSId] [int] NULL,
	[GRMEventReasonCd] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NULL,
 CONSTRAINT [BVSTran0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BVSValue]    Script Date: 3/10/2017 9:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BVSValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSTranId] [int] NOT NULL,
	[SubComponent] [int] NOT NULL,
	[BaseYear] [smallint] NOT NULL,
	[ValueAmount] [decimal](28, 10) NOT NULL,
	[PctComplete] [decimal](14, 10) NOT NULL,
	[FullValueAmount] [decimal](28, 10) NOT NULL,
	[GRMEventId] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
 CONSTRAINT [BVSValue0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[BVS] ([Id], [RevobjId], [TranId], [DynCalcInstanceId]) VALUES (1, 100001, 10000, 0)
SET IDENTITY_INSERT [dbo].[BVSOwner] ON 

INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (1, 1, 1000, CAST(100.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (2, 2, 1000, CAST(70.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (3, 2, 2000, CAST(30.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (4, 3, 2000, CAST(30.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (5, 3, 2000, CAST(70.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (6, 4, 2000, CAST(30.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (7, 4, 2000, CAST(70.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (8, 5, 2000, CAST(30.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (9, 5, 2000, CAST(20.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSOwner] ([Id], [BVSTranId], [BeneficialInterest], [Percent], [GRMEventId], [DynCalcStepTrackingId]) VALUES (10, 5, 3000, CAST(50.0000000000 AS Decimal(28, 10)), 0, 0)
SET IDENTITY_INSERT [dbo].[BVSOwner] OFF
SET IDENTITY_INSERT [dbo].[BVSOwnerValue] ON 

INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (1, 1, 1, 1, CAST(100000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (2, 2, 2, 2, CAST(70000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (3, 2, 3, 3, CAST(60000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (4, 3, 4, 4, CAST(60000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (5, 3, 5, 5, CAST(154000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (6, 4, 6, 6, CAST(60000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (7, 4, 7, 7, CAST(154000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (8, 4, 6, 8, CAST(3000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (9, 4, 7, 8, CAST(7000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (10, 5, 8, 9, CAST(60000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (11, 5, 9, 10, CAST(44000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (12, 5, 8, 11, CAST(3000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (13, 5, 9, 11, CAST(2000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (14, 5, 10, 12, CAST(105000.0000000000 AS Decimal(28, 10)), 0)
INSERT [dbo].[BVSOwnerValue] ([Id], [BVSTranId], [BVSOwnerId], [BVSValueId], [BaseValue], [DynCalcStepTrackingId]) VALUES (15, 5, 10, 13, CAST(20000.0000000000 AS Decimal(28, 10)), 0)
SET IDENTITY_INSERT [dbo].[BVSOwnerValue] OFF
SET IDENTITY_INSERT [dbo].[BVSTran] ON 

INSERT [dbo].[BVSTran] ([Id], [BVSId], [TranId], [EffStatus], [AsOf], [BVSTranType], [AsmtRevnBVSId], [GRMEventReasonCd], [DynCalcStepTrackingId]) VALUES (1, 1, 10000, N'A', CAST(N'1776-07-04 00:00:00.000' AS DateTime), 100001, 0, 0, 0)
INSERT [dbo].[BVSTran] ([Id], [BVSId], [TranId], [EffStatus], [AsOf], [BVSTranType], [AsmtRevnBVSId], [GRMEventReasonCd], [DynCalcStepTrackingId]) VALUES (2, 1, 10000, N'A', CAST(N'2014-10-10 00:00:00.000' AS DateTime), 100001, 0, 0, 0)
INSERT [dbo].[BVSTran] ([Id], [BVSId], [TranId], [EffStatus], [AsOf], [BVSTranType], [AsmtRevnBVSId], [GRMEventReasonCd], [DynCalcStepTrackingId]) VALUES (3, 1, 10000, N'A', CAST(N'2015-10-10 00:00:00.000' AS DateTime), 100001, 0, 0, 0)
INSERT [dbo].[BVSTran] ([Id], [BVSId], [TranId], [EffStatus], [AsOf], [BVSTranType], [AsmtRevnBVSId], [GRMEventReasonCd], [DynCalcStepTrackingId]) VALUES (4, 1, 10000, N'A', CAST(N'2016-04-10 00:00:00.000' AS DateTime), 100001, 0, 0, 0)
INSERT [dbo].[BVSTran] ([Id], [BVSId], [TranId], [EffStatus], [AsOf], [BVSTranType], [AsmtRevnBVSId], [GRMEventReasonCd], [DynCalcStepTrackingId]) VALUES (5, 1, 10000, N'A', CAST(N'2016-07-10 00:00:00.000' AS DateTime), 100001, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[BVSTran] OFF
SET IDENTITY_INSERT [dbo].[BVSValue] ON 

INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (1, 1, 1000, 2010, CAST(100000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (2, 2, 1000, 2010, CAST(100000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (3, 2, 1000, 2015, CAST(200000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (4, 3, 1000, 2015, CAST(200000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (5, 3, 1000, 2016, CAST(220000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (6, 4, 1000, 2015, CAST(200000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (7, 4, 1000, 2016, CAST(220000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (8, 4, 1001, 2016, CAST(10000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (9, 5, 1000, 2015, CAST(200000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (10, 5, 1000, 2016, CAST(220000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (11, 5, 1001, 2016, CAST(10000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (12, 5, 1000, 2017, CAST(210000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
INSERT [dbo].[BVSValue] ([Id], [BVSTranId], [SubComponent], [BaseYear], [ValueAmount], [PctComplete], [FullValueAmount], [GRMEventId], [DynCalcStepTrackingId]) VALUES (13, 5, 1001, 2017, CAST(40000.0000000000 AS Decimal(28, 10)), CAST(0.0000000000 AS Decimal(14, 10)), CAST(0.0000000000 AS Decimal(28, 10)), 0, 0)
SET IDENTITY_INSERT [dbo].[BVSValue] OFF
ALTER TABLE [dbo].[BVSAsmtEventTran]  WITH CHECK ADD  CONSTRAINT [FK_BVSAsmtEventTran_AsmtEventTran] FOREIGN KEY([AsmtEventTranId])
REFERENCES [dbo].[BVSAsmtEventTran] ([Id])
GO
ALTER TABLE [dbo].[BVSAsmtEventTran] CHECK CONSTRAINT [FK_BVSAsmtEventTran_AsmtEventTran]
GO
ALTER TABLE [dbo].[BVSAsmtEventTran]  WITH CHECK ADD  CONSTRAINT [FK_BVSAsmtEventTran_BVSTran] FOREIGN KEY([BVSTranId])
REFERENCES [dbo].[BVSTran] ([Id])
GO
ALTER TABLE [dbo].[BVSAsmtEventTran] CHECK CONSTRAINT [FK_BVSAsmtEventTran_BVSTran]
GO
ALTER TABLE [dbo].[BVSOwner]  WITH CHECK ADD  CONSTRAINT [FK_BVSValueBVSTran] FOREIGN KEY([BVSTranId])
REFERENCES [dbo].[BVSTran] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BVSOwner] CHECK CONSTRAINT [FK_BVSValueBVSTran]
GO
ALTER TABLE [dbo].[BVSOwnerValue]  WITH CHECK ADD  CONSTRAINT [BVSOwnerValueBVSTran] FOREIGN KEY([BVSTranId])
REFERENCES [dbo].[BVSTran] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BVSOwnerValue] CHECK CONSTRAINT [BVSOwnerValueBVSTran]
GO
ALTER TABLE [dbo].[BVSTran]  WITH CHECK ADD  CONSTRAINT [FK_BVSTran_BVS] FOREIGN KEY([BVSId])
REFERENCES [dbo].[BVS] ([Id])
GO
ALTER TABLE [dbo].[BVSTran] CHECK CONSTRAINT [FK_BVSTran_BVS]
GO
ALTER TABLE [dbo].[BVSValue]  WITH CHECK ADD  CONSTRAINT [BVSValueBVSTran] FOREIGN KEY([BVSTranId])
REFERENCES [dbo].[BVSTran] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BVSValue] CHECK CONSTRAINT [BVSValueBVSTran]
GO
