using System;
using System.Text.RegularExpressions;

namespace NETStandardLibrary.Common
{
  public static class StringExtensions
	{
		/// <summary>
		/// The regex string to be used for new line checks in methods.
		/// </summary>
		public const string NewLineRegex = @"\r\n?|\n";

		/// <summary>
		/// Runs an equality check while ignoring the provided regular expression.
		/// </summary>
		/// <param name="@this">The string.</param>
		/// <param name="otherValue">The value to compare against.</param>
		/// <param name="regex">The regular expression to be applied to both sides of the comparison.</param>
		/// <returns>Equality check boolean.</returns>
		public static bool EqualsIgnore(this string @this, string otherValue, Regex regex)
		{
			if (regex == null)
				throw new ArgumentNullException("A Regex must be provided");

			if (@this == null && otherValue == null)
				return true;

			if ((@this == null && otherValue != null) || (@this != null && otherValue == null))
				return false;

			var cleanValue = regex.Replace(@this, string.Empty);
			var cleanOther = regex.Replace(otherValue, string.Empty);
			return cleanValue.Equals(cleanOther);
		}

		/// <summary>
		/// Runs an equality check while ignoring case.
		/// </summary>
		/// <param name="@this">The string.</param>
		/// <param name="otherValue">The value to compare against.</param>
		/// <returns>Equality check boolean.</returns>
		public static bool EqualsIgnoreCase(this string @this, string otherValue)
		{
			if (@this == null && otherValue == null)
				return true;

			if ((@this == null && otherValue != null) || (@this != null && otherValue == null))
				return false;

			var loweredValue = @this.ToLower();
			var loweredOther = @otherValue.ToLower();
			return loweredValue.Equals(loweredOther);
		}

		/// <summary>
		/// Runs an equality check while ignoring typical line break patterns.
		/// </summary>
		/// <param name="@this">The string.</param>
		/// <param name="otherValue">The value to compare to.</param>
		/// <returns>Equality check boolean.</returns>
		public static bool EqualsIgnoreLineBreaks(this string @this, string otherValue)
		{
			return @this.EqualsIgnore(otherValue, new Regex(NewLineRegex));
		}
	}
}
