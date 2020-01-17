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
		[InlineData(typeof(string), true)]
		public void IsComparable(Type type, bool expected)
		{
			Assert.Equal(expected, type.IsComparable());
		}

		[Theory]
		[InlineData(typeof(List<int>), false)]
		[InlineData(typeof(bool), false)]
		[InlineData(typeof(DateTime), false)]
		[InlineData(typeof(DateTime?), false)]
		[InlineData(typeof(int), true)]
		[InlineData(typeof(int?), true)]
		[InlineData(typeof(string), false)]
		public void IsNumericType(Type type, bool expected)
		{
			Assert.Equal(expected, type.IsNumericType());
		}
	}
}