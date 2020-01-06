using System.Linq;
using System.Reflection;

namespace TAGov.Common.ResourceLocatorClient.Enums
{
	public enum Features
	{
		[ResourceKey("BaseValueSegmentFeature")]
		BaseValueSegment,
		[ResourceKey("LegalPartySearchFeature")]
		LegalPartySearch,
		[ResourceKey("RevenueObjectSearchFeature")]
		RevenueObjectSearch
	}
}