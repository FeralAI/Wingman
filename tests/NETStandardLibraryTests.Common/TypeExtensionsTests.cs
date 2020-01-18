using System;
using System.Collections.Generic;
using NETStandardLibrary.Common;
using Xunit;

namespace NETStandardLibraryTests.Common
{
	public class TypeExtensionsTests
	{
		[Theory]
		[InlineData(typeof(List<int>), false)]
		[InlineData(typeof(bool), true)]
		[InlineData(typeof(DateTime), true)]
		[InlineData(typeof(DateTime?), true)]
		[InlineData(typeof(int), true)]
		[InlineData(typeof(int?), true)]
		[InlineData(typeof(object), false)]
		[InlineData(typeof(string), true)]
		public void IsComparable(Type type, bool expected)
		{
			Assert.Equal(expected, type.IsComparable());
		}

		[Theory]
		[InlineData(typeof(byte?), true)]
		[InlineData(typeof(byte), true)]
		[InlineData(typeof(sbyte), true)]
		[InlineData(typeof(short), true)]
		[InlineData(typeof(ushort), true)]
		[InlineData(typeof(int), true)]
		[InlineData(typeof(uint), true)]
		[InlineData(typeof(long), true)]
		[InlineData(typeof(ulong), true)]
		[InlineData(typeof(float), true)]
		[InlineData(typeof(double), true)]
		[InlineData(typeof(decimal), true)]

		[InlineData(typeof(List<int>), false)]
		[InlineData(typeof(bool?), false)]
		[InlineData(typeof(bool), false)]
		[InlineData(typeof(DateTime), false)]
		[InlineData(typeof(object), false)]
		[InlineData(typeof(string), false)]
		public void IsNumericType(Type type, bool expected)
		{
			Assert.Equal(expected, type.IsNumericType());
		}
	}
}
