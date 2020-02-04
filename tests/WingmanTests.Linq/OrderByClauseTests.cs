using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	 public class OrderByClauseTests
	 {
		 [Theory]
		 [InlineData("FirstName", OrderByDirection.ASC, "FirstName ASC")]
		 [InlineData("LastName", OrderByDirection.DESC, "LastName DESC")]
		 public void TestName(string name, OrderByDirection direction, string expected)
		 {
			 var clause = new OrderByClause
			 {
				 Name = name,
				 Direction = direction,
			 };
			 Assert.Equal(expected, clause.ToString());
		 }
	 }
}
