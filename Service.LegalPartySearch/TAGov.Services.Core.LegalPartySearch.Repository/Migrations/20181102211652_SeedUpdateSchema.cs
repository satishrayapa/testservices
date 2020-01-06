using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Migrations
{
    public partial class SeedUpdateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			//migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON [search].[LegalPartySearch];", true);

			//migrationBuilder.Sql(UpdatedSeedQuery.BuildSeedQuery());

			//migrationBuilder.Sql(@"
			//CREATE FULLTEXT INDEX ON[search].[LegalPartySearch]
			//(
			//    DisplayName LANGUAGE 0,
			//    Addr LANGUAGE 0,
			//    PIN LANGUAGE 0,
			//    AIN LANGUAGE 0,
			//    GeoCode LANGUAGE 0,
			//    SearchAll LANGUAGE 0,   
			//    Tag LANGUAGE 0,
			//	UnformattedPIN LANGUAGE 0)
			//KEY INDEX PK_LegalPartySearch
			//WITH STOPLIST = OFF;		
			//", true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {
			//migrationBuilder.Sql(@"TRUNCATE TABLE [search].[LegalPartySearch]");
		}
    }
}
