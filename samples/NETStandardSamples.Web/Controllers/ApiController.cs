using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NETStandardSamples.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public abstract class ApiController : ControllerBase
	{

	}

	public static class ApiConventions
	{
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
		[ProducesDefaultResponseType]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public static void Search([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object model) { }
	}
}
