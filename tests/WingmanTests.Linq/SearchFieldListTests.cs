using System;
using System.Linq;
using Wingman.Linq;
using Xunit;

namespace WingmanTests.Linq
{
	public class SearchFieldListTests
	{
		[Fact]
		public void FromObject()
		{
			var result = WhereClause.FromObject(new { Quantity = 1, Size = 10 });
			Assert.Equal(2, result.Count());
			Assert.Equal(1, (int)result.Where(s => s.Name == "Quantity").First().Value);
			Assert.Equal(10, (int)result.Where(s => s.Name == "Size").First().Value);
		}

		[Fact]
		public void FromObject_ArgumentNullException()
		{
			Assert.ThrowsAny<ArgumentNullException>(() => WhereClause.FromObject(null));
		}

		[Fact]
		public void FromObject_EmptyObject()
		{
			var result = WhereClause.FromObject(new {});
			Assert.Empty(result);
		}

		[Fact]
		public void FromObject_SearchFieldAttribute()
		{
			var result = WhereClause.FromObject(new SearchForm()).First();
			Assert.Equal("Words", result.Name);
			Assert.Equal(typeof(string), result.ValueType);
			Assert.Equal(WhereOperator.Equal, result.WhereOperator);
		}

		private class SearchForm
		{
			[SearchField(WhereOperator.Equal, typeof(string), "Words")]
			public string Text { get; set; }
		}
	}
}
