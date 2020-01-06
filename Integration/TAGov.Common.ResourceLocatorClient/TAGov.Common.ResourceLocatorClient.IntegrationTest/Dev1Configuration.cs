namespace TAGov.Common.ResourceLocatorClient.IntegrationTest
{
	public class Dev1Configuration : IConfiguration
	{
		public string Get(string key)
		{
			if (key == Constants.CommonResourceLocatorPartition)
			{
				return "DEV";
			}

			if (key == Constants.CommonResourceLocatorUri)
			{
				return "http://c592xfglumwb1.ecomqc.tlrg.com/common.resourcelocator";
			}

			return null;
		}
	}
}