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

			var lastNestedProperty = (Expression)null;
			var notNullCheck = (Expression)null;

			var propertyParts = propertyName.Split('.');
			var property =  propertyParts.Aggregate<string, Expression>(parameter, (e, s) =>
			{
				var nestedProperty = Expression.PropertyOrField(lastNestedProperty ?? e, s);
				var defaultValue = Expression.Lambda(Expression.Default(nestedProperty.Type), "", null).Compile().DynamicInvoke();
				if (defaultValue == null)
				{
					var notNull = Expression.NotEqual(nestedProperty, Expression.Convert(Expression.Constant(null), nestedProperty.Type));
					if (notNullCheck == null)
						notNullCheck = notNull;
					else
						notNullCheck = Expression.AndAlso(notNullCheck, notNull);
				}

				lastNestedProperty = nestedProperty;
				return nestedProperty;
			});

			var constant = Expression.Convert(Expression.Constant(value), property.Type);

			var whereExpression = (Expression)null;
			if (valueType == typeof(string))
			{
				// If it's a string, limit to only Contains and Equals
				switch (expressionType)
				{
					case WhereClauseType.Contains:
						var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
						whereExpression = Expression.Call(property, method, constant);
						break;

					case WhereClauseType.Equal:
						whereExpression = Expression.Equal(property, constant);
						break;

					default:
						throw new NotImplementedException("Strings are limited to Contains and Equals expression types only");
				}
			}
			else
			{
				switch (expressionType)
				{
					case WhereClauseType.Equal:
						whereExpression = Expression.Equal(property, constant);
						break;

					case WhereClauseType.GreaterThan:
						whereExpression = Expression.GreaterThan(property, constant);
						break;

					case WhereClauseType.GreaterThanOrEqual:
						whereExpression = Expression.GreaterThanOrEqual(property, constant);
						break;

					case WhereClauseType.LessThan:
						whereExpression = Expression.LessThan(property, constant);
						break;

					case WhereClauseType.LessThanOrEqual:
						whereExpression = Expression.LessThanOrEqual(property, constant);
						break;

					default:
						throw new NotImplementedException($"The expression type {expressionType} is not implemented");
				}
			}

			if (notNullCheck != null)
			{
				whereExpression = Expression.AndAlso(notNullCheck, whereExpression);
			}

			return Expression.Lambda<Func<T, bool>>(whereExpression, new ParameterExpression[] { parameter });
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
