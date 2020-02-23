using System;

namespace Wingman.AspNetCore.Middleware
{
	public interface IExceptionLogger
	{
		void LogError(Exception ex);
	}
}
