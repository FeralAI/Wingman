using System.Linq;
using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	public class IQueryableExtensionsTests
	{
		#region OrderByClause

		// The test data for the methods in this region should stay in sync to
		// test all possible logic routes.

		[Theory]
		[InlineData(null, "James")]
		[InlineData("", "James")]
		[InlineData(",", "James")]
		[InlineData("LastName DESC", "Bob")]
		[InlineData("Age DESC", "Mary")]
		[InlineData("Age, LastName", "Chris")]
		[InlineData("LastName, FirstName DESC", "James")]
		public void OrderByClause_Clauses(string clauseString, string expected)
		{
			var clause = new OrderByClauseList(clauseString);
			var results = TestPerson.Data.OrderByClause(clause);
			Assert.Equal(expected, results.First().FirstName);
		}

		[Theory]
		[InlineData(null, "James")]
		[InlineData("", "James")]
		[InlineData(",", "James")]
		[InlineData("LastName DESC", "Bob")]
		[InlineData("Age DESC", "Mary")]
		[InlineData("Age, LastName", "Chris")]
		[InlineData("LastName, FirstName DESC", "James")]
		public void OrderByClause_String(string clause, string expected)
		{
			var results = TestPerson.Data.OrderByClause(clause);
			Assert.Equal(expected, results.First().FirstName);
		}

		#endregion OrderByClause
	}
}
