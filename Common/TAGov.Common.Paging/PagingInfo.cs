using System;
using Microsoft.Extensions.Configuration;

namespace TAGov.Common.Paging
{
	public class PagingInfo : IPagingInfo
	{
		private const int DefaultMaxRows = 1000;
		public PagingInfo(IConfiguration configuration)
		{
			var maxRows = configuration["paging:maxRows"];
			MaxRows = string.IsNullOrEmpty(maxRows) ? DefaultMaxRows : Convert.ToInt32(maxRows);
		}
		public int MaxRows { get; private set; }

        public void OverrideMaxRowsValue(int maxRows)
        {
            MaxRows = maxRows;
        }
	}
}
