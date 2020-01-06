using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddNServiceBusLegalPartySync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
									-- Create Schema Messaging
									IF NOT EXISTS(select  SCHEMA_NAME from information_schema.schemata where SCHEMA_NAME = 'messaging')
									BEGIN
										EXEC sp_executesql N'CREATE SCHEMA messaging'
									END


									declare @schema VARCHAR(100) = 'messaging'
									declare @tablePrefix VARCHAR(100) = 'LegalPartySync_'

									-- Create SubscriptionData table
									declare @tableNameSubscription nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'SubscriptionData]';

									if not exists(select * from sys.objects where object_id = object_id(@tableNameSubscription) and type in ('U'))
									begin
										declare @createTableSubscription nvarchar(max);
										set @createTableSubscription =
												'CREATE TABLE ' + @tableNameSubscription + 
												'(
													Subscriber nvarchar(200) not null,
													Endpoint nvarchar(200),
													MessageType nvarchar(200) not null,
													PersistenceVersion varchar(23) not null,
													primary key clustered(Subscriber, MessageType)
												)';
										
										EXEC sp_executesql @createTableSubscription;
									end

									-- Create Outbox Table
									declare @tableNameOutbox nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'OutboxData]';

									if not exists(select * from sys.objects where object_id = object_id(@tableNameOutbox) and type in ('U'))
									begin
										declare @createTableOutbox nvarchar(max);
										set @createTableOutbox =
												'CREATE TABLE ' + @tableNameOutbox + 
												'(
													MessageId nvarchar(200) not null primary key nonclustered,
													Dispatched bit not null default 0,
													DispatchedAt datetime,
													PersistenceVersion varchar(23) not null,
													Operations nvarchar(max) not null
												)';

										EXEC sp_executesql @createTableOutbox;
									end

									if not exists(select * from sys.indexes where name = 'Index_DispatchedAt' and object_id = object_id(@tableNameOutbox))
									begin

										declare @createDispatchedAtIndex nvarchar(max);
										set @createDispatchedAtIndex = 'CREATE INDEX Index_DispatchedAt  on ' + @tableNameOutbox + ' (DispatchedAt) where Dispatched = 1;';
										EXEC sp_executesql @createDispatchedAtIndex;

									end


									-- Create TimeOut Table
									declare @tableNameTimeOut nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'TimeoutData]';

									if not exists(select * from sys.objects where object_id = object_id(@tableNameTimeOut) and type in ('U'))
									begin
										declare @createTableTimeOut nvarchar(max);
										set @createTableTimeOut =
												'CREATE TABLE ' + @tableNameTimeOut + 
												'(
													Id uniqueidentifier not null primary key,
													Destination nvarchar(200),
													SagaId uniqueidentifier,
													State varbinary(max),
													Time datetime,
													Headers nvarchar(max) not null,
													PersistenceVersion varchar(23) not null
												)';

										EXEC sp_executesql @createTableTimeOut;
									end

									if not exists(select * from sys.indexes where name = 'Index_SagaId' and object_id = object_id(@tableNameTimeOut))
									begin
										declare @createSagaIdIndex nvarchar(max);
										set @createSagaIdIndex = 'CREATE INDEX Index_SagaId on ' + @tableNameTimeOut + '(SagaId);';
										EXEC sp_executesql @createSagaIdIndex;
									end

									if not exists(select * from sys.indexes where name = 'Index_Time' and object_id = object_id(@tableNameTimeOut))
									begin
										declare @createTimeIndex nvarchar(max);
										set @createTimeIndex = 'CREATE INDEX Index_Time on ' + @tableNameTimeOut + '(Time);';
										EXEC sp_executesql @createTimeIndex;
									end

								");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
									if exists(select * from sys.objects where object_id = object_id('[messaging].[audit]') and type in ('U'))
									begin
										drop table [messaging].[audit]
									end
									
									if exists(select * from sys.objects where object_id = object_id('[messaging].[error]') and type in ('U'))
									begin
										drop table [messaging].[error]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator_OutboxData]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator_OutboxData]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySync]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySync]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySync.Retries]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySync.Retries]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySync.Timeouts]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySync.Timeouts]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySync.TimeoutsDispatcher]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySync.TimeoutsDispatcher]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator.Retries]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator.Retries]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator.Timeouts]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator.Timeouts]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator.TimeoutsDispatcher]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator.TimeoutsDispatcher]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator_SubscriptionData]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator_SubscriptionData]
									end

									if exists(select * from sys.objects where object_id = object_id('[messaging].[LegalPartySyncCoordinator_TimeoutData]') and type in ('U'))
									begin
										drop table [messaging].[LegalPartySyncCoordinator_TimeoutData]
									end

									declare @schema VARCHAR(100)='messaging'
									declare @tablePrefix VARCHAR(100) = 'LegalPartySync_'

									-- Drop TimeOutData table
									declare @tableNameTimeoutData nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'TimeoutData]';

									if exists(select * from sys.objects where object_id = object_id(@tableNameTimeoutData) and type in ('U'))
									begin
										declare @dropTableTimeout nvarchar(max);
										set @dropTableTimeout = 'drop table ' + @tableNameTimeoutData;
										exec sp_executesql @dropTableTimeout;
									end

									-- Drop OutboxData table
									declare @tableNameOutbox nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'OutboxData]';

									if exists(select * from sys.objects where object_id = object_id(@tableNameOutbox) and type in ('U'))
									begin
										declare @dropTableOutbox nvarchar(max);
										set @dropTableOutbox = 'drop table ' + @tableNameOutbox;
										exec sp_executesql @dropTableOutbox;
									end

									-- Drop SubscriptionData table
									declare @tableNameSubscription nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'SubscriptionData]';

									if exists(select * from sys.objects where object_id = object_id(@tableNameSubscription) and type in ('U'))
									begin
										declare @dropTableSubscription nvarchar(max);
										set @dropTableSubscription = 'drop table ' + @tableNameSubscription;
										exec sp_executesql @dropTableSubscription;
									end

									-- Drop Schema Messaging
									IF EXISTS(select  SCHEMA_NAME from information_schema.schemata where SCHEMA_NAME = 'messaging')
									BEGIN
										EXEC sp_executesql N'DROP SCHEMA messaging'
									END
								");
		}
    }
}
