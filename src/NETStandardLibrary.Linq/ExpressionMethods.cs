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

		public static Expression<Func<T, bool>> ToWhereExpression<T>(
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

			var lastProperty = (Expression)null;
			var notNullCheck = (Expression)null;

			// We're hijacking the aggregate callbacks to do the work around null checks, original code here was:
			// var property = propertyName.Split('.').Aggregate<string, Expression>(parameter, Expression.PropertyOrField);
			var property = propertyName.Split('.').Aggregate<string, Expression>(parameter, (e, s) =>
			{
				// Get the current property reference and a default value for its type.
				// If it's null, then start building out not null expressions to prepend to the final expression.
				var currentProperty = Expression.PropertyOrField(lastProperty ?? e, s);
				var defaultValue = Expression.Lambda(Expression.Default(currentProperty.Type), "", null).Compile().DynamicInvoke();
				if (defaultValue == null)
				{
					var notNull = Expression.NotEqual(currentProperty, Expression.Convert(Expression.Constant(null), currentProperty.Type));

					// Wanted to do a ternary here, but the if/else is better for debugging
					if (notNullCheck == null)
						notNullCheck = notNull;
					else
						notNullCheck = Expression.AndAlso(notNullCheck, notNull);
				}

				lastProperty = currentProperty;

				// Always return the current property
				return currentProperty;
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

		public static Expression<Func<T, bool>> ToWhereExpression<T, U>(
			string propertyName,
			U value,
			WhereClauseType expressionType)
		{
			return ToWhereExpression<T>(propertyName, value, value.GetType(), expressionType);
		}
	}
}
