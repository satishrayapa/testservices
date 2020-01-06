
rem reset resource locator values
SQLCmd -E -S localhost -Q "USE [LumenGold] DELETE FROM [Common.Resource].[Resource] INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'BaseValueSegmentFeature', N'LOCALDEV', N'true') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'facade.assessmentheader', N'urlservices:LOCALDEV', N'http://localhost:50206') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'facade.basevaluesegment', N'urlservices:LOCALDEV', N'http://localhost:50205') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.assessmentevent', N'urlservices:LOCALDEV', N'http://localhost:50201') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.basevaluesegment', N'urlservices:LOCALDEV', N'http://localhost:50204') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.grmevent', N'urlservices:LOCALDEV', N'http://localhost:50207') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.legalparty', N'urlservices:LOCALDEV', N'http://localhost:50203') INSERT [Common.Resource].[Resource] ([Key], [Partition], [Value]) VALUES (N'service.revenueobject', N'urlservices:LOCALDEV', N'http://localhost:50202')"

rem set env variable
setx -m ASPNETCORE_ENVIRONMENT "LOCALDEV"

rem transform web config
start %~dp0ctt.exe s:C:\local_aa\sites\Aumentum\web.config t:%~dp0LumenTransformDev.config d:C:\local_aa\sites\Aumentum\web.config i

pause
