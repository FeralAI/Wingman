using System;
using System.Text.RegularExpressions;

namespace NETStandardLibrary.Common
{
  public static class StringExtensions
	{
		private const string NewLineRegex = @"\r\n?|\n";

		public static bool EqualsIgnore(this string value, string otherValue, Regex regex)
		{
			if (regex == null)
				throw new ArgumentNullException("A Regex must be provided");

			if (string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(otherValue))
				return true;

			var cleanValue = regex.Replace(value ?? string.Empty, string.Empty);
			var cleanOther = regex.Replace(otherValue ?? string.Empty, string.Empty);
			return cleanValue.Equals(cleanOther);
		}

		public static bool EqualsIgnoreLineBreaks(this string value, string otherValue)
		{
			return value.EqualsIgnore(otherValue, new Regex(NewLineRegex));
		}
	}
}
