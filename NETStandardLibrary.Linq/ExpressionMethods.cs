using System;
using System.Linq.Expressions;

namespace NETStandardLibrary.Linq
{
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

		public static Expression<Func<T, object>> ToPropertyExpression<T>(string propertyName)
		{
			var parameter = Expression.Parameter(typeof(T));
			var property = Expression.Property(parameter, propertyName);
			var propAsObject = Expression.Convert(property, typeof(object));
			return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
		}
	}
}
