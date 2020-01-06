using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using TAGov.Common.Security.Repository.Interfaces;

namespace TAGov.Common.Security.Repository.Implementation
{
	public class ClientRepository : IClientRepository
	{
		private readonly ProxyConfigurationDbContext _proxyConfigurationDbContext;

		public ClientRepository(ProxyConfigurationDbContext proxyConfigurationDbContext)
		{
			_proxyConfigurationDbContext = proxyConfigurationDbContext;
		}
		public async Task<IEnumerable<Client>> List()
		{
			return await _proxyConfigurationDbContext.Clients
				.Include(x => x.AllowedScopes)
				.Include(x => x.AllowedGrantTypes)
				.ToListAsync();
		}

		public async Task<Client> Get(string username)
		{
			return await _proxyConfigurationDbContext.Clients
				.Include(x => x.AllowedScopes)
				.Include(x => x.AllowedGrantTypes)
				.Include(x => x.ClientSecrets)
				.SingleOrDefaultAsync(x => x.ClientId == username);
		}

		public async Task Add(Client client)
		{
			await _proxyConfigurationDbContext.Clients.AddAsync(client);
			await _proxyConfigurationDbContext.SaveChangesAsync();
		}

		public async Task Update(Client client)
		{
			await _proxyConfigurationDbContext.SaveChangesAsync();
		}
	}
}
