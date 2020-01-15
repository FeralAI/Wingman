using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NETStandardLibrary.Linq
{
	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
	public sealed class ExpressionMethods
	{
		// TODO: Cleanup?
		// public static Expression<Func<Type, object>> ToPropertyExpression(Type type, string propertyName)
		// {
		// 	var parameter = Expression.Parameter(type);
		// 	var property = Expression.Property(parameter, propertyName);
		// 	var propAsObject = Expression.Convert(property, typeof(object));
		// 	return Expression.Lambda<Func<Type, object>>(propAsObject, parameter);
		// }

		public static MethodCallExpression ToMethodCallExpression<T>(
			IQueryable<T> query,
			string propertyName,
			string methodName,
			IComparer<object> comparer = null)
		{
			var parameter = Expression.Parameter(typeof(T), "x");
			var body = propertyName
				.Split('.')
				.Aggregate<string, Expression>(parameter, Expression.PropertyOrField);

			return comparer != null
				? Expression.Call(
						typeof(Queryable),
						methodName,
						new[] { typeof(T), body.Type },
						query.Expression,
						Expression.Lambda(body, parameter),
						Expression.Constant(comparer)
					)
				: Expression.Call(
						typeof(Queryable),
						methodName,
						new[] { typeof(T), body.Type },
						query.Expression,
						Expression.Lambda(body, parameter)
					);
		}

		public static Expression<Func<T, object>> ToPropertyExpression<T>(string propertyName)
		{
			var parameter = Expression.Parameter(typeof(T));
			var property = Expression.Property(parameter, propertyName);
			var propAsObject = Expression.Convert(property, typeof(object));
			return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
		}

		public static Expression<Func<T, bool>> ToWhereClauseExpression<T, U>(
			string propertyName,
			U value,
			WhereClauseType expressionType)
		{
			var parameter = Expression.Parameter(typeof(T), "type");
			var property = Expression.Property(parameter, propertyName);
			var constant = Expression.Constant(value, typeof(U));

			if (typeof(U) == typeof(string))
			{
				// If it's a string, limit to only Contains and Equals
				switch (expressionType)
				{
					case WhereClauseType.Contains:
						var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
						var containsMethodCall = Expression.Call(property, method, constant);
						return Expression.Lambda<Func<T, bool>>(containsMethodCall, new ParameterExpression[] { parameter });
					case WhereClauseType.Equal:
						var equalExpression = Expression.Equal(property, constant);
						return Expression.Lambda<Func<T, bool>>(equalExpression, new ParameterExpression[] { parameter });
					default:
						throw new NotImplementedException("Strings are limited to Contains and Equals expression types only");
				}
			}
			else
			{
				switch (expressionType)
				{
					case WhereClauseType.Equal:
						var equalExpression = Expression.Equal(property, constant);
						return Expression.Lambda<Func<T, bool>>(equalExpression, new ParameterExpression[] { parameter });
					case WhereClauseType.GreaterThan:
						var greaterThanExpression = Expression.GreaterThan(property, constant);
						return Expression.Lambda<Func<T, bool>>(greaterThanExpression, new ParameterExpression[] { parameter });
					case WhereClauseType.GreaterThanOrEqual:
						var greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(property, constant);
						return Expression.Lambda<Func<T, bool>>(greaterThanOrEqualExpression, new ParameterExpression[] { parameter });
					case WhereClauseType.LessThan:
						var lessThanExpression = Expression.LessThan(property, constant);
						return Expression.Lambda<Func<T, bool>>(lessThanExpression, new ParameterExpression[] { parameter });
					case WhereClauseType.LessThanOrEqual:
						var lessThanOrEqualExpression = Expression.LessThanOrEqual(property, constant);
						return Expression.Lambda<Func<T, bool>>(lessThanOrEqualExpression, new ParameterExpression[] { parameter });
					default:
						throw new NotImplementedException($"The expression type {expressionType} is not implemented");
				}
			}
		}
	}
}
