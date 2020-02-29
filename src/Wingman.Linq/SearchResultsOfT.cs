using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Wingman.Linq
{
	public class SearchResults<T>
	{
		public IEnumerable<T> Results { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
		public int TotalCount { get; set; }
		[JsonIgnore]
		public bool HasPaging => (Page ?? 0) > 0 && (PageSize ?? 0) > 0;
		[JsonIgnore]
		public int TotalPages
		{
			get
			{
				var pageSize = !PageSize.HasValue || PageSize.Value <= 0
					? Math.Max(TotalCount, 1)
					: PageSize.Value;

				return (int)Math.Ceiling(TotalCount / (double)pageSize);
			}
		}
	}
}
