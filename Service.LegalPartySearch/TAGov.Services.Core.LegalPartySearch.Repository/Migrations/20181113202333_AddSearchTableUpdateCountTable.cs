using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddSearchTableUpdateCountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"CREATE TABLE search.SearchTableUpdateRate
(
	id tinyint not null,
	updateRate int not null,
	lastUpdated datetime not null,
	constraint id_p unique(id),
	constraint id_c check(id=1)
);

insert into search.SearchTableUpdateRate (id, updateRate, lastUpdated) values (1, 0, getdate());

GO

CREATE PROCEDURE search.UpdatePerfCounters
AS
BEGIN
 
    declare @nbRecordPending int;
 
    declare @maxLegalParty int, @maxLegalPartyRole int, @maxComm int, @maxCommRole int, @maxSitusAddr int, @maxSitusAddrRole int
        ,@maxApplSite int, @maxApplSiteRole int, @maxRevObj int, @maxTAG int;
 
    select @maxLegalParty = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'LegalParty'
    select @maxLegalPartyRole = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'LegalPartyRole'
    select @maxComm = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'Comm'
    select @maxCommRole = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'CommRole'
    select @maxSitusAddr = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'SitusAddr'
    select @maxSitusAddrRole = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'SitusAddrRole'
    select @maxApplSite = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'ApplSite'
    select @maxApplSiteRole = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'ApplSiteRole'
    select @maxRevObj = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'RevObj'
    select @maxTAG = changeVersion from search.AumentumChangeTrackingVersion where tableName = 'TAG'
 
    select @nbRecordPending = SUM(nb)
    from
    (
        SELECT 'LegalParty' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES LegalParty, @maxLegalParty) AS CT
        union
        SELECT 'LegalPartyRole' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES LegalPartyRole, @maxLegalPartyRole) AS CT
        union
        SELECT 'Comm' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES Comm, @maxComm) AS CT
        union
        SELECT 'CommRole' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES CommRole, @maxCommRole) AS CT
        union
        SELECT 'SitusAddr' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES SitusAddr, @maxSitusAddr) AS CT
        union
        SELECT 'SitusAddrRole' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES SitusAddrRole, @maxSitusAddrRole) AS CT
        union
        SELECT 'ApplSite' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES ApplSite, @maxApplSite) AS CT
        union
        SELECT 'ApplSiteRole' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES ApplSiteRole, @maxApplSiteRole) AS CT
        union
        SELECT 'RevObj' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES RevObj, @maxRevObj) AS CT
        union
        SELECT 'TAG' as tbl, count(*) as nb FROM CHANGETABLE(CHANGES TAG, @maxTAG) AS CT
    ) as t
    
 
    declare @nbRecordUpdated int;
    declare @lastUpdated datetime;
 
    select @nbRecordUpdated = ( updateRate * 60 ) , @lastUpdated = lastUpdated
    from search.SearchTableUpdateRate
 
    if ( (datediff(mi, @lastUpdated, getdate()) > 5) AND (@nbRecordUpdated <> 0) )
    BEGIN
        set @nbRecordUpdated = 0;
        update search.SearchTableUpdateRate set updateRate = @nbRecordUpdated, lastUpdated = GETDATE();
    END
 
    dbcc setinstance ('SQLServer:User Settable', 'Query', 'ESS-LPST_NumberOfPendingChanges', @nbRecordPending)
    dbcc setinstance ('SQLServer:User Settable', 'Query', 'ESS-LPST_UpdateRateOfChanges', @nbRecordUpdated)
 
END", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"drop procedure search.UpdatePerfCounters;
drop table search.SearchTableUpdateRate;", true);
        }
    }
}
