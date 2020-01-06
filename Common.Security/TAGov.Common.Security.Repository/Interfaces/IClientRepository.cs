using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace TAGov.Common.Security.Repository.Interfaces
{
	public interface IClientRepository
	{
		Task<IEnumerable<Client>> List();

		Task<Client> Get(string username);

		Task Add(Client client);

		Task Update(Client client);
	}
}
