using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace TAGov.Common.Security.SecurityClient
{
    public class AzureAdJwtTokenRequestClient : IJwtTokenRequestClient
    {
        private const string AzureAdBaseUrl = "https://login.microsoftonline.com";
        private const string TenantId = "mgmttlrgcom.onmicrosoft.com";
        private readonly ISecurityConfiguration _securityConfiguration;
        private readonly IInternalLogger _internalLogger;
        private readonly IUserProfileId _userProfileId;
        private readonly IJwtTokenCache _jwtTokenCache;

        public AzureAdJwtTokenRequestClient(
            ISecurityConfiguration securityConfiguration,
            IInternalLogger internalLogger,
            IUserProfileId userProfileId,
            IJwtTokenCache jwtTokenCache)
        {
            _securityConfiguration = securityConfiguration;
            _internalLogger = internalLogger;
            _userProfileId = userProfileId;
            _jwtTokenCache = jwtTokenCache;
        }

        public JwtTokenRequestResult ProcessByUserProfileId()
        {
            int profileLoginId;
            if (_userProfileId.IsAuthenticated(out profileLoginId))
            {
                var jwt = _jwtTokenCache.Get(GetKeyByUserProfileId(profileLoginId));
                if (jwt != null) return jwt;
                var clientId = _securityConfiguration.AzureAdClientId;
                var clientPassword = _securityConfiguration.AzureAdClientPassword;
                var resourceId = _securityConfiguration.AzureAdResourceId;

                if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientPassword))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(AzureAdBaseUrl);

                        List<KeyValuePair<string, string>> formData =
                            new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                                new KeyValuePair<string, string>("client_id", clientId),
                                new KeyValuePair<string, string>("client_secret", clientPassword),
                                new KeyValuePair<string, string>("resource", resourceId)
                            };

                        HttpContent content = new FormUrlEncodedContent(formData);
                        var result = client.PostAsync($"/{TenantId}/oauth2/token", content).Result;

                        if (!result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _internalLogger.AppendLog("Token response error: The client Id/Secret is invalid.");
                            return null;
                        }

                        var tokenResponse = result.IsSuccessStatusCode ? result.Content.ReadAsStringAsync().Result : string.Empty;

                        if (string.IsNullOrEmpty(tokenResponse))
                        {
                            _internalLogger.AppendLog("Token response error: " + tokenResponse);
                            return null;
                        }

                        var token = JsonConvert.DeserializeObject<AzureAdTokenResponse>(tokenResponse);

                        var jwtResult = new JwtTokenRequestResult
                        {
                            AccessToken = token.AccessToken,
                            ExpiresIn = token.ExpiresOn,
                            TokenType = token.TokenType
                        };


                        _jwtTokenCache.Add(GetKeyByUserProfileId(profileLoginId), jwtResult);

                        return jwtResult;
                    }
                }

                _internalLogger.AppendLog("TAGov.AzureAd configuration(s) are missing!");
            }

            return null;
        }

        private string GetKeyByUserProfileId(int userProfileId)
        {
            return $"userProfileIdForAzureAd:{userProfileId}";
        }

        public JwtTokenRequestResult ProcessByServiceCredentials()
        {
            throw new NotImplementedException();
        }
    }
}
