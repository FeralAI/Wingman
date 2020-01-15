using System.Collections.Generic;
using NETStandardLibrary.Linq;

namespace NETStandardLibrary.Search
{
	public class SearchParameters
	{
		public List<SearchField> Fields { get; set; }
		public List<OrderByClause> OrderBys { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
	}
}
