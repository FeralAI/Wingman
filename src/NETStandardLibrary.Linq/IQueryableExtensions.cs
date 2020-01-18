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

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(@this, "OrderBy", propertyName, comparer);
		}

		/// <summary>
		/// Parses order by clause using standard SQL-like syntax.
		/// e.g. LastName ASC, FirstName DESC, Age
		///
		/// TODO: https://stackoverflow.com/questions/1689199/c-sharp-code-to-order-by-a-property-using-the-property-name-as-a-string
		///
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

			var orderByClause = orderByClauses.First();
			var propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);
			if (orderByClause.Direction == OrderByDirection.DESC)
				orderedQueryable = orderedQueryable.OrderByDescending(propertyExpression);
			else
				orderedQueryable = orderedQueryable.OrderBy(propertyExpression);

			for (var i = 1; i < orderByClauses.Count; i++)
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

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(@this, "OrderByDescending", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(@this, "ThenBy", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> @this, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(@this, "ThenByDescending", propertyName, comparer);
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
