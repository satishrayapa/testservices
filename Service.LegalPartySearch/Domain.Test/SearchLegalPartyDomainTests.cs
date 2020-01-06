using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Domain.Implementation;
using TAGov.Services.Core.LegalPartySearch.Domain.Models.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Interfaces.V1;
using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;
using Xunit;
using TAGov.Common.Paging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TAGov.Services.Core.LegalPartySearch.Domain.Interfaces;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Test
{
	public class SearchLegalPartyDomainTests
	{
		private readonly SearchLegalPartyDomain _legalPartyDomain;
		private readonly Mock<ISearchLegalPartyRepository> _legalPartyRepositoryMock;
		private readonly PagingInfo _pagingInfo;
		private readonly Mock<ISearchResultsConfiguration> _searchResultsConfigurationMock;

		public SearchLegalPartyDomainTests()
		{
			_legalPartyRepositoryMock = new Mock<ISearchLegalPartyRepository>();
			_searchResultsConfigurationMock = new Mock<ISearchResultsConfiguration>();

			var provider = new Mock<ISearchProviderSelector>();

			provider.Setup(x => x.Get(It.IsAny<string>()))
				.Returns<string>(searchText =>
					new SearchProvider
					{
						Provider = _legalPartyRepositoryMock.Object,
						ParsedSearchText = searchText
					}
				);

			_searchResultsConfigurationMock.Setup(x => x.MaxRows).Returns(35000);

			var configurationMock = new Mock<IConfigurationRoot>();

			configurationMock.Setup(x => x["paging:maxRows"]).Returns("701");

			_pagingInfo = new PagingInfo(configurationMock.Object);

			_legalPartyDomain = new SearchLegalPartyDomain(provider.Object, new Mock<ILogger>().Object, _pagingInfo, _searchResultsConfigurationMock.Object);
		}

		[Fact]
		public void SearchAsyncShouldGetDisplayNameSpecificResults()
		{
			var list = new List<SearchLegalParty> { new SearchLegalParty { DisplayName = "foo one" } };

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("foo", It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(list);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "foo" }).Result.ToList();

			results.Count.ShouldBe(1);
			results[0].DisplayName.ShouldBe("foo one");
		}

		[Fact]
		public void SearchAsyncShouldGetTrimmedCommAddrSpecificResults()
		{
			var list = new List<SearchLegalParty> { new SearchLegalParty { DisplayName = "", Address = "    123 bar st   " } };

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("bar", It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(list);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "bar" }).Result.ToList();

			results.Count.ShouldBe(1);
			results[0].Address.ShouldBe("123 bar st");
		}

		[Fact]
		public void SearchAsyncShouldGetTrimmedLegalPartyRoleSpecificResults()
		{
			var list = new List<SearchLegalParty> { new SearchLegalParty { DisplayName = "", LegalPartyRole = "    owner   " } };

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("bar", It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(list);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "bar" }).Result.ToList();

			results.Count.ShouldBe(1);
			results[0].LegalPartyRole.ShouldBe("owner");
		}

		[Fact]
		public void SearchAsyncWithEmptySearchTextShouldGetBadRequestException()
		{
			var ex = Should.Throw<AggregateException>(
				() => _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "" }).Wait());

			ex.InnerExceptions.Single().ShouldBeOfType<BadRequestException>();
		}

		[Fact]
		public void SearchAsyncWithNullSearchTextShouldGetBadRequestException()
		{
			var ex = Should.Throw<AggregateException>(
				() => _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto()).Wait());

			ex.InnerExceptions.Single().ShouldBeOfType<BadRequestException>();
		}

		[Fact]
		public void SearchAsyncWithAllExcludeTypedNullableBooleanParametersShouldGetBadRequestException()
		{
			var ex = Should.Throw<AggregateException>(
				() => _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto
				{
					SearchText = "foo",
					ExcludeAddress = true,
					ExcludeAin = true,
					ExcludeDisplayName = true,
					ExcludeGeoCode = true,
					ExcludePin = true,
					ExcludeTag = true,
					MaxRows = 200,
					IsActive = true,
					EffectiveDate = DateTime.Now,
					RevenueObjectIdIsNotNull = true,
					AppraisalSiteIdIsNotNull = true,
					MineralIsNotNullWithValue = true,
					RevenueObjectIsActive = true,
					AddressType = string.Empty
				}).Wait());

			ex.InnerExceptions.Single().ShouldBeOfType<BadRequestException>();
		}

		[Fact]
		public void SearchAsyncShouldGetFullAddressSpecificResults()
		{

				var addressId = 100;
				var addressEffectiveDate = new DateTime(2016, 1, 1);
				var addressRoleId = 200;
				var addressRoleEffectiveDate = new DateTime(2016, 2, 1);
				var addressType = "Commercial";
				var addressUnitNumber = "300";
				var addressStreetNumber = 9000;
				var addressStreetName = "UnitTestStreet";
				var neighborhood = "UnitTestNeighborhood";
				var pin = "777777777";
				var ain = "888888888";
				var effectiveDate = new DateTime(2016, 1, 1);
				var revenueObjectId = 777777777;
				var revenueObjectEffectiveDate = new DateTime(2016, 1, 1);
				const string geoCode = "432 abcde";
				const string tag = "432 3209 342";
				const string legalPartyType = "owner";
				const string legalPartySubType = "foobar";
				const string streetType = "DR";
				const int searchLegalPartyId = 546;
				const int legalPartyRoleId = 4575;
				const int legalPartyId = 34096;
				const int legalPartySubTypeId = 3252;
				const int legalPartyTypeId = 532532;
				const string source = "migrations";
				DateTime lastUpdated = new DateTime(2017, 1, 1);
				const string city = "Carrollton";
				const string state = "TX";
				const string postalCode = "75006";

				var list = new List<SearchLegalParty>
				{
					new SearchLegalParty
					{
						Id = searchLegalPartyId,
						LegalPartyRoleId = legalPartyRoleId,
						LegalPartyId = legalPartyId,
						DisplayName = "",
						AddressId = addressId,
						AddressEffectiveDate = addressEffectiveDate,
						AddressRoleId = addressRoleId,
						AddressRoleEffectiveDate = addressRoleEffectiveDate,
						AddressType = addressType,
						AddressUnitNumber = addressUnitNumber,
						AddressStreetNumber = addressStreetNumber,
						AddressStreetName = addressStreetName,
						Neighborhood = neighborhood,
						Pin = pin,
						Ain = ain,
						EffectiveDate = effectiveDate,
						RevenueObjectId = revenueObjectId,
						RevenueObjectEffectiveDate = revenueObjectEffectiveDate,
						GeoCode = geoCode,
						Tag = tag,
						LegalPartyType = legalPartyType,
						LegalPartySubType = legalPartySubType,
						StreetType = streetType,
						EffectiveStatus = "A",
						LegalPartySubTypeId = legalPartySubTypeId,
						LegalPartyTypeId = legalPartyTypeId,
						Source = source,
						LastUpdated = lastUpdated,
						PrimeAddress = false,
						City = city,
						State = state,
						PostalCode = postalCode,
						RevenueObjectEffectiveStatus = "A"
					}
				};

				_legalPartyRepositoryMock.Setup(x => x.SearchAsync("foo", It.IsAny<SearchLegalPartyQuery>()))
					.ReturnsAsync(list);

				var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto {SearchText = "foo"}).Result
					.ToList();

				results.Count.ShouldBe(1);
				results[0].Id.ShouldBe(searchLegalPartyId);
				results[0].LegalPartyId.ShouldBe(legalPartyId);
				results[0].LegalPartyRoleId.ShouldBe(legalPartyRoleId);
				results[0].AddressId.ShouldBe(addressId);
				results[0].AddressEffectiveDate.ShouldBe(addressEffectiveDate);
				results[0].AddressRoleId.ShouldBe(addressRoleId);
				results[0].AddressRoleEffectiveDate.ShouldBe(addressRoleEffectiveDate);
				results[0].AddressType.ShouldBe(addressType);
				results[0].AddressUnitNumber.ShouldBe(addressUnitNumber);
				results[0].AddressStreetNumber.ShouldBe(addressStreetNumber);
				results[0].AddressStreetName.ShouldBe(addressStreetName);
				results[0].Neighborhood.ShouldBe(neighborhood);
				results[0].Pin.ShouldBe(pin);
				results[0].Ain.ShouldBe(ain);
				results[0].EffectiveDate.ShouldBe(effectiveDate);
				results[0].RevenueObjectId.ShouldBe(revenueObjectId);
				results[0].RevenueObjectEffectiveDate.ShouldBe(revenueObjectEffectiveDate);
				results[0].RevenueObjectEffectiveStatus.ShouldBe("A");
				results[0].GeoCode.ShouldBe(geoCode);
				results[0].Tag.ShouldBe(tag);
				results[0].LegalPartyType.ShouldBe(legalPartyType);
				results[0].LegalPartySubType.ShouldBe(legalPartySubType);
				results[0].StreetType.ShouldBe(streetType);
				results[0].IsActive.ShouldBe(true);
				results[0].LegalPartySubTypeId.ShouldBe(legalPartySubTypeId);
				results[0].LegalPartyTypeId.ShouldBe(legalPartyTypeId);
				results[0].PrimeAddress.ShouldBe(false);
				results[0].City.ShouldBe(city);
				results[0].State.ShouldBe(state);
				results[0].PostalCode.ShouldBe(postalCode);


		}


		[Fact]
		public void SearchAsyncShouldGetTagAndAppraisalAttributesSpecificResults()
		{
			var tagId = 100;
			var tagBegEffYear = (short)2016;
			var tagRoleId = 200;
			var tagRoleBegEffDate = new DateTime(2016, 2, 1);
			var appraisalSiteId = 300;
			var appraisalBeginEffectiveDate = new DateTime(2016, 3, 1);
			var appraisalEffectiveStatus = "UnitTestAppraisalEffectiveStatus";
			var appraisalRoleId = 400;
			var appraisalRoleBeginEffectiveDate = new DateTime(2016, 4, 1);
			var appraisalRoleEffectiveStatus = "UnitTestAppraisalRoleEffectiveStatus";
			var appraisalSiteName = "UnitTestAppraisalSiteName";
			var appraisalClass = "UnitTestAppraisalClass";
			const string city = "Carrollton";
			const string state = "TX";
			const string postalCode = "75006";

			var list = new List<SearchLegalParty>
			{
				new SearchLegalParty
				{
					DisplayName = "",
					TagId = tagId,
					TagBegEffYear = tagBegEffYear,
					TagRoleId = tagRoleId,
					TagRoleBegEffDate = tagRoleBegEffDate,
					AppraisalSiteId = appraisalSiteId,
					AppraisalBeginEffectiveDate = appraisalBeginEffectiveDate,
					AppraisalEffectiveStatus = appraisalEffectiveStatus,
					AppraisalRoleId = appraisalRoleId,
					AppraisalRoleBeginEffectiveDate = appraisalRoleBeginEffectiveDate,
					AppraisalRoleEffectiveStatus = appraisalRoleEffectiveStatus,
					AppraisalSiteName = appraisalSiteName,
					AppraisalClass = appraisalClass,
					PrimeAddress = true,
					City = city,
					State = state,
					PostalCode = postalCode
				}
			};

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("foo", It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(list);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "foo" }).Result.ToList();

			results.Count.ShouldBe(1);
			results[0].TagId.ShouldBe(tagId);
			results[0].TagBegEffYear.ShouldBe(tagBegEffYear);
			results[0].TagRoleId.ShouldBe(tagRoleId);
			results[0].TagRoleBegEffDate.ShouldBe(tagRoleBegEffDate);
			results[0].AppraisalSiteId.ShouldBe(appraisalSiteId);
			results[0].AppraisalBeginEffectiveDate.ShouldBe(appraisalBeginEffectiveDate);
			results[0].AppraisalEffectiveStatus.ShouldBe(appraisalEffectiveStatus);
			results[0].AppraisalRoleId.ShouldBe(appraisalRoleId);
			results[0].AppraisalRoleBeginEffectiveDate.ShouldBe(appraisalRoleBeginEffectiveDate);
			results[0].AppraisalRoleEffectiveStatus.ShouldBe(appraisalRoleEffectiveStatus);
			results[0].AppraisalSiteName.ShouldBe(appraisalSiteName);
			results[0].AppraisalClass.ShouldBe(appraisalClass);
			results[0].PrimeAddress.ShouldBe(true);
			results[0].City.ShouldBe(city);
			results[0].State.ShouldBe(state);
			results[0].PostalCode.ShouldBe(postalCode);
		}

		[Fact]
		public void MaxRowIsCachedInPagingInfoIfValid()
		{
			// ReSharper disable once UnusedVariable
			var list = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "foo", MaxRows = 800 }).Result
				.ToList();
			_pagingInfo.MaxRows.ShouldBe(800);
		}

		[Fact]
		public void MaxRowIsNotCachedInPagingInfoIfValid()
		{
			// ReSharper disable once UnusedVariable
			var list = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "foo", MaxRows = 0 }).Result
				.ToList();
			_pagingInfo.MaxRows.ShouldBe(701);
		}

		[Fact]
		public void SearchSplitPinSitusResults()
		{
			var resultstring = GetFileContents("SplitPinSitusSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);

			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync("543210006", It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "543210006", AddressType = "situs", IsActive = true, MaxRows = 100 }).Result.ToList();

			// 1 result for each legalparty
			results.Count.ShouldBe(3);
			results.Count(item => item.Pin == "543210006").ShouldBe(1);
			results.Count(item => item.Pin == "543210027").ShouldBe(1);
			results.Count(item => item.Pin == "543210000").ShouldBe(1);
		}

		[Fact]
		public void SearchSplitPinMailingResults()
		{
			var resultstring = GetFileContents("SplitPinMailingSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync("543210006", It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "543210006", AddressType = "mailing", IsActive = true, MaxRows = 100 }).Result.ToList();

			// 2 results for each legalparty
			results.Count.ShouldBe(6);
			results.Count(item => item.Pin == "543210006").ShouldBe(2);
			results.Count(item => item.Pin == "543210027").ShouldBe(2);
			results.Count(item => item.Pin == "543210000").ShouldBe(2);
		}

		[Fact]
		public void SearchAliasTypeMailingResults()
		{
			var resultstring = GetFileContents("AliasTypeSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("FAMILY HAIR AFFAIR", It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "FAMILY HAIR AFFAIR", AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();

			// two results should pass through the filtering
			results.Count(item => item.DisplayName == "FAMILY HAIR AFFAIR").ShouldBe(1);
			results.Count(item => item.DisplayName == "FAMILY AFFAIR HAIR").ShouldBe(1);
		}

		[Fact]
		public void SearchLatestActiveLegalPartyNoRevObjSearchResults()
		{
			var resultstring = GetFileContents("LatestActiveLPNoRevObjSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			var searchText = "5620 GROUP";

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, EffectiveDate = DateTime.Now, AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();

			// two results should pass through the filtering
			results.Count.ShouldBe(1);
			results[0].RevenueObjectId.ShouldNotBeNull();
			results[0].Id.ShouldBe(12848);
		}

		[Fact]
		public void SearchLatestActiveLegalPartySearchResults()
		{
			var resultstring = GetFileContents("LatestActiveLPSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			var searchText = "SMITH HARUMI";

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, EffectiveDate = DateTime.Now, AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();

			// two results should pass through the filtering
			results.Count.ShouldBe(1);
			results[0].RevenueObjectId.ShouldNotBeNull();
			results[0].Id.ShouldBe(10884);
		}

		[Fact]
		public void SearchLatestActiveLegalPartyNoRevObjEffDateNullSearchResults()
		{
			var resultstring = GetFileContents("LatestActiveLPNoRevObjSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			var searchText = "5620 GROUP";

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();

			// two results should pass through the filtering
			results.Count.ShouldBe(1);
			results[0].RevenueObjectId.ShouldNotBeNull();
			results[0].Id.ShouldBe(12847);
		}

		[Fact]
		public void SearchLatestActiveLegalPartyEffDateNullSearchResults()
		{
			var resultstring = GetFileContents("LatestActiveLPSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			var searchText = "SMITH HARUMI";

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();

			// two results should pass through the filtering
			results.Count.ShouldBe(1);
			results[0].RevenueObjectId.ShouldNotBeNull();
			results[0].Id.ShouldBe(10883);
		}

		private string GetFileContents(string sampleFile)
		{
			var asm = Assembly.GetExecutingAssembly();
			var resource = string.Format("Domain.Test.Data.{0}", sampleFile);
			using (var stream = asm.GetManifestResourceStream(resource))
			{
				if (stream != null)
				{
					var reader = new StreamReader(stream);
					return reader.ReadToEnd();
				}
			}

			return string.Empty;
		}

		[Fact]
		public void SearchAddressMailingResults()
		{
			string acceptedAddress = "31715 RIVERSIDE DR";
			var resultstring = GetFileContents("AliasTypeSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("FAMILY HAIR AFFAIR", It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "FAMILY HAIR AFFAIR", AddressType = "mailing", MaxRows = 100, IsActive = true }).Result.ToList();
			Assert.Contains(results, x => x.Address.Trim() == acceptedAddress);
		}


		[Fact]
		public void SearchOldPinNewPinResults()
		{
			var latestPin = "7602311400(7602201700)";
			var resultstring = GetFileContents("OldPinNewPinSearchResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync("7602201700", It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "7602201700", AddressType = "situs", IsActive = true, MaxRows = 100 }).Result.ToList();

			results.Count.ShouldBe(1);
			Assert.True(results.Single().Pin == latestPin);
		}

		[Fact]
		public void SearchLegalPartyByPinOnlyReturnSingleActiveLegalPartyResult()
		{
			var pin = "6403230900";
			var resultstring = GetFileContents("ActiveAndInactiveLegalPartyResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "6403230900", AddressType = "mailing", MaxRows = 100 }).Result.ToList();

			results.Count.ShouldBe(1);
			Assert.True(results.Single().Pin == pin);
		}

		[Fact]
		public void SearchGroupedbyRevenueObjAndLegalPartyResults()
		{
			var pin = "000000438";
			var resultstring = GetFileContents("SearchGroupedbyRevenueObjAndLegalPartyResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "000000438", AddressType = "mailing", MaxRows = 100 }).Result.ToList();

			results.Count.ShouldBe(1);
			Assert.True(results[0].Pin == pin);
		}

		[Fact]
		public void SearchPinWithMultipleLegalPartyAddressOfSameDateAndMultipleEffectiveDatedRevenueObjectsSameId()
		{
			var pin = "102020049";
			var resultstring = GetFileContents("MultipleLegalPartyAddressesOfSameDateMultipleEffectiveRevenueObjects.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "102020049", AddressType = "situs", MaxRows = 701, CoalesceIfDuplicateAddress = true, EffectiveDate = new DateTime(2018, 6, 13, 23, 59, 59), RevenueObjectIdIsNotNull = true }).Result.ToList();

			results.Count.ShouldBe(1);
			results[ 0 ].RevenueObjectEffectiveStatus.ShouldBe( "I" );
		}


		[Fact]
		public void SearchPinWithMultipleSitusAndFilterAddressbyMaxEffectiveDateResults()
		{
			var pin = "000000438";
			var resultstring = GetFileContents("SearchPinWithMultipleSitusAndFilterAddressbyMaxEffectiveDateResults.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = "000000438", AddressType = "mailing", MaxRows = 100 }).Result.ToList();

			results.Count.ShouldBe(1);
			results[0].Pin.ShouldBe(pin);
			results[0].AddressRoleEffectiveDate.ShouldBe(new DateTime(2018, 5, 15));
		}


		[Fact]
		public void SearchOwnedPinResults()
		{
			var searchText = "BROWN PATRICIA L";
			var resultstring = GetFileContents("OwnedPins.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, MaxRows = 100 }).Result.ToList();

			foreach (var rec in results)
			{
				if (!rec.IsActive)
				{
					Assert.True(rec.Pin==string.Empty);
					Assert.True(rec.Ain == string.Empty);
					Assert.True(rec.GeoCode == string.Empty);
				}
				else
				{
					Assert.True(rec.Pin== "1010400100");
					Assert.True(rec.Ain == "1010400100");
					Assert.True(rec.GeoCode == "1010400100");
				}
			}

		}

		[Fact]
		public void SearchWithZeroResultsVerifyCodeNotCrashing()
		{
			var searchText = "45643463456";
			var resultstring = GetFileContents("ZeroResultsReturn.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(searchText, It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = searchText, MaxRows = 100 }).Result.ToList();

			Assert.True(results.Count == 0);
		}


		[Fact]
		public void SearchPinWithLeadingZerosThatDoesntExist()
		{
			var pin = "000000012";
			var resultstring = GetFileContents("SearchPinStartWithZero.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = pin, AddressType = "situs", MaxRows = 101 }).Result.ToList();

			results.Count.ShouldBe(0);
		}

		[Fact]
		public void SearchPinWithLeadingZerosThatDoesExist()
		{
			var pin = "000000015";
			var resultstring = GetFileContents("SearchPinStartWithZero.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = pin, AddressType = "situs", MaxRows = 101 }).Result.ToList();

			results.Count.ShouldBe(1);
		}

		[Fact]
		public void SearchSitusRecords()
		{
			var pin = "7602201700";
			var latestPin = "7602311400(7602201700)";
			var resultstring = GetFileContents("SitusPrimaryOwnerActiveSearchResult.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = pin, AddressType = "situs", MaxRows = 101 }).Result.ToList();

			results.Count.ShouldBe(1);
			Assert.True(results.Single().Pin == latestPin);
		}

		[Fact]
		public void SearchSitusRecordsReturningMoreThanOneResult()
		{
			var pin = "102130003";
			var resultstring = GetFileContents("MorethanOneResultSitusPinSearch.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchPinAsync(pin, It.IsAny<SearchLegalPartyQuery>(), It.IsAny<bool?>(), It.IsAny<DateTime?>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = pin, AddressType = "situs", MaxRows = 101 }).Result.ToList();

			results.Count.ShouldBeGreaterThan(1);

		}

		[Fact]
		public void SearchAlphaNumericPinThatDoesExist()
		{
			var pin = "0299FG";
			var resultstring = GetFileContents("AlphaNumericPinInActiveAndActiveLPRs.json");
			var reporesults = JsonConvert.DeserializeObject<IEnumerable<SearchLegalParty>>(resultstring);
			_legalPartyRepositoryMock.Setup(x => x.SearchAsync(pin, It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(reporesults);

			var results = _legalPartyDomain.SearchAsync(new SearchLegalPartyQueryDto { SearchText = pin, AddressType = "situs", MaxRows = 101 }).Result.ToList();

			results.Count.ShouldBe(1);
		}

		[Fact]
		public void SearchAsyncShouldOnlyReturnUpToMaxRowsFromConfig()
		{
			var list = new List<SearchLegalParty> { new SearchLegalParty { DisplayName = "", Address = "    123 bar st   " } };

			_legalPartyRepositoryMock.Setup(x => x.SearchAsync("bar", It.IsAny<SearchLegalPartyQuery>())).ReturnsAsync(list);

			var searchLegalPartyQueryDto = new SearchLegalPartyQueryDto {SearchText = "bar", MaxRows = 50000};

			var results = _legalPartyDomain.SearchAsync(searchLegalPartyQueryDto).Result.ToList();

			searchLegalPartyQueryDto.MaxRows.ShouldBe(35000);
		}

	}
}
