using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	public class CrawlProgress
	{
		[Key]
		[Column(Order = 0)]
		public int IndexRows { get; set; }
		public int TotalRows { get; set; }
		public string Status { get; set; }
	}
}
