using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using TAGov.Common.ResourceLocator.Repository.Models.V1;

namespace TAGov.Common.ResourceLocator.Repository.Migrations
{
	public partial class SeedResources : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			using (var db = new ResourceContext())
			{
				// Only perform the following operation if this is an empty db. This is to
				// allow for powershell scripts outside to seed the system.

				if (db.Resources.Count() == 0)
				{
					db.Resources.Add(new Resource { Key = "BaseValueSegmentFeature", Partition = "dev", Value = "true" });
					db.Resources.Add(new Resource { Key = "service.legalpartysearch", Partition = "urlservices:dev", Value = "http://" });
					db.SaveChanges();
				}
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Do nothing.
		}
	}
}
