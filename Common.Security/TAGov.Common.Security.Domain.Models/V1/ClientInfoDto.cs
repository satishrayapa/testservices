using System.Collections.Generic;

namespace TAGov.Common.Security.Domain.Models.V1
{
	public class ClientInfoDto
	{
		public string Username { get; set; }

		public ClientTypes ClientType { get; set; }

		public List<ClientScopeDto> AllowedScopes { get; set; }
	}
}
