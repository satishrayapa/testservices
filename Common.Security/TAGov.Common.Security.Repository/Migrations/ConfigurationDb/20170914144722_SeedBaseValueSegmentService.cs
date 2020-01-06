using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;

namespace TAGov.Common.Security.Repository.Migrations.ConfigurationDb
{
    public partial class SeedBaseValueSegmentService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			using (var db = new ProxyConfigurationDbContext())
			{
		
				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentConclusion,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentHistory,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentTransaction,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.InsertSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Owner,
					ServiceTypes.Service));
			}
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			using (var db = new ProxyConfigurationDbContext())
			{
				db.Database.ExecuteSqlCommand(IdentityHelper.DeleteSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentConclusion,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.DeleteSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentHistory,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.DeleteSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.BaseValueSegmentTransaction,
					ServiceTypes.Service));

				db.Database.ExecuteSqlCommand(IdentityHelper.DeleteSql(
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Name,
					AumentumSecurityObjectModel.BaseValueSegementSecurityObjectModel.Resources.Owner,
					ServiceTypes.Service));

				db.SaveChanges();
			}
		}
    }
}
