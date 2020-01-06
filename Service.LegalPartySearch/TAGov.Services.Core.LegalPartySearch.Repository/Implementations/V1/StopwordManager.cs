using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Implementations.V1
{
	public class StopwordManager
	{
		private readonly List<string> _stopwords;
		private readonly List<string> _cacheStopwords = new List<string>();
		private readonly Dictionary<string, Regex> _matchPatterns = new Dictionary<string, Regex>();

		public StopwordManager(IEnumerable<string> stopwords)
		{			
			_stopwords = stopwords.Select(x => x.ToLower()).Distinct().ToList();

			_stopwords.ForEach(x =>
			{
				_matchPatterns.Add(x, new Regex(@"^[#\""]?" + x + @"[\.\""]?$", RegexOptions.IgnoreCase));
			});
		}

		public bool CacheIfStopword(string term)
		{
			if (_stopwords.Count == 0) return false;

			var cleaned = term.Trim('\"');
			var cleanedTerms = cleaned.Split(' ').Select(x => x.ToLower()).ToList();

			if (_stopwords.Any(sw => cleanedTerms.Any(ct => GetRegex(sw).IsMatch(ct))))
			{
				_cacheStopwords.Add(cleaned);
				return true;
			}
			return false;
		}

		private Regex GetRegex(string match)
		{
			return _matchPatterns[match];
		}

		public bool ShouldCheck()
		{
			return _cacheStopwords.Count > 0;
		}

		public bool MatchAllStopwords(string searchAllText)
		{
			return _cacheStopwords.All(searchAllText.CaseInsensitiveContains);
		}
	}
}