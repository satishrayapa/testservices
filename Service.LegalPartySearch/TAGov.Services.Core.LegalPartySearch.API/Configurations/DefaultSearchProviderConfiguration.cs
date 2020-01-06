using Microsoft.Extensions.Configuration;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;

namespace TAGov.Services.Core.LegalPartySearch.API.Configurations
{
	/// <summary>
	/// API based DefaultSearchProviderConfiguration.
	/// </summary>
	public class DefaultSearchProviderConfiguration : IDefaultSearchProviderConfiguration
	{
		private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">IConfiguration.</param>
		public DefaultSearchProviderConfiguration(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Default name.
		/// </summary>
		public string DefaultName => _configuration["Provider:Default"];
	}
}
