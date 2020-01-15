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
		public int TotalPages
		{
			get
			{
				if ((PageSize ?? 0) == 0)
					return 1;

				return (int)Math.Ceiling(TotalCount / (double)PageSize);
			}
		}
	}
}
