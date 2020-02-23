using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Wingman.AspNetCore.Api
{
	[Produces("application/json")]
	[ApiController]
	public abstract class ApiController : ControllerBase
	{
		protected readonly ILogger<ApiController> logger;

		public ApiController(ILogger<ApiController> logger)
		{
			this.logger = logger;
		}
	}
}
