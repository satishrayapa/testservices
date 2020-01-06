namespace TAGov.Common.Security.Repository.Migrations
{
	public static class AumentumSecurityObjectModel
	{
		public static class LegalPartySearchSecurityObjectModel
		{
			public const string Name = "Legal Party Search";
			public static class Resources
			{
				public const string LegalPartySearch = "Legal Party Search";
			}
		}

		public static class MyWorkListSearchSecurityObjectModel
		{
			public const string Name = "My WorkList Search";
			public static class Resources
			{
				public const string MyWorkListSearch = "My WorkList Search";
			}
		}
		public static class LegalPartySecurityObjectModel
		{
			public const string Name = "Legal Party";
			public static class Resources
			{
				public const string LegalParty = "Legal Party";
				public const string LegalPartyRole = "Legal Party Role";
				public const string LegalPartyDocument = "Legal Party Document";
			}
		}

		public static class RevenueObjectSecurityObjectModel
		{
			public const string Name = "Revenue Object";

			public static class Resources
			{
				public const string RevenueObject = "Revenue Object";
				public const string TaxAuthorityGroup = "Tax Authority Group";
			}
		}

		public static class AssessmentEventSecurityObjectModel
		{
			public const string Name = "Assessment Event";

			public static class Resources
			{
				public const string AssessmentEvent = "Assessment Event";
				public const string AssessmentEventAttributeValue = "Assessment Event Attribute Value";
				public const string AssessmentEventRevision = "Assessment Event Revision";
				public const string StatutoryReference = "Statutory Reference";
			}
		}

		// ReSharper disable once InconsistentNaming
		public static class GRMEventSecurityObjectModel
		{
			public const string Name = "GRM Event";

			public static class Resources
			{
				// ReSharper disable once InconsistentNaming
				public const string GRMEvent = "GRM Event";

				// ReSharper disable once InconsistentNaming
				public const string GRMEventInformation = "GRM Event Information";

			  public const string SubComponentValues = "Sub Component Values";
			}
    }

		public static class BaseValueSegementSecurityObjectModel
		{
			public const string Name = "Base Value Segment";

			public static class Resources
			{
				public const string BaseValueSegment = "Base Value Segment";
				public const string BaseValueSegmentEvent = "Base Value Segment Event";
				public const string CaliforniaConsumerPriceIndex = "California Consumer Price Index";
				public const string SubComponent = "Sub Component";

				public const string BaseValueSegmentConclusion = "Base Value Segment Conclusion";
				public const string BaseValueSegmentHistory = "Base Value Segment History";
				public const string BaseValueSegmentTransaction = "Base Value Segment Transaction";
				public const string Owner = "Owner";
				public const string Flags = "Flags";
			}
		}

		public static class ResourceLocatorSecurityObjectModel
		{
			public const string Name = "Resource Locator";

			public static class Resources
			{
				public const string Resource = "Resource";
			}
		}
	}
}
