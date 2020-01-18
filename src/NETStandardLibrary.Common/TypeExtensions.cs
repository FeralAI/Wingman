using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibrary.Common
{
	public static class TypeExtensions
	{
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

		public static bool IsComparable(this Type @this)
		{
			var rootType = Nullable.GetUnderlyingType(@this) ?? @this;
			return typeof(IComparable).IsAssignableFrom(rootType) || typeof(IComparable<>).IsAssignableFrom(rootType);
		}

		public static bool IsNumericType(this Type @this)
		{
			return NumericTypes.Contains(@this) || NumericTypes.Contains(Nullable.GetUnderlyingType(@this));
		}
	}
}
