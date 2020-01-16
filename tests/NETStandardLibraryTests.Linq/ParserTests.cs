using System.Collections.Generic;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ParserTests
	{
		[Fact]
		public void ParseOrderBys()
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
