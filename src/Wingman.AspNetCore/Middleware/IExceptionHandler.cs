using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Wingman.AspNetCore.Middleware
{
	public interface IExceptionHandler
	{
		Task HandleException(HttpContext context, Exception ex);
	}
}
