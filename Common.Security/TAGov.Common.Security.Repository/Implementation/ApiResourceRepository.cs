using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.Security.Repository.Interfaces;

namespace TAGov.Common.Security.Repository.Implementation
{
	public class ApiResourceRepository : IApiResourceRepository
	{
		private readonly ProxyConfigurationDbContext _proxyConfigurationDbContext;

		public ApiResourceRepository(ProxyConfigurationDbContext proxyConfigurationDbContext)
		{
			_proxyConfigurationDbContext = proxyConfigurationDbContext;
		}

		public async Task Add(ApiResource apiResource)
		{
			await _proxyConfigurationDbContext.ApiResources.AddAsync(apiResource);
			await _proxyConfigurationDbContext.SaveChangesAsync();
		}

		public async Task<ApiResource> Get(string name)
		{
			return await _proxyConfigurationDbContext.ApiResources.SingleOrDefaultAsync(x => x.Name == name);
		}
	}
}
