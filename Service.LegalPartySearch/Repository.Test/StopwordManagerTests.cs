using System.Collections.Generic;
using Shouldly;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
using Xunit;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Test
{
	public class StopwordManagerTests
	{
		[Fact]
		public void WordIsCachedButWordNotInListShouldNotMatch()
		{
			var words = new List<string> { "foo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("foolish").ShouldBe(false);
		}

		[Fact]
		public void WordIsCachedAndWordIsInListAtStartShouldMatch()
		{
			var words = new List<string> { "foo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("foo bar one two").ShouldBe(true);
		}

		[Fact]
		public void WordIsCachedAndWordIsInListAtEndShouldMatch()
		{
			var words = new List<string> { "foo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("bar one two foo").ShouldBe(true);
		}

		[Fact]
		public void WordIsCachedAndWordIsInListAtMiddleShouldMatch()
		{
			var words = new List<string> { "foo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one bar foo two three").ShouldBe(true);
		}

		[Fact]
		public void MultipleWordsAreCachedAndWordsIsInListShouldMatch()
		{
			var words = new List<string> { "foo", "doo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.CacheIfStopword("doo three").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one bar foo two doo three").ShouldBe(true);
		}

		[Fact]
		public void MultipleWordsAreCachedAndOnlySomeWordsIsInListShouldNotMatch1()
		{
			var words = new List<string> { "foo", "doo", "coo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.CacheIfStopword("doo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one bar foo two doo three").ShouldBe(true);
		}

		[Fact]
		public void MultipleWordsAreCachedAndOnlySomeWordsIsInListShouldNotMatch2()
		{
			var words = new List<string> { "foo", "doo", "coo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.CacheIfStopword("doo fout").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one bar foo two doo three").ShouldBe(false);
		}

		[Fact]
		public void MultipleWordsAreCachedAndOnlySomeWordsIsInListShouldNotMatch3()
		{
			var words = new List<string> { "foo", "doo", "coo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.CacheIfStopword("doo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one doo three bar foo two doo three").ShouldBe(true);
		}

		[Fact]
		public void MultipleWordsAreCachedAndOnlySomeWordsIsInListShouldNotMatch4()
		{
			var words = new List<string> { "foo", "doo", "coo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("foo").ShouldBe(true);
			stopwordManager.CacheIfStopword("doo").ShouldBe(true);
			stopwordManager.ShouldCheck().ShouldBe(true);
			stopwordManager.MatchAllStopwords("one doo three bar foo two doo four").ShouldBe(true);
		}

		[Fact]
		public void WordNotInListIsNotCachedWithShouldCheckBeFalse()
		{
			var words = new List<string> { "foo", "doo", "coo" };
			var stopwordManager = new StopwordManager(words);
			stopwordManager.CacheIfStopword("bar").ShouldBe(false);
			stopwordManager.ShouldCheck().ShouldBe(false);
			// Don't be confused. This is returning true NOT because all stop words are matching,
			// but because we need to tell the caller the this case is not applicable since cache list is empty.
			stopwordManager.MatchAllStopwords("one bar foo two doo three").ShouldBe(true);
		}
	}
}
