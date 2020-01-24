using System;
using System.Linq;
using LinqKit;
using NETStandardLibrary.Linq;

namespace NETStandardLibrary.Search
{
	public static class SearchMethods
	{
		public static SearchResults<T> Search<T>(IQueryable<T> queryable, SearchParameters parameters)
		{
			var orderedQueryable = (IOrderedQueryable<T>)queryable;
			var searchResults = new SearchResults<T>
			{
				Parameters = parameters,
				Results = orderedQueryable,
			};

			if (parameters == null)
				return searchResults;

			if (parameters.Fields != null && parameters.Fields.Count > 0)
			{
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
				orderedQueryable = (IOrderedQueryable<T>)orderedQueryable.AsExpandable().Where(wherePredicate);
			}

			if (parameters.OrderBys != null)
				orderedQueryable = orderedQueryable.OrderByClause(new OrderByClauseList(parameters.OrderBys));

			// Make sure we get the count BEFORE applying pagination
			searchResults.TotalCount = orderedQueryable.Count();

			if (searchResults.HasPaging)
				searchResults.Results = orderedQueryable.GetPage(searchResults.Page.Value, searchResults.PageSize.Value);
			else
				searchResults.Results = orderedQueryable;

			return searchResults;
		}
	}
}
