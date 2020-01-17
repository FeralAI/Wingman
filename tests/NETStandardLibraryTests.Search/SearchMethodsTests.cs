using System;
using System.Collections.Generic;
using System.Linq;
using NETStandardLibrary.Linq;
using NETStandardLibrary.Search;
using Xunit;

namespace NETStandardLibraryTests.Search
{
	public class SearchMethodsTests
	{
		[Theory]
		[InlineData("FirstName", "Steven", typeof(string), WhereClauseType.Equal, "Jackson")]
		[InlineData("Weight", 200, typeof(int?), WhereClauseType.GreaterThan, "Myers")]
		[InlineData("Age", 21, typeof(int), WhereClauseType.LessThanOrEqual, "Smith")]
		[InlineData("Mother.FirstName", "Mary", typeof(string), WhereClauseType.Equal, "Brown")]
		public void Search(string name, object value, Type valueType, WhereClauseType clauseType, string expected)
		{
			var searchParameters = new SearchParameters
			{
				Fields = new List<SearchField>
				{
					new SearchField
					{
						Name = name,
						Value = value,
						ValueType = valueType,
						Operator = clauseType,
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
			Assert.Equal(expected, results.Results.First().LastName);
		}
	}
}
