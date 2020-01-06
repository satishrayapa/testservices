using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.Repository
{
	public class AppCommandTimeoutConfiguration : ICommandTimeoutConfiguration
	{
		public int? CommandTimeout { get; set; }
	}
}