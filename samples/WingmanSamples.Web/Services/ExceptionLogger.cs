using System;
using Wingman.AspNetCore;

namespace WingmanSamples.Web.Services
{
	public class ExceptionLogger : IExceptionLogger
	{
		public void LogError(Exception ex)
		{
			Console.WriteLine("Caught an exception:");
			Console.WriteLine(ex.Message);
		}
	}
}
