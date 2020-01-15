using System.Collections.Generic;
using System.Linq;
using NETStandardLibrary.Linq;
using NETStandardLibrary.Tests.Data;
using Xunit;

namespace NETStandardLibrary.Search.Tests
{
	public class SearchMethodsTests
	{
		[Fact]
		public void Search()
		{
			var searchParameters = new SearchParameters
			{
				Fields = new List<SearchField>
				{
					new SearchField
					{
						Name = "FirstName",
						Value = "Bob",
						ValueType = typeof(string),
						Operator = WhereClauseType.Equal,
					},
				},
				OrderBys = new List<OrderByClause>
				{
					new OrderByClause
					{
						Name = "LastName",
						Direction = OrderByDirection.ASC,
					},
				},
				Page = 1,
				PageSize = 3,
			};

			var results = SearchMethods.Search<Person>(Person.Data, searchParameters);
			Assert.NotNull(results);
			Assert.Equal(1, results.Page);
			Assert.Equal(3, results.PageSize);
			Assert.Equal(2, results.TotalCount);
			Assert.Equal(2, results.Results.Count());
			Assert.Equal(1, results.TotalPages);
			Assert.Equal("Bob", results.Results.First().FirstName);
		}

		[Fact]
		public void SearchDeep()
		{
			var searchParameters = new SearchParameters
			{
				Fields = new List<SearchField>
				{
					new SearchField
					{
						Name = "Mother.FirstName",
						Value = "Mary",
						ValueType = typeof(string),
						Operator = WhereClauseType.Equal,
					},
				},
				OrderBys = new List<OrderByClause>
				{
					new OrderByClause
					{
						Name = "LastName",
						Direction = OrderByDirection.ASC,
					},
				},
				Page = 1,
				PageSize = 3,
			};

			var results = SearchMethods.Search<Person>(Person.Data, searchParameters);
			Assert.NotNull(results);
			Assert.Equal(1, results.Page);
			Assert.Equal(3, results.PageSize);
			Assert.Equal(2, results.TotalCount);
			Assert.Equal(2, results.Results.Count());
			Assert.Equal(1, results.TotalPages);
			Assert.Equal("Brown", results.Results.First().LastName);
		}
	}
}
