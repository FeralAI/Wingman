using System.Threading.Tasks;
using Wingman.Common;
using Xunit;

namespace WingmanTests.Common
{
	public class MethodInfoExtensionsTests
	{
		public async Task<int> Add(int x, int y)
		{
			return await Task<int>.Run(() => x + y);
		}

		public async Task<string> Concatenate(string left, string right)
		{
			return await Task<string>.Run(() => $"{left}{right}");
		}

		[Theory]
		[InlineData("Add", 2, 2, 4)]
		[InlineData("Concatenate", "fire", "truck", "firetruck")]
		public async void InvokeAsync(string name, object param1, object param2, object expected)
		{
			var methodInfo = typeof(MethodInfoExtensionsTests).GetMethod(name);
			var result = await methodInfo.InvokeAsync(this, param1, param2);
			Assert.Equal(expected, result);
		}
	}
}
