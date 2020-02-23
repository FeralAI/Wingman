using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wingman.AspNetCore.Api
{
	/// <summary>
	/// Basic API result class which typically represents an error state and defaults to a 400 status.
	/// </summary>
	public class ApiResult
	{
		public ApiResult(string message = null, IEnumerable<string> errors = null, int status = StatusCodes.Status400BadRequest)
		{
			Message = message;
			Status = status;
			Errors = errors;
		}

		public ApiResult(string message = null, ModelStateDictionary modelState = null, int status = StatusCodes.Status400BadRequest)
		{
			Message = message;
			Status = status;
			Errors = modelState?.SelectMany(kv => kv.Value.Errors?.Select(e => e.ErrorMessage));
		}

		public string Message { get; set; }
		public int Status { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}

	/// <summary>
	/// Basic API result class which typically represents a success state and defaults to a 200 status with a value of the generic
	/// type parameter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResult<T> : ApiResult
	{
		public ApiResult(T value, string message = null, IEnumerable<string> errors = null, int status = StatusCodes.Status200OK)
			: base(message, errors, status)
		{
			Value = value;
		}

		public ApiResult(T value, string message = null, ModelStateDictionary modelState = null, int status = StatusCodes.Status200OK)
			: base(message, modelState, status)
		{
			Value = value;
		}

		public T Value { get; set; }
	}
}
