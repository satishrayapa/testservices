using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Common.Security.Repository.Implementation;
using Xunit;

namespace TAGov.Common.Security.Repository.Tests
{
	public class PermissionRepositoryTests
	{
		private readonly AumentumSecurityQueryContext _aumentumSecurityQueryContext;
		public PermissionRepositoryTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<AumentumSecurityQueryContext>();
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

			_aumentumSecurityQueryContext = new AumentumSecurityQueryContext(optionsBuilder.Options);

			TestDataBuilder.Build(_aumentumSecurityQueryContext);
		}

		[Fact]
		public void User1OnlyHas1ApplicationLevelPermissionsForApp1()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetByUserProfileLoginId(TestDataBuilder.User1Id).ToList();

			permissions.Count.ShouldBe(1);
			permissions[0].Name.ShouldBe("api.app1");
			permissions[0].ApplicationName.ShouldBe("api.app1");
			permissions[0].Type.ShouldBe("application");
			permissions[0].CanCreate.ShouldBe(true);
			permissions[0].CanView.ShouldBe(true);
			permissions[0].CanModify.ShouldBe(true);
			permissions[0].CanDelete.ShouldBe(true);
		  permissions[ 0 ].AppFunctionId.ShouldBe( TestDataBuilder.App1Id );
		  permissions[ 0 ].AppFunctionParentId.ShouldBe( 0 );
		}

		[Fact]
		public void User2OnlyHasBothApplicationAndFieldLevelPermissionsForApp1()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetByUserProfileLoginId(TestDataBuilder.User2Id).ToList();

			permissions.Count.ShouldBe(1);
			permissions[0].Name.ShouldBe("res1");
			permissions[0].ApplicationName.ShouldBe("api.app1");
			permissions[0].Type.ShouldBe("field");
			permissions[0].CanCreate.ShouldBe(true);
			permissions[0].CanView.ShouldBe(true);
			permissions[0].CanModify.ShouldBe(true);
			permissions[0].CanDelete.ShouldBe(true);
		  permissions[ 0 ].AppFunctionId.ShouldBe( TestDataBuilder.App2Id );
		  permissions[ 0 ].AppFunctionParentId.ShouldBe( TestDataBuilder.App1Id );
		}

		[Fact]
		public void User3OnlyHasBothApplicationAndFieldLevelPermissionsForApp1()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetByUserProfileLoginId(TestDataBuilder.User3Id).ToList();

			permissions.Count.ShouldBe(2);

			var applicationLevelPermission = permissions.Single(x => x.Type == "application");
			applicationLevelPermission.Name.ShouldBe("api.app1");
			applicationLevelPermission.ApplicationName.ShouldBe("api.app1");
			applicationLevelPermission.Type.ShouldBe("application");
			applicationLevelPermission.CanCreate.ShouldBe(true);
			applicationLevelPermission.CanView.ShouldBe(true);
			applicationLevelPermission.CanModify.ShouldBe(true);
			applicationLevelPermission.CanDelete.ShouldBe(true);
		  applicationLevelPermission.AppFunctionId.ShouldBe( TestDataBuilder.App1Id );
		  applicationLevelPermission.AppFunctionParentId.ShouldBe( 0 );

			var fieldLevelPermission = permissions.Single(x => x.Type == "field");
			fieldLevelPermission.Name.ShouldBe("res1");
			fieldLevelPermission.ApplicationName.ShouldBe("api.app1");
			fieldLevelPermission.Type.ShouldBe("field");
			fieldLevelPermission.CanCreate.ShouldBe(true);
			fieldLevelPermission.CanView.ShouldBe(true);
			fieldLevelPermission.CanModify.ShouldBe(true);
			fieldLevelPermission.CanDelete.ShouldBe(true);
		  fieldLevelPermission.AppFunctionId.ShouldBe( TestDataBuilder.App2Id );
		  fieldLevelPermission.AppFunctionParentId.ShouldBe( TestDataBuilder.App1Id );
		}

		[Fact]
		public void User4OnlyHasBothApplicationAndFieldLevelPermissionsForApp1()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetByUserProfileLoginId(TestDataBuilder.User4Id).ToList();

			permissions.Count.ShouldBe(2);

			var applicationLevelPermission = permissions.Single(x => x.Type == "application");
			applicationLevelPermission.Name.ShouldBe("api.app1");
			applicationLevelPermission.ApplicationName.ShouldBe("api.app1");
			applicationLevelPermission.Type.ShouldBe("application");
			applicationLevelPermission.CanCreate.ShouldBe(true);
			applicationLevelPermission.CanView.ShouldBe(true);
			applicationLevelPermission.CanModify.ShouldBe(true);
			applicationLevelPermission.CanDelete.ShouldBe(true);
		  applicationLevelPermission.AppFunctionId.ShouldBe( TestDataBuilder.App1Id );
		  applicationLevelPermission.AppFunctionParentId.ShouldBe( 0 );

			var fieldLevelPermission = permissions.Single(x => x.Type == "field");
			fieldLevelPermission.Name.ShouldBe("res1");
			fieldLevelPermission.ApplicationName.ShouldBe("api.app1");
			fieldLevelPermission.Type.ShouldBe("field");
			fieldLevelPermission.CanCreate.ShouldBe(true);
			fieldLevelPermission.CanView.ShouldBe(true);
			fieldLevelPermission.CanModify.ShouldBe(true);
			fieldLevelPermission.CanDelete.ShouldBe(true);
		  fieldLevelPermission.AppFunctionId.ShouldBe( TestDataBuilder.App2Id );
		  fieldLevelPermission.AppFunctionParentId.ShouldBe( TestDataBuilder.App1Id );
		}

		[Fact]
		public void User5OnlyHasBothApplicationAndFieldLevelPermissionsForApp1AndApp2()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetByUserProfileLoginId(TestDataBuilder.User5Id).ToList();

			permissions.Count.ShouldBe(4);

			var applicationLevelPermissionApp1 = permissions.Single(x => x.Type == "application" && x.ApplicationName == "api.app1");
			applicationLevelPermissionApp1.Name.ShouldBe("api.app1");
			applicationLevelPermissionApp1.ApplicationName.ShouldBe("api.app1");
			applicationLevelPermissionApp1.Type.ShouldBe("application");
			applicationLevelPermissionApp1.CanCreate.ShouldBe(true);
			applicationLevelPermissionApp1.CanView.ShouldBe(true);
			applicationLevelPermissionApp1.CanModify.ShouldBe(true);
			applicationLevelPermissionApp1.CanDelete.ShouldBe(true);
		  applicationLevelPermissionApp1.AppFunctionId.ShouldBe( TestDataBuilder.App1Id );
		  applicationLevelPermissionApp1.AppFunctionParentId.ShouldBe( 0 );

			var fieldLevelPermissionApp1 = permissions.Single(x => x.Type == "field" && x.ApplicationName == "api.app1");
			fieldLevelPermissionApp1.Name.ShouldBe("res1");
			fieldLevelPermissionApp1.ApplicationName.ShouldBe("api.app1");
			fieldLevelPermissionApp1.Type.ShouldBe("field");
			fieldLevelPermissionApp1.CanCreate.ShouldBe(true);
			fieldLevelPermissionApp1.CanView.ShouldBe(true);
			fieldLevelPermissionApp1.CanModify.ShouldBe(true);
			fieldLevelPermissionApp1.CanDelete.ShouldBe(true);
		  fieldLevelPermissionApp1.AppFunctionId.ShouldBe( TestDataBuilder.App2Id );
		  fieldLevelPermissionApp1.AppFunctionParentId.ShouldBe( TestDataBuilder.App1Id );

			var applicationLevelPermissionApp2 = permissions.Single(x => x.Type == "application" && x.ApplicationName == "api.app2");
			applicationLevelPermissionApp2.Name.ShouldBe("api.app2");
			applicationLevelPermissionApp2.ApplicationName.ShouldBe("api.app2");
			applicationLevelPermissionApp2.Type.ShouldBe("application");
			applicationLevelPermissionApp2.CanCreate.ShouldBe(true);
			applicationLevelPermissionApp2.CanView.ShouldBe(true);
			applicationLevelPermissionApp2.CanModify.ShouldBe(true);
			applicationLevelPermissionApp2.CanDelete.ShouldBe(true);
		  applicationLevelPermissionApp2.AppFunctionId.ShouldBe( TestDataBuilder.App4Id );
		  applicationLevelPermissionApp2.AppFunctionParentId.ShouldBe( 0 );

			var fieldLevelPermissionApp2 = permissions.Single(x => x.Type == "field" && x.ApplicationName == "api.app2");
			fieldLevelPermissionApp2.Name.ShouldBe("res3");
			fieldLevelPermissionApp2.ApplicationName.ShouldBe("api.app2");
			fieldLevelPermissionApp2.Type.ShouldBe("field");
			fieldLevelPermissionApp2.CanCreate.ShouldBe(true);
			fieldLevelPermissionApp2.CanView.ShouldBe(true);
			fieldLevelPermissionApp2.CanModify.ShouldBe(true);
			fieldLevelPermissionApp2.CanDelete.ShouldBe(true);
		  fieldLevelPermissionApp2.AppFunctionId.ShouldBe( TestDataBuilder.App5Id );
		  fieldLevelPermissionApp2.AppFunctionParentId.ShouldBe( TestDataBuilder.App4Id );
		}

		[Fact]
		public void GetAllPermissions()
		{
			var permissionRepository = new PermissionRepository(_aumentumSecurityQueryContext);
			var permissions = permissionRepository.GetAll().ToList();

			permissions.Count.ShouldBe(5);
		}
	}
}
