using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wingman.AspNetCore.Api;

namespace Wingman.AspNetCore.Validation
{
	public class ValidationFailedResult : ObjectResult
	{
		public ValidationFailedResult(ModelStateDictionary modelState)
			: base(new ApiResult("Validation Failed", modelState, StatusCodes.Status400BadRequest))
		{
			StatusCode = StatusCodes.Status400BadRequest;
		}
	}
}
