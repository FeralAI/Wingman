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
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>(propertyName, WhereOperator.Equal, valueType, null);
				TestPerson.Data.Where(clause);
			});
		}

		#region ToWhereExpression - IComparable/IComparable<>

		[Theory]
		// Comparable type
		[InlineData("Age", 25, 30, typeof(int), WhereOperator.Between, "James")]
		[InlineData("Age", 25, null, typeof(int), WhereOperator.Equal, "Jackie")]
		[InlineData("Age", 35, null, typeof(int), WhereOperator.GreaterThan, "Mary")]
		[InlineData("Age", 50, null, typeof(int), WhereOperator.GreaterThanOrEqual, "Mary")]
		[InlineData("Age", 25, null, typeof(int), WhereOperator.LessThan, "Bob")]
		[InlineData("Age", 30, null, typeof(int), WhereOperator.LessThanOrEqual, "James")]
		// Comparable type - nullable
		[InlineData("Weight", 200, 300, typeof(int?), WhereOperator.Between, "Keith")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.Equal, "Bob")]
		[InlineData("Weight", 200, null, typeof(int?), WhereOperator.GreaterThan, "Keith")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.GreaterThanOrEqual, "Keith")]
		[InlineData("Weight", 250, null, typeof(int?), WhereOperator.LessThan, "Mary")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.LessThanOrEqual, "Mary")]
		// DateTime
		[InlineData("Date", "2000-06-01", "2000-07-01", typeof(DateTime?), WhereOperator.Between, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereOperator.Equal, "Keith")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereOperator.GreaterThan, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereOperator.GreaterThanOrEqual, "Bob")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereOperator.LessThan, "Steven")]
		[InlineData("Date", "2000-01-01", null, typeof(DateTime?), WhereOperator.LessThanOrEqual, "Steven")]
		public void ToWhereExpression_Comparable(string name, object rawValue, object rawMaxValue, Type valueType, WhereOperator clauseType, string expected)
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
		[InlineData(WhereOperator.Contains)]
		public void ToWhereExpression_Comparable_NotImplementedException(WhereOperator clauseType)
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
		[InlineData(WhereOperator.Equal, 2)]
		public void ToWhereExpression_Object(WhereOperator clauseType, int expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Mother", clauseType, typeof(TestPerson), TestPerson.Mom);
			var results = TestPerson.Data.Where(clause);
			Assert.Equal(expected, results.Count());
		}

		[Theory]
		[InlineData(WhereOperator.Between)]
		[InlineData(WhereOperator.Contains)]
		[InlineData(WhereOperator.GreaterThan)]
		[InlineData(WhereOperator.GreaterThanOrEqual)]
		[InlineData(WhereOperator.LessThan)]
		[InlineData(WhereOperator.LessThanOrEqual)]
		public void ToWhereExpression_Object_NotImplementedException(WhereOperator clauseType)
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
		[InlineData("LastName", "mit", WhereOperator.Contains, "Bob")]
		[InlineData("LastName", "Brown", WhereOperator.Equal, "James")]
		[InlineData("Father.FirstName", "Steven", WhereOperator.Equal, "James")]
		public void ToWhereExpression_String(string name, string value, WhereOperator clauseType, string expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>(name, clauseType, value.GetType(), value);
			var result = TestPerson.Data.Where(clause)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.First();

			Assert.Equal(expected, result.FirstName);
		}

		[Theory]
		[InlineData(WhereOperator.Between)]
		[InlineData(WhereOperator.GreaterThan)]
		[InlineData(WhereOperator.GreaterThanOrEqual)]
		[InlineData(WhereOperator.LessThan)]
		[InlineData(WhereOperator.LessThanOrEqual)]
		public void ToWhereExpression_String_NotImplementedException(WhereOperator clauseType)
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
