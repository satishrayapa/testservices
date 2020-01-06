using System.Collections.Generic;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Domain.Implementation;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
using Xunit;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Test
{
	public class SearchProviderSelectorTests
	{
		private readonly ISearchProviderSelector _searchProviderSelector;

		private Mock<IDefaultSearchProviderConfiguration> _defaultSearchProviderConfigurationMock;

		public SearchProviderSelectorTests()
		{
			_defaultSearchProviderConfigurationMock = new Mock<IDefaultSearchProviderConfiguration>();

			var provider1 = new Mock<ISearchLegalPartyRepository>();
			provider1.Setup(x => x.ProviderName).Returns("provider1");
			var provider2 = new Mock<ISearchLegalPartyRepository>();
			provider2.Setup(x => x.ProviderName).Returns("provider2");
			var provider3 = new Mock<ISearchLegalPartyRepository>();
			provider3.Setup(x => x.ProviderName).Returns("provider3");

			_searchProviderSelector = new SearchProviderSelector(new List<ISearchLegalPartyRepository>
			{
				provider1.Object,
				provider2.Object,
				provider3.Object
			}, _defaultSearchProviderConfigurationMock.Object);
		}

		[Fact]
		public void SelectFromDefaultConfigurationIfNoProviderFlagIsDetected()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("provider2");

			var provider = _searchProviderSelector.Get("foo");

			provider.Provider.ProviderName.ShouldBe("provider2");
			provider.ParsedSearchText.ShouldBe("foo");
		}

		[Fact]
		public void ValidProviderFlagIsSetProvider3_ShouldGetProvider3()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("provider2");

			var provider = _searchProviderSelector.Get("--provider provider3 foobar");

			provider.Provider.ProviderName.ShouldBe("provider3");
			provider.ParsedSearchText.ShouldBe("foobar");
		}

		[Fact]
		public void ValidProviderFlagIsSetProvider3WithSearchTermMoreThanOneWord_ShouldGetProvider3()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("provider2");

			var provider = _searchProviderSelector.Get("--provider provider3 foo bar s a c k");

			provider.Provider.ProviderName.ShouldBe("provider3");
			provider.ParsedSearchText.ShouldBe("foo bar s a c k");
		}

		[Fact]
		public void InvalidProviderFlagIsSet_ShouldGetDefaultProvider()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("provider2");

			var provider = _searchProviderSelector.Get("--provider blah foobar");

			provider.Provider.ProviderName.ShouldBe("provider2");
			provider.ParsedSearchText.ShouldBe("foobar");
		}

		[Fact]
		public void DefaultConfigurationIsEmptyString_ShouldGetException()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("");

			Should.Throw<InternalServerErrorException>(() => _searchProviderSelector.Get("foobar"));
		}

		[Fact]
		public void WithOnlyValidProviderFlagAndArgValue_ShouldGetException()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("");

			Should.Throw<BadRequestException>(() => _searchProviderSelector.Get("--provider provider1"));
		}

		[Fact]
		public void WithInValidProviderFlagAndArgValue_ShouldGetException()
		{
			_defaultSearchProviderConfigurationMock.Setup(x => x.DefaultName).Returns("");

			Should.Throw<BadRequestException>(() => _searchProviderSelector.Get("--provider blah"));
		}
	}
}
