using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wingman.AspNetCore.Api;
using Wingman.AspNetCore.Middleware;

namespace WingmanSamples.Web.Services
{
	public class WingmanSamplesExceptionHandler : ExceptionHandler
	{
		private readonly ILogger<WingmanSamplesExceptionHandler> logger;

		public WingmanSamplesExceptionHandler(ILogger<WingmanSamplesExceptionHandler> logger)
		{
			this.logger = logger;
		}

		public override async Task HandleException(HttpContext context, Exception ex)
		{
			logger.LogError(ex, $"{nameof(ExceptionHandlerMiddleware)} handled this exception.");

			context.Response.Clear();

			var result = new ApiResult(
				$"An error of type {ex.GetType().Name} occurred with your request",
				new List<string> { ex.Message },
				StatusCodes.Status500InternalServerError
			);

			var serializeOptions = new JsonSerializerOptions
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
