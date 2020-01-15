using System.Collections.Generic;
using System.Linq;
using NETStandardLibrary.Linq;
using Xunit;

namespace NETStandardLibraryTests.Linq
{
	public class IQueryableExtensionsTests
	{
		[Fact]
		public void GetPage()
		{
			var list = new List<string>
			{
				"this",
				"is",
				"a",
				"test",
				"of",
				"the",
				"code",
			}.AsQueryable();

			var results = list.GetPage(2, 2).ToList();
			var expected = new List<string>
			{
				"a",
				"test",
			};

			Assert.Equal(expected, results);
		}

		[Fact]
		public void OrderByClause()
		{
			var data = new List<Person> {
				new Person { FirstName = "Bob", LastName = "Smith", Age = 20 },
				new Person { FirstName = "John", LastName = "Johnson", Age = 23 },
			}.AsQueryable();

			var results = data.OrderByClause("LastName ASC");
			Assert.Equal("John", results.First().FirstName);
		}
	}
}
