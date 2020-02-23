using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Wingman.AspNetCore.Swagger
{
	public static class IApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseSwagger(this IApplicationBuilder @this, IApiVersionDescriptionProvider provider)
		{
			@this.UseSwagger();

			@this.UseSwaggerUI(options =>
			{
				// Sort them descending since swagger will default to the first endpoint mapped
				var orderedDescriptions = provider.ApiVersionDescriptions
					.OrderByDescending(d => d.ApiVersion.MajorVersion)
					.ThenByDescending(d => d.ApiVersion.MinorVersion)
					.ThenByDescending(d => d.ApiVersion.Status);

				foreach (var description in orderedDescriptions)
				{
					var url = $"/swagger/{description.GroupName}/swagger.json";
					options.SwaggerEndpoint(url, description.GroupName.ToUpperInvariant());
				}
			});

			return @this;
		}
	}
}
