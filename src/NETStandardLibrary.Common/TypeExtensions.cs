using System;
using System.Collections.Generic;

namespace NETStandardLibrary.Common
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
