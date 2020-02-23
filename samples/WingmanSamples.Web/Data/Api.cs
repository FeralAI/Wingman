using Microsoft.AspNetCore.Mvc;

namespace WingmanSamples.Web.Data
{
	public static class Api
	{
		public static readonly ApiVersion CurrentVersion = ApiVersion.Parse(v1_1);
		public const string GroupNameFormat = "'v'VVVV";
		public const string v1_0 = "1.0";
		public const string v1_1 = "1.1";
	}
}
