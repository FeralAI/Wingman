using System.Reflection;
using System.Threading.Tasks;

namespace NETStandardLibrary.Common
{
	public static class MethodInfoExtensions
	{
		/// <summary>
		/// Invokes an asynchronous method.
		/// </summary>
		/// <param name="@this">The MethodInfo object.</param>
		/// <param name="obj">The object to run the method against.</param>
		/// <param name="parameters">Parameters for the method to be invoked.</param>
		/// <returns>The task result.</returns>
		public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
		{
			dynamic awaitable = @this.Invoke(obj, parameters);
			await awaitable;
			return awaitable.GetAwaiter().GetResult();
		}
	}
}
