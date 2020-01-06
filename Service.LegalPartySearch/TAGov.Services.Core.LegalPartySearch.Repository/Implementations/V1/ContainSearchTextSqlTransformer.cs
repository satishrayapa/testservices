using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Internal;
using TAGov.Common.Exceptions;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class ContainSearchTextSqlTransformer
	{
		private readonly string _searchText;
		private string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
		public ContainSearchTextSqlTransformer(string searchText, StopwordManager stopStopwordManager)
		{
			_searchText = searchText;
		}

		public string GetSqlFriendly()
		{
			var terms = new List<string>();

			// split our terms into tokens for processing
			var parsed = _searchText.Split(' ').Where(x => !string.IsNullOrEmpty(x));

			bool detectedStartOfTerm = false;
			foreach (var item in parsed)
			{
				if (item.StartsWith("\""))
				{
					if (!item.EndsWith("\""))
					{
						detectedStartOfTerm = true;
					}
					terms.Add(item);
				}
				else if (item.EndsWith("\""))
				{
					if (!detectedStartOfTerm)
					{
						terms.Add(item);
					}
					else
					{
						detectedStartOfTerm = false;
						terms[terms.Count - 1] = terms[terms.Count - 1] += " " + item;
					}
				}
				else
				{
					if (detectedStartOfTerm)
					{
						terms[terms.Count - 1] = terms[terms.Count - 1] += " " + item;
					}
					else
					{
						terms.Add(item);
					}
				}
			}

			var result = terms.Select(term =>
			{
				// if term starts with double quote and ends with double quote you can have any character
				// no check necessary

				// if starts with star and ends in quote, should throw exception
				if (term.StartsWith("*"))
				{
					throw new BadRequestException(@"The search term(s) entered contains special character(s). Please ensure that the search term(s) with special character(s) are placed within a double quote; ex: ""E * Trade"" or ""Arco AM / PM"".");
				}

				// if starts with star and ends in quote, should throw exception
				if (term.StartsWith("\"") && term.EndsWith("*"))
				{
					throw new BadRequestException(@"The search term(s) entered contains special character(s). Please ensure that the search term(s) with special character(s) are placed within a double quote; ex: ""E * Trade"" or ""Arco AM / PM"".");
				}

				// if term has no double quotes at start and end you can't have special characters
				if (!(term.StartsWith("\"") && term.EndsWith("\"")))
				{
					if (!term.EndsWith("*"))
						if (-1 < term.IndexOfAny(specialCharacters.ToCharArray()))
							throw new BadRequestException(@"The search term(s) entered contains special character(s). Please ensure that the search term(s) with special character(s) are placed within a double quote; ex: ""E * Trade"" or ""Arco AM / PM"".");
				}

				return !term.StartsWith("\"") ? "\"" + term + "\"" : term;

			}).Where(x => !string.IsNullOrEmpty(x)).ToList().Join(" and ");

			if (string.IsNullOrEmpty(result))
				throw new BadRequestException("There was an error with the syntax. All search text provided are stop words that are not indexed.");

			if ( (terms.Count > 1) && !terms.Any(t=>t.Contains("\"")))
				result = "(" + result + ")" + " or (\"" + _searchText + "\")";

			return result;
		}
	}
}

