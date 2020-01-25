namespace NETStandardLibrary.Linq
{
	public class SearchParameters
	{
		public SearchFieldList Fields { get; set; }
		public OrderByClauseList OrderBys { get; set; }
		public int? Page { get; set; }
		public int? PageSize { get; set; }
	}
}
