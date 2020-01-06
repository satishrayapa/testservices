using System;
using TAGov.Common.ResourceLocatorClient.Enums;

namespace TAGov.Common.ResourceLocatorClient
{
	internal class ResourceKeyAttribute : Attribute
	{
		private readonly string _keyValue;
		public ResourceKeyAttribute(string keyValue) { _keyValue = keyValue; }
		public override string ToString()
		{
			return _keyValue;
		}
	}

	public static class ResourceKeyAttributeExtension
	{
		public static string GetResourceKey(this Features feature)
		{
			var type = feature.GetType();

			var memInfo = type.GetMember(feature.ToString());

			if (memInfo.Length <= 0) return feature.ToString();

			var attrs = memInfo[0].GetCustomAttributes(typeof(ResourceKeyAttribute), false);

			return attrs.Length > 0 ? ((ResourceKeyAttribute)attrs[0]).ToString() : feature.ToString();
		}
	}
}
