using System;
using System.Linq;

namespace NETStandardLibrary.Linq
{
	public class SearchResults<T>
	{
		public SearchParameters Parameters { get; set; }
		public IQueryable<T> Results { get; set; }
		public int TotalCount { get; set; }

		public bool HasPaging => (Page ?? 0) > 0 && (PageSize ?? 0) > 0;
		public int? Page => Parameters?.Page;
		public int? PageSize => Parameters?.PageSize;
		public int TotalPages => ((PageSize ?? 0) <= 0) ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
	}
}
