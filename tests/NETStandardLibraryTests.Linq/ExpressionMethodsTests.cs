using System;
using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ExpressionMethodsTests
	{
		[Theory]
		[InlineData("Age", 25, typeof(int), WhereClauseType.Equal, "Jackie")]
		[InlineData("Age", 35, typeof(int), WhereClauseType.GreaterThan, "Mary")]
		[InlineData("Age", 50, typeof(int), WhereClauseType.GreaterThanOrEqual, "Mary")]
		[InlineData("Age", 25, typeof(int), WhereClauseType.LessThan, "Bob")]
		[InlineData("Age", 30, typeof(int), WhereClauseType.LessThanOrEqual, "James")]
		[InlineData("Weight", 175, typeof(int?), WhereClauseType.Equal, "Bob")]
		[InlineData("Weight", 200, typeof(int?), WhereClauseType.GreaterThan, "Keith")]
		[InlineData("Weight", 175, typeof(int?), WhereClauseType.GreaterThanOrEqual, "Keith")]
		[InlineData("Weight", 250, typeof(int?), WhereClauseType.LessThan, "Mary")]
		[InlineData("Weight", 175, typeof(int?), WhereClauseType.LessThanOrEqual, "Mary")]
		[InlineData("LastName", "mit", typeof(string), WhereClauseType.Contains, "Bob")]
		[InlineData("LastName", "Brown", typeof(string), WhereClauseType.Equal, "James")]
		public void ToWhereExpression(string name, object value, Type valueType, WhereClauseType clauseType, string expected)
		{
			var clause = ExpressionMethods.ToWhereExpression<Person>(name, value, valueType, clauseType);
			var result = Person.Data.Where(clause)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.First();

			Assert.Equal(expected, result.FirstName);
		}
	}
}
