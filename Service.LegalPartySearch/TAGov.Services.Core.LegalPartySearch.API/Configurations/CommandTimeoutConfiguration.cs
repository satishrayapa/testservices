using Microsoft.Extensions.Configuration;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;

namespace TAGov.Services.Core.LegalPartySearch.API.Configurations
{
	/// <inheritdoc />
	public class CommandTimeoutConfiguration : ICommandTimeoutConfiguration
    {
	    private readonly IConfiguration _configuration;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">configuration containing command timeout</param>
		public CommandTimeoutConfiguration(IConfiguration configuration)
		{
			_configuration = configuration;
		}

	    /// <inheritdoc />
	    public int? CommandTimeout
	    {
		    get
		    {
			    if ( int.TryParse( _configuration[ "Db:TimeoutInSeconds" ], out int commandTimeout ) )
				    return commandTimeout;
			    return null;
		    }
	    }
    }
}
