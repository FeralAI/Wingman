using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wingman.AspNetCore.Api
{
	/// <summary>
	/// Basic API result class which will typically represent an error state.
	/// </summary>
	public class ApiResult
	{
		public ApiResult(string message = null, IEnumerable<string> errors = null, int statusCode = (int)HttpStatusCode.BadRequest)
		{
			Message = message;
			StatusCode = statusCode;
			Errors = errors;
		}

		public ApiResult(string message = null, ModelStateDictionary modelState = null, int statusCode = (int)HttpStatusCode.BadRequest)
		{
			Message = message;
			StatusCode = statusCode;
			Errors = modelState.SelectMany(kv => kv.Value.Errors.Select(e => e.ErrorMessage));
		}

		public string Message { get; set; }
		public int StatusCode { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}

	/// <summary>
	/// Basic API result class which will typically represent a success state with a value of the generic type parameter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResult<T> : ApiResult
	{
		public ApiResult(T result, string message = null, IEnumerable<string> errors = null, int statusCode = (int)HttpStatusCode.OK)
			: base(message, errors, statusCode)
		{
			Result = result;
		}

		public ApiResult(T result, string message = null, ModelStateDictionary errors = null, int statusCode = (int)HttpStatusCode.BadRequest)
			: base(message, errors, statusCode)
		{
			Result = result;
		}

		public T Result { get; set; }
	}
}
