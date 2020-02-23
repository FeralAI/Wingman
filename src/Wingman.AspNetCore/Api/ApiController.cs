using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

		protected JsonResult BadRequestJson(string message, IEnumerable<string> errors = null)
		{
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult(new ApiResult(message, errors));
		}

		protected JsonResult BadRequestJson(string message, ModelStateDictionary modelState = null)
		{
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new JsonResult(new ApiResult(message, modelState));
		}
	}
}
