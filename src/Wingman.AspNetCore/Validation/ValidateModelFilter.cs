using Microsoft.AspNetCore.Mvc.Filters;

namespace Wingman.AspNetCore.Validation
{
	public class ValidateModelFilter : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context) { }

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
				context.Result = new ValidationFailedResult(context.ModelState);
		}
	}
}
