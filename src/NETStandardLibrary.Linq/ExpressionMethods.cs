using System;
using System.Linq;
using System.Linq.Expressions;
using NETStandardLibrary.Common;

namespace NETStandardLibrary.Linq
{
	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
	public sealed class ExpressionMethods
	{
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
			WhereOperator clauseType,
			Type valueType,
			object value,
			object maxValue = null)
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("The propertyName must not be null or empty");

			if (valueType == null)
				throw new ArgumentNullException("The valueType must be provided");

			var parameter = Expression.Parameter(typeof(T));

			// We're hijacking the aggregate callbacks to do the work around null checks, original code was:
			// var property = propertyName.Split('.').Aggregate<string, Expression>(parameter, Expression.PropertyOrField);
			var notNullCheck = (Expression)null;
			var property = propertyName.Split('.').Aggregate<string, Expression>(parameter, (e, s) =>
			{
				// Get the current property reference and a default value for its type.
				// If it's null, then start building out not null expressions to prepend to the final expression.
				var currentProperty = Expression.PropertyOrField(e, s);
				var defaultValue = Expression.Lambda(Expression.Default(currentProperty.Type), "", null).Compile().DynamicInvoke();
				if (defaultValue == null)
				{
					var notNull = Expression.NotEqual(currentProperty, Expression.Convert(Expression.Constant(null), currentProperty.Type));
					if (notNullCheck == null)
						notNullCheck = notNull;
					else
						notNullCheck = Expression.AndAlso(notNullCheck, notNull);
				}

				// This will be "e" in the next callback, so we pass the property expression for this "s"
				return currentProperty;
			});

			// Create our value expressions
			var constant = Expression.Convert(Expression.Constant(value), property.Type);
			var maxConstant = (Expression)null;
			if (maxValue != null)
				maxConstant = Expression.Convert(Expression.Constant(maxValue), property.Type);

			// Which where?
			var whereExpression = (Expression)null;
			if (valueType == typeof(string))
				whereExpression = BuildWhereExpressionString(clauseType, property, constant);
			else if (valueType.IsNumericType() || valueType.IsComparable())
				whereExpression = BuildWhereExpressionComparable(clauseType, property, constant, maxConstant);
			else
				whereExpression = BuildWhereExpressionObject(clauseType, property, constant);

			// No where there?
			if (notNullCheck != null)
				whereExpression = Expression.AndAlso(notNullCheck, whereExpression);

			return Expression.Lambda<Func<T, bool>>(whereExpression, new ParameterExpression[] { parameter });
		}

		/// <summary>
		/// Builds an expression for IComparables to be used in a .Where call.
		/// </summary>
		/// <param name="clauseType"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		private static Expression BuildWhereExpressionComparable(WhereOperator clauseType, Expression property, Expression value, Expression maxValue = null)
		{
			switch (clauseType)
			{
				case WhereOperator.Between:
					return Expression.AndAlso(Expression.GreaterThanOrEqual(property, value), Expression.LessThanOrEqual(property, maxValue));

				case WhereOperator.Equal:
					return Expression.Equal(property, value);

				case WhereOperator.GreaterThan:
					return Expression.GreaterThan(property, value);

				case WhereOperator.GreaterThanOrEqual:
					return Expression.GreaterThanOrEqual(property, value);

				case WhereOperator.LessThan:
					return Expression.LessThan(property, value);

				case WhereOperator.LessThanOrEqual:
					return Expression.LessThanOrEqual(property, value);

				default:
					throw new NotImplementedException($"WhereClauseType.{clauseType} is not implemented");
			}
		}

		/// <summary>
		/// Builds an expression for an object to be used in a .Where call.
		/// </summary>
		/// <param name="clauseType"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private static Expression BuildWhereExpressionObject(WhereOperator clauseType, Expression property, Expression value)
		{
			// For objects we will only do an equality check
			switch (clauseType)
			{
				case WhereOperator.Equal:
					return Expression.Equal(property, value);

				default:
					throw new NotImplementedException("Objects are limited to Equal expression types only");
			}
		}

		/// <summary>
		/// Builds an expression for a string to be used in a .Where call.
		/// </summary>
		/// <param name="clauseType"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private static Expression BuildWhereExpressionString(WhereOperator clauseType, Expression property, Expression value)
		{
			// If it's a string, limit to only Contains and Equals
			switch (clauseType)
			{
				case WhereOperator.Contains:
					var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
					return Expression.Call(property, method, value);

				case WhereOperator.Equal:
					return Expression.Equal(property, value);

				default:
					throw new NotImplementedException("Strings are limited to Contains and Equal expression types only");
			}
		}
	}
}
