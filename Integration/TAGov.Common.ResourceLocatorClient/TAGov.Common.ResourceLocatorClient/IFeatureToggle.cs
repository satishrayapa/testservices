using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Common.ResourceLocatorClient
{
	public interface IFeatureToggle
	{
		bool IsEnabled(Features feature);
	}
}
