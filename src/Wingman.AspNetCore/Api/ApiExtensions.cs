using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Wingman.AspNetCore.Api
{
	public static class ApiExtensions
	{
		/// <summary>
		/// Adds versioning to an ASP.NET Core WebApi.
		/// </summary>
		/// <remarks>
		/// API versioning and documentation:
		/// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
		/// https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
		/// </remarks>
		/// <param name="this">The service collection</param>
		/// <param name="defaultVersion">The default API version to serve</param>
		/// <param name="nameFormat">The API version format</param>
		/// <returns>The service collection</returns>
		public static IServiceCollection AddVersionedApi(this IServiceCollection @this, ApiVersion defaultVersion, string nameFormat = "'v'VVVV")
		{
			@this.AddApiVersioning(options => {
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = defaultVersion;
				options.ReportApiVersions = true;
			});

			@this.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = nameFormat;
				options.SubstituteApiVersionInUrl = true;
			});

			return @this;
		}
	}
}
