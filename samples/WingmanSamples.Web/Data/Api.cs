using Microsoft.AspNetCore.Mvc;

namespace WingmanSamples.Web.Data
{
	/// <summary>
	/// Class that contains information for the API.
	/// </summary>
	/// <remarks>
	/// When adding a new API version:
	/// * Add a new `const` for the version, i.e.static `public const string v1_2 = "1.2";`
	/// * Update the parsed version in `CurrentVersion` to the latest
	/// </remarks>
	public static class Api
	{
		public static readonly ApiVersion CurrentVersion = ApiVersion.Parse(v1_1);
		public const string GroupNameFormat = "VVVV";
		public const string v1_0 = "1.0";
		public const string v1_1 = "1.1";
	}
}
