using System.Collections.Generic;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Interfaces
{
	public interface IPermissionRepository
	{
		IEnumerable<Permission> GetByUserProfileLoginId(int userProfileLoginId);
	  IEnumerable<Permission> GetAll();
	}
}
