using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Wingman.AspNetCore.Middleware
{
	public abstract class ExceptionHandler : IExceptionHandler
	{
		public virtual Task HandleException(HttpContext context, Exception ex)
		{
			throw new NotImplementedException();
		}
	}
}
