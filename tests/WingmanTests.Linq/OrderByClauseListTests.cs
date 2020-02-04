using System;
using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	public class OrderByClauseListTests
	{
		[Fact]
		public void Constructor()
		{
			var orderByList = new OrderByClauseList
			{
				new OrderByClause { Direction = OrderByDirection.ASC, Name = "FirstName" }
			};
			Assert.Single(orderByList);
		}

		[Fact]
		public void Constructor_Clause()
		{
			var orderByList = new OrderByClauseList("FirstName ASC, LastName DESC, Age");
			Assert.Equal(3, orderByList.Count);
		}

		[Fact]
		public void Constructor_IEnumerable()
		{
			var list = OrderByClauseList.Parse("FirstName ASC, LastName DESC, Age");
			var orderByList = new OrderByClauseList(list);
			Assert.Equal(3, orderByList.Count);
		}

		[Theory]
		[InlineData("FirstName", 1)]
		[InlineData("FirstName, LastName DESC", 2)]
		[InlineData("FirstName DESC, LastName ASC", 2)]
		public void AddByClause(string clause, int expected)
		{
			var list = new OrderByClauseList();
			list.AddByClause(clause);
			Assert.Equal(expected, list.Count);
		}

		[Fact]
		public void Parse_ArgumentException()
		{
			Assert.ThrowsAny<ArgumentException>(() => OrderByClauseList.Parse("First Name ASC"));
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", null)]
		[InlineData("FirstName", "FirstName ASC")]
		[InlineData("FirstName ASC", "FirstName ASC")]
		[InlineData("FirstName ASC, LastName DESC, Age", "FirstName ASC, LastName DESC, Age ASC")]
		public void ParseAndToString(string clause, string expected)
		{
			var orderBys = new OrderByClauseList();
			if (!string.IsNullOrWhiteSpace(clause))
				orderBys = new OrderByClauseList(clause);

			var result = orderBys.ToString();
			Assert.Equal(expected, result);
		}
	}
}
