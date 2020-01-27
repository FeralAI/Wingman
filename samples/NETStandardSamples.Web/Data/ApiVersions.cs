using System;

namespace NETStandardSamples.Web.Data
{
	public static class ApiVersions
	{
		public static readonly Tuple<int, int, string> CurrentVersion = new Tuple<int, int, string>(1, 1, null);
		public const string v10 = "1.0";
		public const string v11 = "1.1";
	}
}
