using System.Linq;

namespace NETStandardLibrary.Search
{
	public static class SearchMethods
	{
		public static SearchResults<T> Search<T>(IQueryable<T> queryable, SearchParameters parameters)
		{
			var results = new SearchResults<T>
			{
				Page = parameters?.Page,
				PageSize = parameters?.PageSize,
				Parameters = parameters,
				Results = queryable,
			};

			if (parameters == null || parameters.Fields == null || parameters.Fields.Count == 0)
			{
				results.TotalCount = queryable.Count();
				return results;
			}

			foreach(var field in parameters.Fields)
			{
				// TODO: do something cool
			}

			if (parameters.OrderBys != null)
			{
				foreach(var orderBy in parameters.OrderBys)
				{
					// TODO: do something cooler
				}
			}

			if ((parameters.PageSize ?? 0) > 0)
			{
				results.Results = results.Results;
			}

			return results;
		}
	}
}
