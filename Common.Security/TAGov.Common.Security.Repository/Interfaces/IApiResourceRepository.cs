using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace TAGov.Common.Security.Repository.Interfaces
{
	public interface IApiResourceRepository
	{
		Task Add(ApiResource apiResource);

		Task<ApiResource> Get(string name);
	}
}
