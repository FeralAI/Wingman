using System.Linq;
using LinqKit;
using NETStandardLibrary.Linq;

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

			// TODO: https://www.c-sharpcorner.com/UploadFile/c42694/dynamic-query-in-linq-using-predicate-builder/
			// TODO: http://www.albahari.com/nutshell/predicatebuilder.aspx
			var wherePredicate = PredicateBuilder.New<T>(true);
			foreach(var field in parameters.Fields)
			{
				var expression = ExpressionMethods.ToWhereClauseExpression<T>(
					field.Name,
					field.Value,
					field.Value.GetType(),
					field.Operator
				);

				wherePredicate = wherePredicate.And(expression);
			}

			// NOTE: Is the .AsExpandable() really needed here? Might only be for SQL Server...
			var searchResults = (IOrderedQueryable<T>)queryable.AsExpandable().Where(wherePredicate);

			if (parameters.OrderBys != null)
			{
				var firstOrderBy = true;
				foreach(var orderBy in parameters.OrderBys)
				{
					if (firstOrderBy)
					{
						if (orderBy.Direction == OrderByDirection.ASC)
							searchResults = searchResults.OrderBy(orderBy.Name);
						else if (orderBy.Direction == OrderByDirection.DESC)
							searchResults = searchResults.OrderByDescending(orderBy.Name);
					}
					else
					{
						if (orderBy.Direction == OrderByDirection.ASC)
							searchResults = searchResults.ThenBy(orderBy.Name);
						else if (orderBy.Direction == OrderByDirection.DESC)
							searchResults = searchResults.ThenByDescending(orderBy.Name);
					}
				}
			}

			results.TotalCount = searchResults.Count();

			if ((results.Page ?? 0) > 0 && (results.PageSize ?? 0) > 0)
			{
				results.Results = searchResults.GetPage(results.Page.Value, results.PageSize.Value);
			}
			else
			{
				results.Results = searchResults;
			}

			return results;
		}
	}
}
