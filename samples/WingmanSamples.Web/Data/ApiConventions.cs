using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace WingmanSamples.Web.Data
{
	public static class ApiConventions
	{
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
		[ProducesDefaultResponseType]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public static void Search([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object model) { }
	}
}
