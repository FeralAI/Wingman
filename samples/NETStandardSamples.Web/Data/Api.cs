using System;

namespace NETStandardSamples.Web.Data
{
	public static class Api
	{
		public static readonly Tuple<int, int, string> CurrentVersion = new Tuple<int, int, string>(1, 1, null);
		public static string CurrentVersionString => $"{CurrentVersion.Item1}.{CurrentVersion.Item2}{(CurrentVersion.Item3 != null ? "-" : "")}{CurrentVersion.Item3}";
		public const string v1_0 = "1.0";
		public const string v1_1 = "1.1";
	}
}
