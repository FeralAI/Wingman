using System;
using System.Collections.Generic;
using Wingman.Common;
using Xunit;

namespace WingmanTests.Common
{
	public class TypeExtensionsTests
	{
		[Theory]
		[InlineData("John", "Smith", 45, null)]
		[InlineData("Joe", null, 30, 175)]
		public void GetPropertyValues(string firstName, string lastName, int age, int? weight)
		{
			var person = new TestPerson
			{
				FirstName = firstName,
				LastName = lastName,
				Age = age,
				Weight = weight,
			};

			var results = typeof(TestPerson).GetPropertyValues(person);
			Assert.Equal(firstName, results["FirstName"]);
			Assert.Equal(lastName, results["LastName"]);
			Assert.Equal(age, results["Age"]);
			Assert.Equal(weight, results["Weight"]);
		}

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
