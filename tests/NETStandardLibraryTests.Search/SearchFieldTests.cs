using System;
using System.Linq;
using NETStandardLibrary.Search;
using Xunit;

namespace NETStandardLibraryTests.Search
{
	public class SearchFieldTests
	{
		[Fact]
		public void FromObject()
		{
			var result = SearchField.FromObject(new { Quantity = 1, Size = 10 });
			Assert.Equal(2, result.Count());
			Assert.Equal(1, (int)result.Where(s => s.Name == "Quantity").First().Value);
			Assert.Equal(10, (int)result.Where(s => s.Name == "Size").First().Value);
		}

		[Fact]
		public void FromObject_ArgumentNullException()
		{
			Assert.ThrowsAny<ArgumentNullException>(() => SearchField.FromObject(null));
		}

		[Fact]
		public void FromObject_EmptyObject()
		{
			var result = SearchField.FromObject(new {});
			Assert.Empty(result);
		}
	}
}
