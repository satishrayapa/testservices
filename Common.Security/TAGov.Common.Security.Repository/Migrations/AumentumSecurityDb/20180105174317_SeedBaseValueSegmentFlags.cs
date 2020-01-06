using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TAGov.Common.Security.Repository.Migrations.AumentumSecurityDb
{
	public partial class SeedBaseValueSegmentFlags : Migration
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
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Flags));
				db.SaveChanges();
			}
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			using (var db = new AumentumSecurityContext())
			{
				var baseValueSegmentServiceApplication = db.Permissions.Single(p => p.Name == "api.BaseValueSegmentService" &&
																					p.AppFunctionType == "Application" &&
																					p.App == "api.BaseValueSegmentService");

				var name = PermissionSeeder.ToResourceName(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Flags);

				var permission = db.Permissions.Single(
					x => x.ParentId == baseValueSegmentServiceApplication.Id &&
					x.Name == name &&
					x.AppFunctionType == "field");

				db.Permissions.Remove(permission);

				db.SaveChanges();
			}
		}
	}
}
