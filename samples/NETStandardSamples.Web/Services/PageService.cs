using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NETStandardSamples.Web.Services
{
	public class PageService
	{
		public static IEnumerable<Type> GetAllPages()
		{
			return Assembly.GetAssembly(typeof(PageService))
				.GetTypes()
				.Where(t => t.FullName.StartsWith("NETStandardSamples.Web.Pages."))
				.Where(t => !t.FullName.Contains("+"))
				.Where(t => t.FullName.Split(".").Length > 4)
				.OrderBy(t => t.FullName);
		}
	}
}
