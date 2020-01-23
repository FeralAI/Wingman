using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibrary.Linq
{
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Retuns a page of data using <c>IQueryable.Skip</c> and <c> IQueryable.Take</c>.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static IQueryable<T> GetPage<T>(this IQueryable<T> @this, int page, int pageSize)
		{
			return @this
				.Skip((pageSize * (Math.Max(page, 1) - 1)))
				.Take(pageSize);
		}

		/// <summary>
		/// Parses order by clause using standard SQL-like syntax.
		/// e.g. LastName ASC, FirstName DESC, Age
		///
		/// TODO: https://stackoverflow.com/questions/1689199/c-sharp-code-to-order-by-a-property-using-the-property-name-as-a-string
		/// TODO: This only works for static types. Need to implement this for dynamic types as well.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		public static IOrderedQueryable<T> OrderByClause<T>(this IQueryable<T> @this, string originalClause)
		{
			var orderedQueryable = (IOrderedQueryable<T>)@this;

			if (string.IsNullOrWhiteSpace(originalClause))
				return orderedQueryable;

			var orderByClauses = Parser.ParseOrderBys(originalClause);
			if (orderByClauses == null || orderByClauses.Count == 0)
				return orderedQueryable;

			// Pluck out the first clause so we can start with an .OrderBy() call
			var i = 0;
			var orderByClause = orderByClauses[i];
			var propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);
			if (orderByClause.Direction == OrderByDirection.DESC)
				orderedQueryable = orderedQueryable.OrderByDescending(propertyExpression);
			else
				orderedQueryable = orderedQueryable.OrderBy(propertyExpression);

			// Loop over the remaining clauses and append .ThenBy() calls
			for (i = 1; i < orderByClauses.Count; i++)
			{
				orderByClause = orderByClauses[i];
				propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);
				if (orderByClause.Direction == OrderByDirection.DESC)
					orderedQueryable = orderedQueryable.ThenByDescending(propertyExpression);
				else
					orderedQueryable = orderedQueryable.ThenBy(propertyExpression);
			}

			return orderedQueryable;
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return @this.CallOrderedQueryable("OrderBy", propertyName, comparer);
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return @this.CallOrderedQueryable("OrderByDescending", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return @this.CallOrderedQueryable("ThenBy", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return @this.CallOrderedQueryable("ThenByDescending", propertyName, comparer);
		}

		/// <summary>
		/// Builds the Queryable functions using a TSource property name.
		/// </summary>
		private static IOrderedQueryable<T> CallOrderedQueryable<T>(
			this IQueryable<T> @this,
			string methodName,
			string propertyName,
			IComparer<object> comparer = null)
		{
			var methodCallExpression = ExpressionMethods.ToMethodCallExpression(@this, propertyName, methodName, comparer);
			return (IOrderedQueryable<T>)@this.Provider.CreateQuery(methodCallExpression);
		}
	}
}
