namespace NETStandardLibrary.Linq
{
	public class SearchParameters
	{
		public WhereClause WhereClause { get; set; }
		public OrderByClauseList OrderBys { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
	}
}
