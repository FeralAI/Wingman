using System;
using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ExpressionMethodsTests
	{
		[Theory]
		// Comparable type
		[InlineData("Age", 25, 30, typeof(int), WhereClauseType.Between, "James")]
		[InlineData("Age", 25, null, typeof(int), WhereClauseType.Equal, "Jackie")]
		[InlineData("Age", 35, null, typeof(int), WhereClauseType.GreaterThan, "Mary")]
		[InlineData("Age", 50, null, typeof(int), WhereClauseType.GreaterThanOrEqual, "Mary")]
		[InlineData("Age", 25, null, typeof(int), WhereClauseType.LessThan, "Bob")]
		[InlineData("Age", 30, null, typeof(int), WhereClauseType.LessThanOrEqual, "James")]
		// Comparable type - nullable
		[InlineData("Weight", 200, 300, typeof(int?), WhereClauseType.Between, "Keith")]
		[InlineData("Weight", 175, null, typeof(int?), WhereClauseType.Equal, "Bob")]
		[InlineData("Weight", 200, null, typeof(int?), WhereClauseType.GreaterThan, "Keith")]
		[InlineData("Weight", 175, null, typeof(int?), WhereClauseType.GreaterThanOrEqual, "Keith")]
		[InlineData("Weight", 250, null, typeof(int?), WhereClauseType.LessThan, "Mary")]
		[InlineData("Weight", 175, null, typeof(int?), WhereClauseType.LessThanOrEqual, "Mary")]
		// DateTime
		[InlineData("Date", "2000-06-01", "2000-07-01", typeof(DateTime?), WhereClauseType.Between, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereClauseType.Equal, "Keith")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereClauseType.GreaterThan, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereClauseType.GreaterThanOrEqual, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereClauseType.LessThan, "Steven")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereClauseType.LessThanOrEqual, "Steven")]
		// Strings
		[InlineData("LastName", "mit", null, typeof(string), WhereClauseType.Contains, "Bob")]
		[InlineData("LastName", "Brown", null, typeof(string), WhereClauseType.Equal, "James")]
		public void ToWhereExpression(string name, object rawValue, object rawMaxValue, Type valueType, WhereClauseType clauseType, string expected)
		{
			var value = rawValue;
			var maxValue = rawMaxValue;
			if (valueType == typeof(DateTime) || Nullable.GetUnderlyingType(valueType) == typeof(DateTime))
			{
				if (rawValue != null)
					value = DateTime.Parse(rawValue as string);
				if (rawMaxValue != null)
					maxValue = DateTime.Parse(rawMaxValue as string);
			}

			var clause = ExpressionMethods.ToWhereExpression<Person>(name, clauseType, valueType, value, maxValue);
			var result = Person.Data.Where(clause)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.First();

			Assert.Equal(expected, result.FirstName);
		}
	}
}
