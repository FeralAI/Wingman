using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NETStandardLibrary.Linq
{
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Builds the Queryable functions using a TSource property name.
		/// </summary>
		public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName,
						IComparer<object> comparer = null)
		{
				var param = Expression.Parameter(typeof(T), "x");
				var body = propertyName
					.Split('.')
					.Aggregate<string, Expression>(param, Expression.PropertyOrField);

				return comparer != null
					? (IOrderedQueryable<T>)query.Provider.CreateQuery(
						Expression.Call(
							typeof(Queryable),
							methodName,
							new[] { typeof(T), body.Type },
							query.Expression,
							Expression.Lambda(body, param),
							Expression.Constant(comparer)
						)
					)
					: (IOrderedQueryable<T>)query.Provider.CreateQuery(
						Expression.Call(
							typeof(Queryable),
							methodName,
							new[] { typeof(T), body.Type },
							query.Expression,
							Expression.Lambda(body, param)
						)
					);
		}

		/// <summary>
		/// Retuns a page of data using <c>IQueryable.Skip</c> and <c> IQueryable.Take</c>.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static IQueryable<T> GetPage<T>(this IQueryable<T> queryable, int page, int pageSize)
		{
			return queryable
				.Skip((pageSize * (Math.Max(page, 1) - 1)))
				.Take(pageSize);
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
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
		public static IOrderedQueryable<T> OrderByClause<T>(this IQueryable<T> queryable, string originalClause)
		{
			var orderedQueryable = (IOrderedQueryable<T>)queryable;

			if (string.IsNullOrWhiteSpace(originalClause))
				return orderedQueryable;

			var orderByClauses = Parser.ParseOrderBys(originalClause);
			if (orderByClauses == null || orderByClauses.Count == 0)
				return orderedQueryable;

			var first = true;
			foreach (var orderByClause in orderByClauses)
			{
				// TODO: One way to do it...
				// var propertyExpression = ExpressionMethods.ToPropertyExpression<T>(orderByClause.Name);

				// if (first)
				// {
				// 	if (orderByClause.Direction == OrderByDirection.DESC)
				// 		orderedQueryable = orderedQueryable.OrderByDescending(propertyExpression);
				// 	else
				// 		orderedQueryable = orderedQueryable.OrderBy(propertyExpression);
				// }
				// else
				// {
				// 	if (orderByClause.Direction == OrderByDirection.DESC)
				// 		orderedQueryable = orderedQueryable.ThenByDescending(propertyExpression);
				// 	else
				// 		orderedQueryable = orderedQueryable.ThenBy(propertyExpression);
				// }

				// TODO: Another way to do it
				if (first)
				{
					if (orderByClause.Direction == OrderByDirection.DESC)
						orderedQueryable = orderedQueryable.OrderByDescending(orderByClause.Name);
					else
						orderedQueryable = orderedQueryable.OrderBy(orderByClause.Name);
				}
				else
				{
					if (orderByClause.Direction == OrderByDirection.DESC)
						orderedQueryable = orderedQueryable.ThenByDescending(orderByClause.Name);
					else
						orderedQueryable = orderedQueryable.ThenBy(orderByClause.Name);
				}

				first = false;
			}

			return orderedQueryable;
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
		{
			return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
		}
	}
}
