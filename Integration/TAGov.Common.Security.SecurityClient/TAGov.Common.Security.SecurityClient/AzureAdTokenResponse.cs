using Newtonsoft.Json;

namespace TAGov.Common.Security.SecurityClient
{
    public class AzureAdTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_on")]
        public long ExpiresOn { get; set; }
    }
}