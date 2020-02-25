using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wingman.AspNetCore.Swagger
{
	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection AddSwagger(this IServiceCollection @this, string title, string version = "v1")
		{
			@this.AddSwaggerGen(options =>
			{
				options.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
				var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			return @this;
		}

		public static IServiceCollection AddVersionedSwagger(this IServiceCollection @this)
		{
			@this.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

			@this.AddSwaggerGen(options =>
			{
				options.OperationFilter<SwaggerDefaultValues>();
				var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			return @this;
		}

		public static IApplicationBuilder UseSwagger(this IApplicationBuilder @this, string title, string version = "v1")
		{
			@this.UseSwagger();

			@this.UseSwaggerUI(options => options.SwaggerEndpoint($"./{version}/swagger.json", title));

			return @this;
		}

		public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder @this, IApiVersionDescriptionProvider provider)
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
					var url = $"./{description.GroupName}/swagger.json";
					options.SwaggerEndpoint(url, description.GroupName);
				}
			});

			return @this;
		}
	}
}
