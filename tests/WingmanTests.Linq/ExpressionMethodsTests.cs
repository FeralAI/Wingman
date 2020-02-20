using System;
using System.Linq;
using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	public class ExpressionMethodsTests
	{
		private static IQueryable<TestPerson> Data = TestPerson.GenerateData();

		[Theory]
		[InlineData(typeof(string), null)]
		[InlineData(typeof(string), "")]
		[InlineData(null, "FirstName")]
		public void ToWhereExpression_ArgumentNullException(Type valueType, string propertyName)
		{
			Assert.ThrowsAny<ArgumentNullException>(() =>
			{
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>(propertyName, WhereOperator.Equal, valueType, null);
				Data.Where(clause);
			});
		}

		#region ToWhereExpression - IComparable/IComparable<>

		[Theory]
		// Comparable type
		[InlineData("Age", 25, 30, typeof(int), WhereOperator.Between, "Norman")]
		[InlineData("Age", 25, null, typeof(int), WhereOperator.Equal, "Horace")]
		[InlineData("Age", 35, null, typeof(int), WhereOperator.GreaterThan, "Miguel")]
		[InlineData("Age", 50, null, typeof(int), WhereOperator.GreaterThanOrEqual, "Earl")]
		[InlineData("Age", 25, null, typeof(int), WhereOperator.LessThan, "Miguel")]
		[InlineData("Age", 30, null, typeof(int), WhereOperator.LessThanOrEqual, "Miguel")]
		[InlineData("Age", 30, null, typeof(int), WhereOperator.NotEqual, "Miguel")]
		// Comparable type - nullable
		[InlineData("Weight", 200, 300, typeof(int?), WhereOperator.Between, "Miguel")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.Equal, "Seth")]
		[InlineData("Weight", 200, null, typeof(int?), WhereOperator.GreaterThan, "Miguel")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.GreaterThanOrEqual, "Miguel")]
		[InlineData("Weight", 250, null, typeof(int?), WhereOperator.LessThan, "Miguel")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.LessThanOrEqual, "Miguel")]
		[InlineData("Weight", 175, null, typeof(int?), WhereOperator.NotEqual, "Miguel")]
		// DateTime
		[InlineData("Updated", "2000-06-01", "2000-07-01", typeof(DateTime?), WhereOperator.Between, "Van")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.Equal, "Alonzo")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.GreaterThan, "Van")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.GreaterThanOrEqual, "Van")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.LessThan, "Margaret")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.LessThanOrEqual, "Margaret")]
		[InlineData("Updated", "2000-01-01", null, typeof(DateTime?), WhereOperator.NotEqual, "Van")]
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
			var result = Data.Where(clause)
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
				Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - IComparable/IComparable<>

		#region ToWhereExpression - object

		[Theory]
		[InlineData(WhereOperator.Equal, "Claudia", "Kihn", 1)]
		[InlineData(WhereOperator.NotEqual, "Claudia", "Kihn", 49)]
		public void ToWhereExpression_Object(WhereOperator clauseType, string firstName, string lastName, int expected)
		{
			var value = Data.Where(p => p.FirstName == firstName).Where(p => p.LastName == lastName).First();
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Friend", clauseType, typeof(TestPerson), value);
			var results = Data.Where(clause);
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
				var clause = ExpressionMethods.ToWhereExpression<TestPerson>("Friend", clauseType, typeof(TestPerson), null);
				Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - object

		#region ToWhereExpression - string

		[Theory]
		[InlineData("LastName", "ittl", WhereOperator.Contains, 2)]
		[InlineData("LastName", "Klein", WhereOperator.Equal, 1)]
		[InlineData("Friend.FirstName", "Claudia", WhereOperator.Equal, 2)]
		[InlineData("LastName", "Smith", WhereOperator.NotEqual, 99)]
		public void ToWhereExpression_String(string name, string value, WhereOperator clauseType, int expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<TestPerson>(name, clauseType, value.GetType(), value);
			var result = Data.Where(clause);

			Assert.Equal(expected, result.Count());
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
				Data.Where(clause);
			});
		}

		#endregion ToWhereExpression - string
	}
}
