using System;
using System.Collections.Generic;
using System.Linq;
using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	public class SearchMethodsTests
	{
		[Theory]
		[InlineData("FirstName", "Steven", null, typeof(string), WhereOperator.Equal, 1)]
		[InlineData("Weight", 200, null, typeof(int?), WhereOperator.GreaterThan, 24)]
		[InlineData("Age", 21, null, typeof(int), WhereOperator.LessThanOrEqual, 14)]
		[InlineData("Created", "1998-01-01", "1999-01-01", typeof(DateTime), WhereOperator.Between, 100)]
		[InlineData("Friend.FirstName", "Patty", null, typeof(string), WhereOperator.Equal, 1)]
		[InlineData("Friend.FirstName", "Patty", null, typeof(string), WhereOperator.NotEqual, 49)]
		public void Search(string name, object value, object maxValue, Type valueType, WhereOperator clauseType, int expected)
		{
			var searchParameters = new SearchParameters
			{
				WhereClause = new WhereClause
				{
					new SearchField
					{
						Name = name,
						Value = (valueType == typeof(DateTime)) ? DateTime.Parse((string)value) : value,
						MaxValue = (valueType == typeof(DateTime)) ? DateTime.Parse((string)maxValue) : maxValue,
						ValueType = valueType,
						WhereOperator = clauseType,
					},
				},
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = TestPerson.GenerateData().Search(searchParameters);
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

			var results = TestPerson.GenerateData().Search(searchParameters);
			Assert.Equal(20, results.TotalPages);
		}

		[Fact]
		public void Search_NullParameters()
		{
			var searchParameters = default(SearchParameters);
			var results = TestPerson.GenerateData().Search(searchParameters);
			Assert.NotEmpty(results.Results);
		}

		[Fact]
		public void Search_Subclauses()
		{
			// WHERE (FirstName LIKE '%ac%' AND LastName LIKE '%mith%')
			// OR (FirstName = 'Josephine'
			// 	OR (LastName = 'Dickinson'
			// 		AND (Age = 20 OR Age = 22 OR Age = 25 OR Age = 30 OR Age = 48)
			// 	)
			// )
			var clause1 = new WhereClause(new List<SearchField> {
				new SearchField("FirstName", "ac", WhereOperator.Contains),
				new SearchField("LastName", "mith", WhereOperator.Contains),
			}, WhereJoinOperator.And);

			var subSubclause = new WhereClause(new List<SearchField> {
				new SearchField("Age", 20, WhereOperator.Equal),
				new SearchField("Age", 22, WhereOperator.Equal),
				new SearchField("Age", 25, WhereOperator.Equal),
				new SearchField("Age", 30, WhereOperator.Equal),
				new SearchField("Age", 48, WhereOperator.Equal),
			}, WhereJoinOperator.Or);

			var subclause = new WhereClause(new List<SearchField> {
				new SearchField("LastName", "Dickinson", WhereOperator.Contains)
			}, WhereJoinOperator.And) {
				Subclauses = new List<WhereClause> { subSubclause },
				JoinToSubclauseOperator = WhereJoinOperator.Or,
			};

			var clause2 = new WhereClause(new List<SearchField> {
				new SearchField("FirstName", "Josephine", WhereOperator.Equal)
			}) {
				Subclauses = new List<WhereClause> { subclause },
				JoinToSubclauseOperator = WhereJoinOperator.And,
			};

			var whereClause = new WhereClause
			{
				Subclauses = new List<WhereClause> { clause1, clause2 },
				JoinToSubclauseOperator = WhereJoinOperator.And,
				SubclauseJoinOperator = WhereJoinOperator.Or,
			};

			var searchParameters = new SearchParameters
			{
				WhereClause = whereClause,
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = TestPerson.GenerateData().Search(searchParameters);
			Assert.Equal(2, results.TotalCount);
			Assert.NotEmpty(results.Results.Where(p => p.FirstName == "Josephine"));
			Assert.NotEmpty(results.Results.Where(p => p.FirstName == "Tracey"));
		}

		// [Fact]
		// public void Search_DynamicType()
		// {
		// 	var collection = new List<dynamic>
		// 	{
		// 		new { Name = "Bob", Age = 20 },
		// 		new { Name = "Joe", Age = 25 },
		// 		new { Name = "Mary", Age = 22 }
		// 	};

		// 	var whereClause = new WhereClause(new List<SearchField>
		// 	{
		// 		new SearchField("Name", "Joe", WhereOperator.Equal)
		// 	});

		// 	var searchParameters = new SearchParameters
		// 	{
		// 		WhereClause = whereClause
		// 	};

		// 	var results = collection.AsQueryable().Search(searchParameters);
		// 	Assert.Single(results.Results);
		// 	Assert.Equal(25, results.Results.First().Age);
		// }
	}
}
