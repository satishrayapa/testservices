using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;
using TAGov.Common.Security.Claims;
using TAGov.Common.Security.Domain.Implementation;
using TAGov.Common.Security.Repository.Interfaces;
using TAGov.Common.Security.Repository.Models;
using Xunit;

namespace Domain.Tests
{
	public class ClaimsProviderTests
	{
		[Fact]
		public void WithApplicationAndFieldPermissions_ApplicationPermissionShouldOverrideFieldPermissionsInClaims()
		{
			var permissionRepository = new Mock<IPermissionRepository>();
			permissionRepository.Setup(x => x.GetByUserProfileLoginId(12345))
				.Returns(new List<Permission>
				{
					new Permission { ApplicationName = "app1", Name ="app1", Type = "application", CanCreate = true, CanView = true, CanModify = true, CanDelete = true, AppFunctionId = 1, AppFunctionParentId = 0},
					new Permission { ApplicationName = "app1", Name = "Res1",Type = "field", CanCreate = false, CanView = false, CanModify = false, CanDelete = false, AppFunctionId = 2}
				});
      //Note that the app has fields for which the user does not have any permissions.  To test "filling in the missing fields" logic.
		  var appFunctionForQueryRepository = new Mock<IAppFunctionForQueryRepository>();
		  appFunctionForQueryRepository.Setup( x => x.GetAllFieldAppFunctionsForApplicationLevelAppFunctions( new[] { 1 } ) )
		            .Returns( new List<AppFunctionForQuery>
		                      {
		                        new AppFunctionForQuery { App = "app1", AppFunctionType = "field", Id = 2, Name = "Res1", ParentId = 1, ParentName = "app1" },
		                        new AppFunctionForQuery { App = "app1", AppFunctionType = "field", Id = 3, Name = "Res2", ParentId = 1, ParentName = "app1" }
		                      } );

			var claimsProvider = new ClaimsProvider(permissionRepository.Object, appFunctionForQueryRepository.Object);
			var claims = claimsProvider.GetClaims(12345).ToList();

			var app1Res1Claim = claims.GetClaim("app1", "res1");

			app1Res1Claim.ShouldNotBe(null);
			app1Res1Claim.CanGet().ShouldBe(true);
			app1Res1Claim.CanPost().ShouldBe(true);
			app1Res1Claim.CanPut().ShouldBe(true);
			app1Res1Claim.CanDelete().ShouldBe(true);

		  var app1Res2Claim = claims.GetClaim( "app1", "res2" );
		  app1Res2Claim.ShouldNotBe( null );
		  app1Res2Claim.CanGet().ShouldBe( true );
		  app1Res2Claim.CanPost().ShouldBe( true );
		  app1Res2Claim.CanPut().ShouldBe( true );
		  app1Res2Claim.CanDelete().ShouldBe( true );
		}

		[Fact]
		public void WithOnlyFieldPermissions_ShouldStillGetClaims()
		{
			var permissionRepository = new Mock<IPermissionRepository>();
			permissionRepository.Setup(x => x.GetByUserProfileLoginId(12345))
				.Returns(new List<Permission>
				{
					new Permission { ApplicationName = "app1", Name = "Res1",Type = "field", CanCreate = true, CanView = true, CanModify = true, CanDelete = true}
				});

			var claimsProvider = new ClaimsProvider(permissionRepository.Object, new Mock<IAppFunctionForQueryRepository>().Object);
			var claims = claimsProvider.GetClaims(12345);

			var app1Res1Claim = claims.GetClaim("app1", "res1");

			app1Res1Claim.ShouldNotBe(null);
			app1Res1Claim.CanGet().ShouldBe(true);
			app1Res1Claim.CanPost().ShouldBe(true);
			app1Res1Claim.CanPut().ShouldBe(true);
			app1Res1Claim.CanDelete().ShouldBe(true);
		}

