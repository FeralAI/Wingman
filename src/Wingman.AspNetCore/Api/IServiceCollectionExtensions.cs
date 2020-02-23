using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Wingman.AspNetCore.Api
{
	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection AddVersionedApi(this IServiceCollection @this, ApiVersion defaultVersion, string nameFormat = "VVVV")
		{
			// API versioning and documentation
			// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
			// https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
			@this.AddApiVersioning(options => {
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = defaultVersion;
			});

			@this.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = nameFormat;
				options.SubstitutionFormat = nameFormat;
				options.SubstituteApiVersionInUrl = true;
			});

			return @this;
		}
	}
}
