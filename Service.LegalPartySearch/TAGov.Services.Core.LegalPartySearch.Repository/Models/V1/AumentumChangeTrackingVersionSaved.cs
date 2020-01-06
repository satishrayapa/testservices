﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TAGov.Services.Core.LegalPartySearch.Repository.Models.V1
{
	[Table("AumentumChangeTrackingVersionSaved", Schema = "search")]
	public class AumentumChangeTrackingVersionSaved
	{
		[Key]
		public int Id { get; set; }
		public string TableName { get; set; }
		public long ChangeVersion { get; set; }
	}
}
