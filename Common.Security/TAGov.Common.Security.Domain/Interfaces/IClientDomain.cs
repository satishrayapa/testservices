using System.Collections.Generic;
using System.Threading.Tasks;
using TAGov.Common.Security.Domain.Models.V1;

namespace TAGov.Common.Security.Domain.Interfaces
{
	public interface IClientDomain
	{
		Task<IEnumerable<ClientInfoDto>> List();
		Task Update(ClientDto clientDto);
		Task Create(ClientDto clientDto);
		Task<ClientInfoDto> Get(string username);
	}
}
