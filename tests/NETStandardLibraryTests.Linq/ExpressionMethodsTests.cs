using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ExpressionMethodsTests
	{
		#region int
		[Fact]
		public void ToWhereClauseExpressionIntEqual()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, int>("Age", 25, WhereClauseType.Equal);
			var result = Person.Data.Where(clause).First();
			Assert.Equal("Jackie", result.FirstName);
		}

		[Fact]
		public void ToWhereClauseExpressionIntLessThan()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, int>("Age", 25, WhereClauseType.LessThan);
			var result = Person.Data.Where(clause).First();
			Assert.Equal("Bob", result.FirstName);
		}

		[Fact]
		public void ToWhereClauseExpressionIntLessThanOrEqual()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, int>("Age", 25, WhereClauseType.LessThanOrEqual);
			var result = Person.Data.Where(clause);
			Assert.Equal(3, result.Count());
		}

		[Fact]
		public void ToWhereClauseExpressionIntGreaterThan()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, int>("Age", 20, WhereClauseType.GreaterThan);
			var result = Person.Data.Where(clause);
			Assert.Equal(6, result.Count());
		}

		[Fact]
		public void ToWhereClauseExpressionIntGreaterThanOrEqual()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, int>("Age", 20, WhereClauseType.GreaterThanOrEqual);
			var result = Person.Data.Where(clause);
			Assert.Equal(7, result.Count());
		}
		#endregion

		#region string
		[Fact]
		public void ToWhereClauseExpressionStringContains()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, string>("LastName", "mit", WhereClauseType.Contains);
			var result = Person.Data.Where(clause).First();
			Assert.Equal("Bob", result.FirstName);
		}

		[Fact]
		public void ToWhereClauseExpressionStringEqual()
		{
			var clause = ExpressionMethods.ToWhereClauseExpression<Person, string>("FirstName", "Bob", WhereClauseType.Equal);
			var result = Person.Data.Where(clause);
			Assert.Equal(2, result.Count());
		}
		#endregion
	}
}
