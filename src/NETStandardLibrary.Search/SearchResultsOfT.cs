using System;
using System.Linq;

namespace NETStandardLibrary.Search
{
	public class SearchResults<T>
	{
		public SearchParameters Parameters { get; set; }
		public IQueryable<T> Results { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
		public int TotalCount { get; set; }

		public bool HasPaging => (Page ?? 0) > 0 && (PageSize ?? 0) > 0;
		public int TotalPages => ((PageSize ?? 0) == 0) ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
	}
}
