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
		[InlineData(null, "Juana")]
		[InlineData("", "Juana")]
		[InlineData(",", "Juana")]
		[InlineData("LastName DESC", "Lucille")]
		[InlineData("Age DESC", "Charlene")]
		[InlineData("Age, LastName", "Miguel")]
		[InlineData("LastName, FirstName DESC", "Miguel")]
		public void OrderByClause_Clauses(string clauseString, string expected)
		{
			var clause = new OrderByClauseList(clauseString);
			var results = TestPerson.GenerateData().OrderByClause(clause);
			Assert.Equal(expected, results.First().FirstName);
		}

		[Theory]
		[InlineData(null, "Juana")]
		[InlineData("", "Juana")]
		[InlineData(",", "Juana")]
		[InlineData("LastName DESC", "Lucille")]
		[InlineData("Age DESC", "Charlene")]
		[InlineData("Age, LastName", "Miguel")]
		[InlineData("LastName, FirstName DESC", "Miguel")]
		public void OrderByClause_String(string clause, string expected)
		{
			var results = TestPerson.GenerateData().OrderByClause(clause);
			Assert.Equal(expected, results.First().FirstName);
		}

		#endregion OrderByClause
	}
}
