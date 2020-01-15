using System.Collections.Generic;
using Xunit;

namespace NETStandardLibrary.Linq.Tests
{
	public class ParserTests
	{
		[Fact]
		public void OrderByClauses()
		{
			var source = "LastName, FirstName DESC, Age ASC";
			var result = Parser.ParseOrderBys(source);
			var expected = new List<OrderByClause>
			{
				new OrderByClause { Name = "LastName", Direction = OrderByDirection.ASC },
				new OrderByClause { Name = "FirstName", Direction = OrderByDirection.DESC },
				new OrderByClause { Name = "Age", Direction = OrderByDirection.ASC },
			};

			Assert.NotStrictEqual(expected, result);
		}
	}
}
