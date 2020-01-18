using System;
using System.Collections.Generic;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class ParserTests
	{
		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("FirstName", "FirstName ASC")]
		[InlineData("FirstName ASC", "FirstName ASC")]
		[InlineData("FirstName ASC, LastName DESC, Age", "FirstName ASC, LastName DESC, Age ASC")]
		public void JoinOrderBys_ParseOrderBys(string clause, string expected)
		{
			var orderBys = (List<OrderByClause>)null;
			if (clause == "")
				orderBys = new List<OrderByClause>();
			else if (clause != null)
				orderBys = Parser.ParseOrderBys(clause);

			var result = Parser.JoinOrderBys(orderBys);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(",,,")]
		[InlineData("First Name ASC")]
		public void ParserOrderBys_ArgumentException(string clause)
		{
			Assert.ThrowsAny<ArgumentException>(() => Parser.ParseOrderBys(clause));
		}
	}
}
