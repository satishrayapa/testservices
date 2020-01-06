using System;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Common.ResourceLocatorClient
{
	public class FeatureToggle : IFeatureToggle
	{
		private readonly IRestClient _client;

		public FeatureToggle(IRestClient client)
		{
			_client = client;
		}

		public bool IsEnabled(Features feature)
		{
			var enabledValue = _client.GetResource(feature.GetResourceKey());

			return !string.IsNullOrEmpty(enabledValue?.Value) && Convert.ToBoolean(enabledValue.Value);
		}
	}
}
