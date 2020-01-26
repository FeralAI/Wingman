using System;
using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ExpressionMethodsTests
	{
		[Theory]
		[InlineData(typeof(string), null)]
		[InlineData(typeof(string), "")]
		[InlineData(null, "FirstName")]
		public void ToWhereExpression_ArgumentNullException(Type valueType, string propertyName)
		{
			Assert.ThrowsAny<ArgumentNullException>(() =>
			{
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>(propertyName, WhereClauseType.Equal, valueType, null);
				TestPerson.Data.Where(clause);
			});
		}

		#region ToWhereExpression - IComparable/IComparable<>

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
		public void ToWhereExpression_Comparable(string name, object rawValue, object rawMaxValue, Type valueType, WhereClauseType clauseType, string expected)
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

			var clause = ExpressionMethods.ToWhereExpression<TestPerson>(name, clauseType, valueType, value, maxValue);
			var result = TestPerson.Data.Where(clause)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.First();

			Assert.Equal(expected, result.FirstName);
		}

		[Theory]
		[InlineData(WhereClauseType.Contains)]
		public void ToWhereExpression_Comparable_NotImplementedException(WhereClauseType clauseType)
		{
			Assert.ThrowsAny<NotImplementedException>(() =>
			{
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Weight", clauseType, typeof(int?), null);
				TestPerson.Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - IComparable/IComparable<>

		#region ToWhereExpression - object

		[Theory]
		[InlineData(WhereClauseType.Equal, 2)]
		public void ToWhereExpression_Object(WhereClauseType clauseType, int expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Mother", clauseType, typeof(TestPerson), TestPerson.Mom);
			var results = TestPerson.Data.Where(clause);
			Assert.Equal(expected, results.Count());
		}

		[Theory]
		[InlineData(WhereClauseType.Between)]
		[InlineData(WhereClauseType.Contains)]
		[InlineData(WhereClauseType.GreaterThan)]
		[InlineData(WhereClauseType.GreaterThanOrEqual)]
		[InlineData(WhereClauseType.LessThan)]
		[InlineData(WhereClauseType.LessThanOrEqual)]
		public void ToWhereExpression_Object_NotImplementedException(WhereClauseType clauseType)
		{
			Assert.ThrowsAny<NotImplementedException>(() =>
			{
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Mother", clauseType, typeof(TestPerson), null);
				TestPerson.Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - object

		#region ToWhereExpression - string

		[Theory]
		[InlineData("LastName", "mit", WhereClauseType.Contains, "Bob")]
		[InlineData("LastName", "Brown", WhereClauseType.Equal, "James")]
		[InlineData("Father.FirstName", "Steven", WhereClauseType.Equal, "James")]
		public void ToWhereExpression_String(string name, string value, WhereClauseType clauseType, string expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>(name, clauseType, value.GetType(), value);
			var result = TestPerson.Data.Where(clause)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.First();

			Assert.Equal(expected, result.FirstName);
		}

		[Theory]
		[InlineData(WhereClauseType.Between)]
		[InlineData(WhereClauseType.GreaterThan)]
		[InlineData(WhereClauseType.GreaterThanOrEqual)]
		[InlineData(WhereClauseType.LessThan)]
		[InlineData(WhereClauseType.LessThanOrEqual)]
		public void ToWhereExpression_String_NotImplementedException(WhereClauseType clauseType)
		{
			Assert.ThrowsAny<NotImplementedException>(() =>
			{
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>("LastName", clauseType, typeof(string), null);
				TestPerson.Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - string
	}
}
