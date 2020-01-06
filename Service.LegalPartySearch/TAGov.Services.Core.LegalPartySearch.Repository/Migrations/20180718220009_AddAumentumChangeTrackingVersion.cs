using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class AddAumentumChangeTrackingVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(ChangeTracking.EnableChangeTracking,true);		
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(ChangeTracking.DisableChangeTracking,true);
		}
    }
}
