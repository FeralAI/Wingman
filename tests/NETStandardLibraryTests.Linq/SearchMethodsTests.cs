using System;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class SearchMethodsTests
	{
		[Theory]
		[InlineData("FirstName", "Steven", null, typeof(string), WhereClauseType.Equal, 1)]
		[InlineData("Weight", 200, null, typeof(int?), WhereClauseType.GreaterThan, 2)]
		[InlineData("Age", 21, null, typeof(int), WhereClauseType.LessThanOrEqual, 2)]
		[InlineData("Mother.FirstName", "Mary", null, typeof(string), WhereClauseType.Equal, 2)]
		public void Search(string name, object value, object maxValue, Type valueType, WhereClauseType clauseType, int expected)
		{
			var searchParameters = new SearchParameters
			{
				Fields = new SearchFieldList
				{
					new SearchField
					{
						Name = name,
						Value = value,
						MaxValue = maxValue,
						ValueType = valueType,
						Operator = clauseType,
					},
				},
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = TestPerson.Data.Search(searchParameters);
			Assert.Equal(expected, results.TotalCount);
		}

		[Fact]
		public void Search_NullFields()
		{
			var searchParameters = new SearchParameters
			{
				Page = 1,
				PageSize = 5,
			};

			var results = TestPerson.Data.Search(searchParameters);
			Assert.Equal(2, results.TotalPages);
		}

		[Fact]
		public void Search_NullParameters()
		{
			var searchParameters = (SearchParameters)null;
			var results = TestPerson.Data.Search(searchParameters);
			Assert.NotEmpty(results.Results);
		}

		// [Fact]
		// public void Search_Subclauses()
		// {
		// 	var subclauses = new SearchFieldList(new List<SearchField>
		// 	{
		// 		new SearchField
		// 		{
		// 			Name = "LastName",
		// 			Value = "on",
		// 			ValueType = typeof(string),
		// 			Operator = WhereClauseType.Contains,
		// 		},
		// 		new SearchField
		// 		{
		// 			Name = "LastName",
		// 			Value = "th",
		// 			ValueType = typeof(string),
		// 			Operator = WhereClauseType.Contains,
		// 		}
		// 	}, WhereClauseOperator.OR);

		// 	var fields = new SearchFieldList
		// 	(
		// 		new List<SearchField>
		// 		{
		// 			new SearchField
		// 			{
		// 				Name = "Age",
		// 				Value = 20,
		// 				MaxValue = 30,
		// 				ValueType = typeof(int),
		// 				Operator = WhereClauseType.Between,
		// 			}
		// 		}
		// 	)	{
		// 		Subclauses = subclauses
		// 	};

		// 	var searchParameters = new SearchParameters
		// 	{
		// 		Fields = fields,
		// 		OrderBys = new OrderByClauseList("LastName ASC"),
		// 	};

		// 	var results = TestPerson.Data.Search(searchParameters);
		// 	Assert.Equal(4, results.TotalCount);
		// }
	}
}
