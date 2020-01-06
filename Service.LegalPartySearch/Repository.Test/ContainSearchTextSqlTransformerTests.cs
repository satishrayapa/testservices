using System.Collections.Generic;
using Shouldly;
using TAGov.Common.Exceptions;
using TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1;
using Xunit;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Test
{
	public class ContainSearchTextSqlTransformerTests
	{
		[Fact]
		public void OneWordSearchTextShouldBeDoubleQuoted()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\"");
		}

		[Fact]
		public void OneWordSearchTextWithIndependentStarShouldGetException1()
		{
			var transformer = new ContainSearchTextSqlTransformer("* foo", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordSearchTextWithIndependentStarShouldGetException2()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo *", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordSearchTextWithStarAtEndShouldBeDoubleQuoted()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo*", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo*\"");
		}

		[Fact]
		public void OneWordSearchTextWithStartAtStartShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("*foo", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordSearchTextWithStarInMiddleShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("fo*bar", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuoteSearchTextShouldBeLeftAloneWithNoChanges()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"foo\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\"");
		}

		[Fact]
		public void OneWordWithDoubleQuoteSearchTextShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"foo", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void TwoWordsSearchTextShouldBeDoubleQuotedEachWithAndInBetween()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo bar", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("(\"foo\" and \"bar\") or (\"foo bar\")");
		}

		[Fact]
		public void TwoWordsWithDoubleQuoteSearchTextShouldHaveAndIncludedInBetween()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"foo one\" \"bar two\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo one\" and \"bar two\"");
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSearchTextShouldHaveAndIncludedInBetween()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"bar two\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\" and \"one\" and \"bar two\"");
		}

		[Fact]
		public void MixedWordsWithMoreThanTwoWordsSomeWithDoubleQuoteSearchTextShouldHaveAndIncludedInBetween()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"bar two three\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\" and \"one\" and \"bar two three\"");
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithStarSearchTextShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("*foo one* \"bar two\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException1()
		{
			var transformer = new ContainSearchTextSqlTransformer("* foo one \"bar two\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException2()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo * one \"bar two\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException3()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one * \"bar two\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException4()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"* bar two\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\" and \"one\" and \"* bar two\"");
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException5()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"bar * two\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\" and \"one\" and \"bar * two\"");
		}

		[Fact]
		public void MixedWordsSomeWithDoubleQuoteSomeWithIndependentStarSearchTextShouldGetException6()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"bar two *\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo\" and \"one\" and \"bar two *\"");
		}

		[Fact]
		public void MixedWordsSomeWithNonePairingQuoteSearchTextShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo one \"bar two\" \"three", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void DoubleQuoteWordThatEndsWithStarShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"fobar\"*", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void DoubleQuoteWordThatStartsWithStarShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("*\"fobar\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void WordShouldBeExcludedWhenShouldAddTermReturnTrue()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo bar one", new StopwordManager(new List<string> ()));
			transformer.GetSqlFriendly().ShouldBe("(\"foo\" and \"bar\" and \"one\") or (\"foo bar one\")");
		}

		[Fact]
		public void WordsWithSpacesAtEndShouldBeTrimmed()
		{
			var transformer = new ContainSearchTextSqlTransformer("foo bar ", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("(\"foo\" and \"bar\") or (\"foo bar \")");
		}

		[Fact]
		public void WordsWithSpacesAtStartShouldBeTrimmed()
		{
			var transformer = new ContainSearchTextSqlTransformer(" foo bar", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("(\"foo\" and \"bar\") or (\" foo bar\")");
		}

		[Fact]
		public void WordsWithSpacesAtStartAndEndShouldBeTrimmed()
		{
			var transformer = new ContainSearchTextSqlTransformer(" foo bar ", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("(\"foo\" and \"bar\") or (\" foo bar \")");
		}

		[Fact]
		public void SingleCharacterInDoubleQuoteShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"f\"", new StopwordManager(new List<string>( )));
			transformer.GetSqlFriendly().ShouldBe("\"f\"");
		}

		[Fact]
		public void MultipleSingleCharactersInDoubleQuoteShouldWork()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"f o o\"", new StopwordManager(new List<string> { "f", "o" }));
			transformer.GetSqlFriendly().ShouldBe("\"f o o\"");
		}

		[Fact]
		public void TwoWordsWithDoubleQuoteSearchTextWithStarAtEndShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"foo bar\"*", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void TwoWordsWithDoubleQuoteSearchTextWithStarAtStartShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("*\"foo bar\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void TwoWordsWithDoubleQuoteSearchTextWithStarAtBothStartAndEndShouldGetException()
		{
			var transformer = new ContainSearchTextSqlTransformer("*\"foo bar\"*", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuoteSearchTextWithSingleCharacterAtEndShouldBeLeftAlone()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"foo f\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"foo f\"");
		}

		[Fact]
		public void OneWordWithDoubleQuoteSearchTextWithSingleCharacterAtStartShouldBeLeftAlone()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"f bar\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"f bar\"");
		}

		[Fact]
		public void TwoWordsWithOneInDoubleQuoteSearchTextWithShouldBeTransformed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"C&\" FOO", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"C&\" and \"FOO\"");
		}

		///////////////////////////////////////////
		// Special Characters Testing

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithExclamationShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd!", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithExclamationShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd!\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd!\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithDoubleQuoteShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd\"", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithDoubleQuoteShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd\"\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd\"\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithNumberSignShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd#", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithNumberSignShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd#\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd#\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithDollarSignShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd!", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithDollarSignShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd$\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd$\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithPercentShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd%", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithPercentShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd%\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd%\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithAmpersandShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd&", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithAmpersandShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd&\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd&\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithSingleQuoteShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd'", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithSingleQuoteShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd'\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd'\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithLeftparenthesisShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd(", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithLeftparenthesisShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd(\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd(\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithRightparenthesisShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd)", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithRightparenthesisShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd)\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd)\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithAsteriskShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd*", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd*\"");
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithAsteriskShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd*\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd*\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithPlusShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd+", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithPlusShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd+\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd+\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithCommaShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd,", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithCommaShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd,\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd,\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithMinusShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd-", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithMinusShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd-\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd-\"");
		}

		[Fact] public void OneWordWithoutDoubleQuotesSearchTextWithFullStopShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd.", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithFullStopShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd.\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd.\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithSlashShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd/", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithSlashShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd/\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd/\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithColonShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd:", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithColonShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd:\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd:\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithSemicolonShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd;", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithSemicolonShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd;\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd;\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithLessThanShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd<", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithLessThanShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd<\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd<\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithEqualSignShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd=", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithEqualSignShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd=\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd=\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithGreterThanShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd>", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithGreaterThanShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd>\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd>\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithQuestionMarkShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd?", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithQuestionMarkShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd?\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd?\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithAtSignShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd@", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithAtSignShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd@\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd@\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithLeftBracketShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd[", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithLeftBracketShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd[\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd[\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithBackslashShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd\\", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithBackslashShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd\\\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd\\\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithRighbracketShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd]", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithRightbracketShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd]\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd]\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithCaretShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd^", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithCaretShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd^\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd^\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithUnderscoreShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd_", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithUnderscoreShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd_\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd_\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithGraveaccentShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd`", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithGraveaccentShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd`\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd`\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithLeftBraceShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd{", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithLeftBraceShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd{\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd{\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithVerticalBarShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd|", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithVerticalBarShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd|\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd|\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithRightBraceShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd}", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithRightBraceShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd}\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd}\"");
		}

		[Fact]
		public void OneWordWithoutDoubleQuotesSearchTextWithTildeShouldFail()
		{
			var transformer = new ContainSearchTextSqlTransformer("abcd~", new StopwordManager(new List<string>()));
			Should.Throw<BadRequestException>(() => transformer.GetSqlFriendly());
		}

		[Fact]
		public void OneWordWithDoubleQuotesSearchTextWithTildeShouldSucceed()
		{
			var transformer = new ContainSearchTextSqlTransformer("\"abcd~\"", new StopwordManager(new List<string>()));
			transformer.GetSqlFriendly().ShouldBe("\"abcd~\"");
		}
	}
}
