using System.Linq;

namespace NETStandardLibrary.Search
{
	public static class IQueryableExtensions
	{
		public static SearchResults<T> Search<T>(this IQueryable<T> query, SearchParameters parameters)
		{
			return SearchMethods.Search<T>(query, parameters);
		}
	}
}
