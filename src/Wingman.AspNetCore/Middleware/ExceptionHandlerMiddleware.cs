using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wingman.AspNetCore.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger, IExceptionLogger exceptionLogger)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				if (logger == null)
				{
					throw ex;
				}
				else
				{
					logger.LogError(ex, $"{nameof(ExceptionHandlerMiddleware)} caught an unhandled exception.");
					exceptionLogger.LogError(ex);
				}
			}
		}
	}
}
