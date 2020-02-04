using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Wingman.Linq
{
	public class SearchResults<T>
	{
		[JsonIgnore]
		public bool HasPaging => (Page ?? 0) > 0 && (PageSize ?? 0) > 0;
		public int? Page { get; set; }
		public int? PageSize { get; set; }
		public IEnumerable<T> Results { get; set; }
		public int TotalCount { get; set; }
		[JsonIgnore]
		public int TotalPages => ((PageSize ?? 0) <= 0) ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
	}
}
