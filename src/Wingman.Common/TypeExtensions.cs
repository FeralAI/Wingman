using System;
using System.Collections.Generic;
using System.Reflection;

namespace Wingman.Common
{
	public static class TypeExtensions
	{
		/// <summary>
		/// A collection of valid types to be considered as numeric.
		/// </summary>
		public static readonly HashSet<Type> NumericTypes = new HashSet<Type>
			{
				typeof(byte),
				typeof(sbyte),

				typeof(short),
				typeof(int),
				typeof(long),

				typeof(ushort),
				typeof(uint),
				typeof(ulong),

				typeof(float),
				typeof(double),
				typeof(decimal),
			};

		/// <summary>
		/// Returns the property names and values from an object, defaulting to all public instance properties.
		/// </summary>
		/// <param name="@this">The type.</param>
		/// <param name="model">The object with the properties and values.</param>
		/// <param name="flags">The <c>BindingFlags</c> to be used in the GetProperties call.</param>
		/// <returns>A dictionary of properties and values.</returns>
		public static Dictionary<string, object> GetPropertyValues(this Type @this, object model, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
		{
			var propertyInfos = @this.GetProperties(flags);
			var result = new Dictionary<string, object>();
			foreach (var propertyInfo in propertyInfos)
			{
				var value = propertyInfo.GetValue(model);
				result.Add(propertyInfo.Name, value);
			}

			return result;
		}

		/// <summary>
		/// Determines whether this type implements the <c>IComparable</c> or <c>IComparable<></c> interfaces.
		/// </summary>
		/// <param name="@this">The type.</param>
		/// <returns>True if one of the interfaces is implemented, otherwise false.</returns>
		public static bool IsComparable(this Type @this)
		{
			var rootType = Nullable.GetUnderlyingType(@this) ?? @this;
			return typeof(IComparable).IsAssignableFrom(rootType) || typeof(IComparable<>).IsAssignableFrom(rootType);
		}

		/// <summary>
		/// Determines whether this type is considered a numeric type.
		/// </summary>
		/// <param name="@this">The type.</param>
		/// <returns>True if the type is part of the <c>NumericTypes</c> set, otherwise false.</returns>
		public static bool IsNumericType(this Type @this)
		{
			return NumericTypes.Contains(@this) || NumericTypes.Contains(Nullable.GetUnderlyingType(@this));
		}
	}
}
