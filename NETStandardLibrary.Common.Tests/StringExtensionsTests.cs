using System;
using System.Text.RegularExpressions;
using Xunit;

namespace NETStandardLibrary.Common.Tests
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
		public void EqualsIgnoreLineBreaksBlanks()
		{
			var value = (string)null;
			var other = string.Empty;
			var result = value.EqualsIgnoreLineBreaks(other);
			Assert.True(result);
		}

		[Fact]
		public void EqualsIgnoreLineBreaksFalse()
		{
			var value = "This\r\nhas\nline\r\nbreaks\n";
			var other = "This\nhas\nline\r\nbreaks\r\ntoo";
			var result = value.EqualsIgnoreLineBreaks(other);
			Assert.False(result);
		}

		[Fact]
		public void EqualsIgnoreRegex()
		{
			var value = "The number is 987654321";
			var other = "The number is 1234567890";
			var result = value.EqualsIgnore(other, new Regex(@"[0-9]+"));
			Assert.True(result);
		}

		[Fact]
		public void EqualsIgnoreRegexNull()
		{
			var value = string.Empty;
			var other = string.Empty;
			Assert.ThrowsAny<ArgumentNullException>(() => value.EqualsIgnore(other, null));
		}
	}
}
