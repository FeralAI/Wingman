using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NETStandardLibrary.Linq
{
	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
	public sealed class ExpressionMethods
	{
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
			var property = propertyName
				.Split('.')
				.Aggregate<string, Expression>(parameter, Expression.PropertyOrField);
			var propAsObject = Expression.Convert(property, typeof(object));
			return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
		}

		public static Expression<Func<T, bool>> ToWhereClauseExpression<T>(
			string propertyName,
			object value,
			Type valueType,
			WhereClauseType expressionType)
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("The propertyName must not be null or empty");

			if (valueType == null)
				throw new ArgumentNullException("The valueType must be provided");

			var parameter = Expression.Parameter(typeof(T), "type");
			var constant = Expression.Constant(value, valueType);

			var propertyParts = propertyName.Split('.');
			var property = propertyParts.Aggregate<string, Expression>(parameter, Expression.PropertyOrField);

			// TODO: Maybe move this into a helper function
			var notNull = Expression.NotEqual(property, Expression.Constant(null, property.Type));
			var notNullPredicate = Expression.Lambda<Func<T, bool>>(notNull, parameter);

			if (valueType == typeof(string))
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

		public static Expression<Func<T, bool>> ToWhereClauseExpression<T, U>(
			string propertyName,
			U value,
			WhereClauseType expressionType)
		{
			return ToWhereClauseExpression<T>(propertyName, value, value.GetType(), expressionType);
		}
	}
}
