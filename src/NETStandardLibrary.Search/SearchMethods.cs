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
			};

			var searchResults = (IOrderedQueryable<T>)queryable;

			if (parameters != null && parameters.Fields != null && parameters.Fields.Count > 0)
			{
				// TODO: https://www.c-sharpcorner.com/UploadFile/c42694/dynamic-query-in-linq-using-predicate-builder/
				// TODO: http://www.albahari.com/nutshell/predicatebuilder.aspx
				var wherePredicate = parameters.Fields
					.Aggregate(PredicateBuilder.New<T>(true), (predicate, field) =>
					{
						var expression = ExpressionMethods.ToWhereExpression<T>(
							field.Name,
							field.Operator,
							field.ValueType,
							field.Value,
							field.MaxValue
						);

						return predicate.And(expression);
					});

				// NOTE: Is the .AsExpandable() really needed here?
				// NOTE: Doesn't seem to hurt, but might only be for SQL Server...
				searchResults = (IOrderedQueryable<T>)searchResults.AsExpandable().Where(wherePredicate);
			}

			if (parameters.OrderBys != null)
				searchResults = searchResults.OrderByClause(new OrderByClauseList(parameters.OrderBys));

			results.TotalCount = searchResults.Count();

			if (results.HasPaging)
				results.Results = searchResults.GetPage(results.Page.Value, results.PageSize.Value);
			else
				results.Results = searchResults;

			return results;
		}
	}
}
