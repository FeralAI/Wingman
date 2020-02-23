using System;

namespace Wingman.AspNetCore
{
	public interface IExceptionLogger
	{
		void LogError(Exception ex);
	}
}
