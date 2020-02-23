using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wingman.AspNetCore.Swagger
{
	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection AddVersionedSwagger(this IServiceCollection @this)
		{
			@this.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

			@this.AddSwaggerGen(options =>
			{
				options.OperationFilter<SwaggerDefaultValues>();
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			return @this;
		}
	}
}
