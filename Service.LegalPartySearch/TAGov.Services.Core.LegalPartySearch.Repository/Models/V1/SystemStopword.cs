using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("fulltext_system_stopwords", Schema = "sys")]
	public class SystemStopword
	{
		[Key]
		public string Stopword { get; set; }

		[Key]
		[Column("language_id")]
		public int LanguageId { get; set; }
	}
}
