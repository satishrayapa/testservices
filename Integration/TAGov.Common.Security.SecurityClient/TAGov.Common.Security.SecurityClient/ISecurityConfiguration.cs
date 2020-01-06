namespace TAGov.Common.Security.SecurityClient
{
    public interface ISecurityConfiguration
    {
        string Authority { get; }
        string ClientId { get; }
        string ClientPassword { get; }
        string ClientScope { get; }
        bool DisableRequireHttps { get; }
        string LogLocation { get; }
        string ServiceClientId { get; }
        string ServiceClientPassword { get; }
        string ServiceClientScope { get; }

        string AzureAdResourceId { get; }

        string AzureAdClientId { get; }

        string AzureAdClientPassword { get; }
    }
}