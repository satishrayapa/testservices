using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TAGov.Common.Security.Claims;
using TAGov.Common.Security.Domain.Interfaces;
using TAGov.Common.Security.Repository.Interfaces;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Domain.Implementation
{
	public class ClaimsProvider : IClaimsProvider
	{
		private readonly IPermissionRepository _permissionRepository;
	  private readonly IAppFunctionForQueryRepository _appFunctionForQueryRepository;
		public ClaimsProvider(IPermissionRepository permissionRepository, IAppFunctionForQueryRepository appFunctionForQueryRepository)
		{
			_permissionRepository = permissionRepository;
		  _appFunctionForQueryRepository = appFunctionForQueryRepository;
		}

		public IEnumerable<Claim> GetClaims(int userProfileLoginId)
		{
		  List<Permission> explicitUserPermissions = _permissionRepository.GetByUserProfileLoginId( userProfileLoginId ).ToList();

      //No guarantee that all fields will be returned for all applications above.  If the user has explicit permissions for an application then
      //we need to make sure we get all the fields for that application and then fill in any missing permissions (assume false) for those fields.

		  IEnumerable<int> applicationLevelAppFunctionIds =
		    explicitUserPermissions
		      .Where(afp => afp.Type.ToLower() == "application")
		      .Select(afp => afp.AppFunctionId)
		      .Distinct();

		  IEnumerable<AppFunctionForQuery> allFieldLevelAppFunctionsForApplicationLevelDefaultPermissions =
		    _appFunctionForQueryRepository.GetAllFieldAppFunctionsForApplicationLevelAppFunctions( applicationLevelAppFunctionIds );

      //Create the permissions inserting any missing field-level permissions
		  List<Permission> permissions =
		    explicitUserPermissions.Select( eachPermission =>
		                                      new Permission
		                                      {
		                                        ApplicationName = eachPermission.ApplicationName,
		                                        CanCreate = eachPermission.CanCreate,
		                                        CanDelete = eachPermission.CanDelete,
		                                        CanModify = eachPermission.CanModify,
		                                        CanView = eachPermission.CanView,
		                                        Name = eachPermission.Name,
		                                        Type = eachPermission.Type,
		                                        AppFunctionId = eachPermission.AppFunctionId,
		                                        AppFunctionParentId = eachPermission.AppFunctionParentId
		                                      } ).ToList();

      //now add the missing permissions
		  permissions.AddRange(
		    allFieldLevelAppFunctionsForApplicationLevelDefaultPermissions
		      .Where( fieldLevelPermission => !explicitUserPermissions.Exists( ep => ep.AppFunctionId == fieldLevelPermission.Id ) )
		      .Select( missingFieldLevelAppFunction =>
		                 new Permission
		                 {
		                   ApplicationName = missingFieldLevelAppFunction.App.Trim(),
		                   CanCreate = false,
		                   CanDelete = false,
		                   CanModify = false,
		                   CanView = false,
		                   Name = missingFieldLevelAppFunction.Name.Trim(),
		                   Type = missingFieldLevelAppFunction.AppFunctionType.Trim(),
		                   AppFunctionId = missingFieldLevelAppFunction.Id,
		                   AppFunctionParentId = missingFieldLevelAppFunction.ParentId
		                 } ) );

		  return GetFullClaimsWithPermissions( permissions );
		}

		public IEnumerable<Claim> GetFullApplicationClaims()
		{
			return GetFullClaimsWithPermissions(_permissionRepository.GetAll().ToList());
		}

		/// <summary>
		/// These are sensitive claims related to being able to manage security for the entire system. Hence these are dealt
		/// seperately from the application related claims.
		/// </summary>
		/// <returns>IEnumerable of Claims.</returns>
		/// <remarks>Notice we are also not storing the permissions in Aumentum. We do not want someone to accidently assign these permissions from the Aumentum side.</remarks>
		public IEnumerable<Claim> GetFullSecurityClaims()
		{
			return new List<Claim>
			{
				ClaimsHelper.ToClaim("api.securityservice", "ClientInfo", true, true, true, true),
				ClaimsHelper.ToClaim("api.securityservice", "Client", true, true, true, true)
			};
		}

		public IEnumerable<Claim> GetFullClaimsWithPermissions(List<Permission> permissions)
		{
			var applicationNames = permissions.Select(x => x.ApplicationName).Distinct().ToList();
			var claims = new List<Claim>();

			applicationNames.ForEach(applicationName =>
				claims.AddRange(GetDirectAndInheritedClaims(applicationName, permissions)));

			return claims;
		}

		private List<Claim> GetDirectAndInheritedClaims(string applicationName, List<Permission> permissions)
		{
			var applicationSpecificPermissions = permissions.Where(x => x.ApplicationName == applicationName).ToList();

			// Determine if there are any application level permissions.
			var applicationLevelPermissions = applicationSpecificPermissions.Where(x => x.Type.ToLowerInvariant() == "application").ToList();

			var fieldLevelPermissions = applicationSpecificPermissions.Where(x => x.Type.ToLowerInvariant() == "field").ToList();

			return ConvertPermissionsAsClaimsAndAdd(fieldLevelPermissions, applicationLevelPermissions);
		}

		private List<Claim> ConvertPermissionsAsClaimsAndAdd(List<Permission> permissions, List<Permission> parentPermissions)
		{
      //Create single aggregate parent (== application-level) permission--most permissive wins
		  var parentPermission = new Permission
		                         {
		                           CanCreate = parentPermissions.Exists( permission => permission.CanCreate ),
		                           CanDelete = parentPermissions.Exists( permission => permission.CanDelete ),
		                           CanModify = parentPermissions.Exists( permission => permission.CanModify ),
		                           CanView = parentPermissions.Exists( permission => permission.CanView )
		                         };
			return permissions.Select(x => ToClaim(x, parentPermission)).ToList();
		}

		private Claim ToClaim(Permission permission, Permission parentPermission = null)
		{
			return ClaimsHelper.ToClaim(permission.ApplicationName, permission.Name,
				parentPermission != null ? parentPermission.CanView || permission.CanView : permission.CanView,
				parentPermission != null ? parentPermission.CanCreate || permission.CanCreate : permission.CanCreate,
				parentPermission != null ? parentPermission.CanModify || permission.CanModify : permission.CanModify,
				parentPermission != null ? parentPermission.CanDelete || permission.CanDelete : permission.CanDelete);
		}
	}
}
