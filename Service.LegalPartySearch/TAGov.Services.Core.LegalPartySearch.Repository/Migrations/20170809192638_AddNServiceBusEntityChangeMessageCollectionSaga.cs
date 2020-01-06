using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
	public partial class AddNServiceBusEntityChangeMessageCollectionSaga : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
declare @schema VARCHAR(100)='messaging'
declare @tablePrefix VARCHAR(100)='LegalPartySyncCoordinator_'

/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'EntityChangeMessageCollectionSaga]';

/* Initialize */

/* CreateTable */

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Id uniqueidentifier not null primary key,
        Metadata nvarchar(max) not null,
        Data nvarchar(max) not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null
    )
';
exec(@createTable);
end

/* AddProperty CorrelationId */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_CorrelationId' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_CorrelationId nvarchar(max);
  set @createColumn_CorrelationId = '
  alter table ' + @tableName + N'
    add Correlation_CorrelationId bigint;';
  exec(@createColumn_CorrelationId);
end

/* VerifyColumnType Int */

declare @dataType_CorrelationId nvarchar(max);
set @dataType_CorrelationId = (
  select data_type
  from information_schema.columns
  where
    table_name = ' + @tableName + N' and
    column_name = 'Correlation_CorrelationId'
);
if (@dataType_CorrelationId <> 'bigint')
  begin
    declare @error_CorrelationId nvarchar(max) = N'Incorrect data type for Correlation_CorrelationId. Expected bigint got ' + @dataType_CorrelationId + '.';
    throw 50000, @error_CorrelationId, 0
  end

/* WriteCreateIndex CorrelationId */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_CorrelationId' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_CorrelationId nvarchar(max);
  set @createIndex_CorrelationId = N'
  create unique index Index_Correlation_CorrelationId
  on ' + @tableName + N'(Correlation_CorrelationId)
  where Correlation_CorrelationId is not null;';
  exec(@createIndex_CorrelationId);
end

/* PurgeObsoleteIndex */

declare @dropIndexQuery nvarchar(max);
select @dropIndexQuery =
(
    select 'drop index ' + name + ' on ' + @tableName + ';'
    from sysindexes
    where
        Id = object_id(@tableName) and
        Name is not null and
        Name like 'Index_Correlation_%' and
        Name <> N'Index_Correlation_CorrelationId'
);
exec sp_executesql @dropIndexQuery

/* PurgeObsoleteProperties */

declare @tableNameWithoutSchema nvarchar(max) = @tablePrefix + N'EntityChangeMessageCollectionSaga';

declare @dropPropertiesQuery nvarchar(max);
select @dropPropertiesQuery =
(
    select 'alter table ' + @tableName + ' drop column ' + column_name + ';'
    from information_schema.columns
    where
        table_name = @tableNameWithoutSchema and
        table_schema = @schema and
        column_name like 'Correlation_%' and
        column_name <> N'Correlation_CorrelationId'
);
exec sp_executesql @dropPropertiesQuery

/* CompleteSagaScript */

");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
declare @schema VARCHAR(100)='messaging'
declare @tablePrefix VARCHAR(100)='LegalPartySyncCoordinator_'

/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'EntityChangeMessageCollectionSaga]';

/* DropTable */

if exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName)
        and type in ('U')
)
begin
    declare @dropTable nvarchar(max);
    set @dropTable = 'drop table ' + @tableName;
    exec(@dropTable);
end
");
		}
	}
}
