using System;
using System.Linq;

namespace Wingman.Linq
{
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Applies order by logic to an <c>IQueryable&lt;&gt;</c> object.
		/// </summary>
		/// <param name="this">The queryable object.</param>
		/// <param name="orderBys">The order by clauses.</param>
		/// <returns>An <c>IOrderedQueryable</c> object.</returns>
		public static IOrderedQueryable<T> OrderByClause<T>(this IQueryable<T> @this, OrderByClauseList orderBys)
		{
			var orderedQueryable = (IOrderedQueryable<T>)@this;
			if (orderBys == null || orderBys.Count == 0)
				return orderedQueryable;

			// Pluck out the first clause so we can start with an .OrderBy() call
			var i = 0;
			var orderByClause = orderBys[i];
			var propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);
			if (orderByClause.Direction == OrderByDirection.DESC)
				orderedQueryable = orderedQueryable.OrderByDescending(propertyExpression);
			else
				orderedQueryable = orderedQueryable.OrderBy(propertyExpression);

			// Loop over the remaining clauses and append .ThenBy() calls
			for (i = 1; i < orderBys.Count; i++)
			{
				orderByClause = orderBys[i];
				propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);
				if (orderByClause.Direction == OrderByDirection.DESC)
					orderedQueryable = orderedQueryable.ThenByDescending(propertyExpression);
				else
					orderedQueryable = orderedQueryable.ThenBy(propertyExpression);
			}

			return orderedQueryable;
		}

		/// <summary>
		/// Applies order by logic to an <c>IQueryable&lt;&gt;</c> object using standard SQL-like syntax.
		/// </summary>
		/// <param name="this">The queryable object.</param>
		/// <param name="orderBy">The order by clause string.</param>
		/// <returns>An <c>IOrderedQueryable</c> object.</returns>
		public static IOrderedQueryable<T> OrderByClause<T>(this IQueryable<T> @this, string orderBy)
		{
			if (string.IsNullOrWhiteSpace(orderBy))
				return (IOrderedQueryable<T>)@this;

			var orderByClauses = new OrderByClauseList(orderBy);
			return @this.OrderByClause(orderByClauses);
		}

		public static SearchResults<T> Search<T>(this IQueryable<T> @this, SearchParameters parameters)
		{
			return SearchMethods.Search<T>(@this, parameters);
		}
	}
}
