using System.Linq;
using Wingman.Common;
using Xunit;

namespace WingmanTests.Common
{
	public class IQueryableExtensionsTests
	{
		[Theory]
		[InlineData(-1, -1, "")]
		[InlineData(0, 0, "")]
		[InlineData(1, 1, "this")]
		[InlineData(2, 2, "a test")]
		[InlineData(3, 3, "code")]
		[InlineData(4, 4, "")]
		public void GetPage(int page, int pageSize, string expected)
		{
			var list = "this is a test of the code".Split(' ').AsQueryable();
			var result = string.Join(" ", list.GetPage(page, pageSize));
			Assert.Equal(expected, result);
		}
	}
}
