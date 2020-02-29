using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Wingman.AspNetCore.Api
{
	public static class ApiExtensions
	{
		/// <summary>
		/// Adds versioning to an ASP.NET Core WebApi. Supports reading the version from the url, query string or request header.
		/// </summary>
		/// <remarks>
		/// API versioning and documentation:
		/// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
		/// https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
		/// </remarks>
		/// <param name="this">The service collection</param>
		/// <param name="defaultVersion">The default API version to serve</param>
		/// <param name="nameFormat">The API version format</param>
		/// <param name="queryParam">The query parameter used for versioning</param>
		/// <param name="header">The request header used for versioning</param>
		/// <returns>The service collection</returns>
		public static IServiceCollection AddVersionedApi(this IServiceCollection @this, ApiVersion defaultVersion, string nameFormat = "'v'VVVV", string queryParam = "v", string header = "X-Version")
		{
			@this.AddApiVersioning(options => {
				options.DefaultApiVersion = defaultVersion;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.ReportApiVersions = true;
				options.ApiVersionReader = ApiVersionReader.Combine(
					new UrlSegmentApiVersionReader(),
					new QueryStringApiVersionReader(queryParam),
					new HeaderApiVersionReader(header)
				);
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
