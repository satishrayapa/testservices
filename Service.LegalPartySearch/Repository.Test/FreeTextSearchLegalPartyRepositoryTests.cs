//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Shouldly;
//using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
//using TAGov.Services.Core.LegalPartySearch.Repository.Models.V1;
//using TAGov.Common.Paging;
//using Microsoft.Extensions.Configuration;
//using System.Collections.Generic;

//namespace TAGov.Services.Core.LegalPartySearch.Repository.Test
//{
//	public class FreeTextSearchLegalPartyRepositoryTests
//	{
//		private readonly FreeTextSearchLegalPartyRepository _legalPartyRepository;

//		public FreeTextSearchLegalPartyRepositoryTests()
//		{
//			var optionsBuilder = new DbContextOptionsBuilder<SearchLegalPartyContext>();
//			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

//			var legalPartyContext = new SearchLegalPartyContext(optionsBuilder.Options);
//			var builder = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>() { [$"Paging:MaxRows"] = "700" });
			
//			_legalPartyRepository = new FreeTextSearchLegalPartyRepository(legalPartyContext, new Mock<ILogger>().Object, new PagingInfo(builder.Build()));

//			var fooOne = new SearchLegalParty
//			{
//				DisplayName = "foo one"
//			};
//			legalPartyContext.LegalParties.AddRange(fooOne, new SearchLegalParty
//			{
//				DisplayName = "foo two"
//			});

//			legalPartyContext.SaveChanges();
//		}

//		// TODO: When we swtich to use EF Core 2, let's check if Contains is supported.
//		//[Fact]
//		public void SearchAsyncWithDisplayNameShouldGetResults()
//		{
//			var results = _legalPartyRepository.SearchAsync("foo", null)
//				.Result.OrderBy(x => x.DisplayName).ToList();

//			results.Count.ShouldBe(2);

//			results[0].DisplayName.ShouldBe("foo one");
//			results[1].DisplayName.ShouldBe("foo two");
//		}
//	}
//}
