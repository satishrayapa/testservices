using System.Collections.Generic;
using System.Linq;
using TAGov.Common.Security.Repository.Interfaces;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Implementation
{
	public class PermissionRepository : IPermissionRepository
	{
		private readonly AumentumSecurityQueryContext _aumentumSecurityQueryContext;

		public PermissionRepository(AumentumSecurityQueryContext aumentumSecurityQueryContext)
		{
			_aumentumSecurityQueryContext = aumentumSecurityQueryContext;
		}

		public IEnumerable<Permission> GetByUserProfileLoginId(int userProfileLoginId)
		{
			return (from rf in _aumentumSecurityQueryContext.RolesPermissions
					join ur in _aumentumSecurityQueryContext.UsersRoles on rf.RoleId equals ur.RoleId
					join af in _aumentumSecurityQueryContext.Permissions on rf.AppFunctionId equals af.Id
					join upl in _aumentumSecurityQueryContext.UserLogins on ur.UserProfileId equals upl.UserProfileId
					where upl.Id == userProfileLoginId
					  && ((af.ParentName != null && af.ParentName.StartsWith("api.") && af.AppFunctionType.Trim() == "field") ||
						  (af.Name.StartsWith("api.") && af.AppFunctionType.Trim() == "application"))
					select new Permission
					{
						ApplicationName = af.App.Trim(),
						Name = af.Name.Trim(),
						Type = af.AppFunctionType.Trim(),
						CanCreate = rf.CanCreate == 1,
						CanModify = rf.CanModify == 1,
						CanDelete = rf.CanDelete == 1,
						CanView = rf.CanView == 1,
            AppFunctionId = af.Id,
            AppFunctionParentId = af.ParentId
					}).ToList();
		}

	  public IEnumerable<Permission> GetAll()
		{
			return (from af in _aumentumSecurityQueryContext.Permissions
					where (af.ParentName != null && af.ParentName.StartsWith("api.") && af.AppFunctionType.Trim() == "field") ||
						  (af.Name.StartsWith("api.") && af.AppFunctionType.Trim() == "application")
					select new Permission
					{
						ApplicationName = af.App.Trim(),
						Name = af.Name.Trim(),
						Type = af.AppFunctionType.Trim(),
						CanCreate = true,
						CanModify = true,
						CanDelete = true,
						CanView = true
					}).ToList();
		}
	}
}
