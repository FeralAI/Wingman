using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wingman.AspNetCore.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger, IExceptionHandler exceptionHandler)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				try
				{
					await exceptionHandler.HandleException(context, ex);
				}
				catch (NotImplementedException)
				{
					logger.LogError(ex, $"{this.GetType().Name} couldn't handle this exception.");
					throw ex;
				}
			}
		}
	}
}
