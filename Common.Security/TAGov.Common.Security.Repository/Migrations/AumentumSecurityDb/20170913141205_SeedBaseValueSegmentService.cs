using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
    public partial class SeedBaseValueSegmentService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			using (var db = new AumentumSecurityContext())
			{
				var permissionSeeder = new PermissionSeeder(db);

				var baseValueSegmentServiceApplication = db.Permissions.Single(p => p.Name == "api.BaseValueSegmentService" &&
										   p.AppFunctionType == "Application" &&
										   p.App == "api.BaseValueSegmentService");

				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegmentServiceApplication.Id, baseValueSegmentServiceApplication.Name, 
																AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentConclusion));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegmentServiceApplication.Id, baseValueSegmentServiceApplication.Name, 
																AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentHistory));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegmentServiceApplication.Id, baseValueSegmentServiceApplication.Name, 
																AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentTransaction));
				db.Permissions.AddRange(permissionSeeder.CreateFields(baseValueSegmentServiceApplication.Id, baseValueSegmentServiceApplication.Name, 
																AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Owner));
				db.SaveChanges();
			}
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			using (var db = new AumentumSecurityContext())
			{
				var permissions = db.Permissions.Where(x => x.AppFunctionType == "field" &&
															x.ParentName == "api.BaseValueSegmentService" &&
															x.TransactionId == 0 &&
															x.IsMenuItem == 0 &&
															x.ParentId > 0).ToList();
				db.Permissions.RemoveRange(permissions);

				db.SaveChanges();
			}
		}
    }
}
