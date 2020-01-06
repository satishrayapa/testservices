using System.Collections.Generic;

namespace TAGov.Common.Security.Domain.Models.V1
{
	public enum ClientTypes
	{
		Unknown = -1,
		ApplicationService = 0,
		ServiceToService = 1
	}

	public class ClientDto
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public ClientTypes ClientType { get; set; }

		public List<ApiResourceDto> AllowedScopes { get; set; }
	}
}
