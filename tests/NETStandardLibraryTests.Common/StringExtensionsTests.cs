using System;
using System.Text.RegularExpressions;
using NETStandardLibrary.Common;
using Xunit;

namespace NETStandardLibraryTests.Common
{
	public class StringExtensionsTests
	{
		[Fact]
		public void EqualsIgnoreLineBreaks()
		{
			var value = "This\r\nhas\nline\r\nbreaks\n";
			var other = "This\nhas\nline\r\nbreaks\r\n";
			var result = value.EqualsIgnoreLineBreaks(other);
			Assert.True(result);
		}

		[Fact]
		public void EqualsIgnoreLineBreaks_Blanks()
		{
			var value = (string)null;
			var other = string.Empty;
			var result = value.EqualsIgnoreLineBreaks(other);
			Assert.True(result);
		}

		[Fact]
		public void EqualsIgnoreLineBreaks_False()
		{
			var value = "This\r\nhas\nline\r\nbreaks\n";
			var other = "This\nhas\nline\r\nbreaks\r\ntoo";
			var result = value.EqualsIgnoreLineBreaks(other);
			Assert.False(result);
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

		[Fact]
		public void EqualsIgnore_Regex()
		{
			var value = "The number is 987654321";
			var other = "The number is 1234567890";
			var result = value.EqualsIgnore(other, new Regex(@"[0-9]+"));
			Assert.True(result);
		}

		[Fact]
		public void EqualsIgnore_RegexNull()
		{
			var value = string.Empty;
			var other = string.Empty;
			Assert.ThrowsAny<ArgumentNullException>(() => value.EqualsIgnore(other, null));
		}
	}
}
