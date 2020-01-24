using System.Collections.Generic;

namespace NETStandardLibrary.Linq
{
	public class SearchParameters
	{
		public List<SearchField> Fields { get; set; }
		public List<OrderByClause> OrderBys { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
	}
}