		[Fact]
		public void WithNoPermissions_ShouldNotGetClaims()
		{
			var permissionRepository = new Mock<IPermissionRepository>();
			permissionRepository.Setup(x => x.GetByUserProfileLoginId(12345))
				.Returns(new List<Permission>());

			var claimsProvider = new ClaimsProvider(permissionRepository.Object, new Mock<IAppFunctionForQueryRepository>().Object);
			var claims = claimsProvider.GetClaims(12345).ToList();

			claims.Count.ShouldBe(0);
		}

		[Fact]
		public void WithMultipleApp1AndApp2ApplicationsAndFieldPermissions_GetApp2OnlyClaimsReturnsApp2OnlyClaims()
		{
			var permissionRepository = new Mock<IPermissionRepository>();
			permissionRepository.Setup(x => x.GetByUserProfileLoginId(12345))
				.Returns(new List<Permission>
				{
					new Permission { ApplicationName = "app1", Name ="app1", Type = "application", CanCreate = true, CanView = true, CanModify = true, CanDelete = true},
					new Permission { ApplicationName = "app1", Name = "Res1",Type = "field", CanCreate = false, CanView = false, CanModify = false, CanDelete = false},
					new Permission { ApplicationName = "app2", Name ="app2", Type = "application", CanCreate = true, CanView = true, CanModify = true, CanDelete = true},
					new Permission { ApplicationName = "app2", Name = "Res2",Type = "field", CanCreate = false, CanView = false, CanModify = false, CanDelete = false},
					new Permission { ApplicationName = "app2", Name = "Res3",Type = "field", CanCreate = true, CanView = true, CanModify = true, CanDelete = true}
				});

			var claimsProvider = new ClaimsProvider(permissionRepository.Object, new Mock<IAppFunctionForQueryRepository>().Object);
			var claims = claimsProvider.GetClaims(12345).ToList();

			claims.Count.ShouldBe(3);

			var app2Res2Claim = claims.GetClaim("app2", "res2");

			app2Res2Claim.ShouldNotBe(null);
			app2Res2Claim.CanGet().ShouldBe(true);
			app2Res2Claim.CanPost().ShouldBe(true);
			app2Res2Claim.CanPut().ShouldBe(true);
			app2Res2Claim.CanDelete().ShouldBe(true);

			var app2Res3Claim = claims.GetClaim("app2", "res3");

			app2Res3Claim.ShouldNotBe(null);
			app2Res3Claim.CanGet().ShouldBe(true);
			app2Res3Claim.CanPost().ShouldBe(true);
			app2Res3Claim.CanPut().ShouldBe(true);
			app2Res3Claim.CanDelete().ShouldBe(true);
		}

    /// <summary>
    /// Use case for this is a user assigned to more than one role.
    /// </summary>
	  [Fact]
	  public void WithMultipleApp1ApplicationPermissions_ShouldStillGetClaim()
	  {
	    var permissionRepository = new Mock<IPermissionRepository>();
	    permissionRepository.Setup( x => x.GetByUserProfileLoginId( 12345 ) )
	              .Returns( new List<Permission>
	                        {
	                          new Permission { ApplicationName = "app1", Name = "app1", Type = "application", CanCreate = true, CanView = true, CanModify = true, CanDelete = true },
	                          new Permission { ApplicationName = "app1", Name = "app1", Type = "application", CanCreate = false, CanView = false, CanModify = false, CanDelete = false },
	                          new Permission { ApplicationName = "app1", Name = "app1", Type = "field", CanCreate = true, CanView = true, CanModify = true, CanDelete = true }
	                        } );

	    var claimsProvider = new ClaimsProvider(permissionRepository.Object, new Mock<IAppFunctionForQueryRepository>().Object);
	    var claims = claimsProvider.GetClaims(12345).ToList();

	    claims.Count.ShouldBe(1);

	    var app1App1Claim = claims.GetClaim("app1", "app1");

	    app1App1Claim.ShouldNotBe(null);
	    app1App1Claim.CanGet().ShouldBe(true);
	    app1App1Claim.CanPost().ShouldBe(true);
	    app1App1Claim.CanPut().ShouldBe(true);
	    app1App1Claim.CanDelete().ShouldBe(true);
	  }
  }
}
