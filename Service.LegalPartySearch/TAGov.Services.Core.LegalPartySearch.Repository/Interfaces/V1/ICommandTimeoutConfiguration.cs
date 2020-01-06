namespace TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1
{
	/// <summary>
	/// Interface to implement to provide the command timeout configured in the
	/// appropriate aspsettings.json.
	/// </summary>
    public interface ICommandTimeoutConfiguration
    {
		/// <summary>
		/// command timeout in seconds--null if not configured
		/// </summary>
		int? CommandTimeout { get; }
    }
}
