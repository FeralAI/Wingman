using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class IQueryableExtensionsTests
	{
		[Theory]
		[InlineData(-1, -1, "")]
		[InlineData(0, 0, "")]
		[InlineData(1, 1, "this")]
		[InlineData(2, 2, "a test")]
		[InlineData(3, 3, "code")]
		[InlineData(4, 4, "")]
		public void GetPage(int page, int pageSize, string expected)
		{
			var list = "this is a test of the code".Split(' ').AsQueryable();
			var result = string.Join(" ", list.GetPage(page, pageSize));
			Assert.Equal(expected, result);
		}

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
