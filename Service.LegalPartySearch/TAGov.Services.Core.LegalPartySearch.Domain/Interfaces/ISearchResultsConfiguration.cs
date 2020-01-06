using System;
using System.Collections.Generic;
using System.Text;

namespace TAGov.Services.Core.LegalPartySearch.Domain.Interfaces
{
	public interface ISearchResultsConfiguration
	{
		int MaxRows { get; }
	}
}
