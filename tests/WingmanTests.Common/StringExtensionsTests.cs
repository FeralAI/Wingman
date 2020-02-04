using System;
using System.Text.RegularExpressions;
using Wingman.Common;
using Xunit;

namespace WingmanTests.Common
{
	public class StringExtensionsTests
	{
		[Theory]
		[InlineData(null, null, true)]
		[InlineData(null, "", false)]
		[InlineData("", null, false)]
		[InlineData("The number is 987654321", "The number is 1234567890", true)]
		[InlineData("The number is 987654321", "The number is 1234567890 still", false)]
		public void EqualsIgnore(string value, string other, bool expected)
		{
			Assert.Equal(expected, value.EqualsIgnore(other, new Regex(@"[0-9]+")));
		}

		[Fact]
		public void EqualsIgnore_Null()
		{
			var value = string.Empty;
			var other = string.Empty;
			Assert.ThrowsAny<ArgumentNullException>(() => value.EqualsIgnore(other, null));
		}

		[Theory]
		[InlineData(null, null, true)]
		[InlineData(null, "", false)]
		[InlineData("", null, false)]
		[InlineData("test", "test", true)]
		[InlineData("Test", "test", true)]
		[InlineData("Test", "case", false)]
		public void EqualsIgnoreCase(string value, string other, bool expected)
		{
			Assert.Equal(expected, value.EqualsIgnoreCase(other));
		}

		[Theory]
		[InlineData(null, null, true)]
		[InlineData(null, "", false)]
		[InlineData("", null, false)]
		[InlineData("This\r\nhas\nline\r\nbreaks\n", "This\nhas\nline\r\nbreaks\r\n", true)]
		[InlineData("This\r\nhas\nline\r\nbreaks\n", "This\nhas\nline\r\nbreaks\r\ntoo", false)]
		public void EqualsIgnoreLineBreaks(string value, string other, bool expected)
		{
			Assert.Equal(expected, value.EqualsIgnoreLineBreaks(other));
		}
	}
}
