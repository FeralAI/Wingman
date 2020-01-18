using System.Linq;

namespace NETStandardLibrary.Search
{
	public static class IQueryableExtensions
	{
		public static SearchResults<T> Search<T>(this IQueryable<T> @this, SearchParameters parameters)
		{
			return SearchMethods.Search<T>(@this, parameters);
		}
	}
}
