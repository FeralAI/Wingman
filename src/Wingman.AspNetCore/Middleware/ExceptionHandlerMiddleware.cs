using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wingman.AspNetCore.Api;

namespace Wingman.AspNetCore.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger, IExceptionLogger exceptionLogger, JsonSerializerOptions jsonOptions)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				if (exceptionLogger != null)
				{
					logger.LogError(ex, $"{nameof(ExceptionHandlerMiddleware)} caught an unhandled exception.");
					exceptionLogger.LogError(ex);
				}

				await HandleException(context, ex, jsonOptions);
			}
		}

		private static async Task HandleException(HttpContext context, Exception ex, JsonSerializerOptions jsonOptions)
		{
			var request = context.Request;
			var result = new ApiResult(
				$"An error of type {ex.GetType().Name} occurred with your request",
				new List<string> { ex.Message },
				StatusCodes.Status500InternalServerError
			);

			var serializeOptions = jsonOptions ?? new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync(JsonSerializer.Serialize(result, serializeOptions));
		}
	}
}
