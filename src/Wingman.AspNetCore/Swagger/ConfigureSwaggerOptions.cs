using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Wingman.AspNetCore.Swagger
{
	/// <summary>
	/// Configures the Swagger generation options.
	/// </summary>
	/// <remarks>This allows API versioning to define a Swagger document per API version after the
	/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly AppInfo appInfo;
		private readonly IApiVersionDescriptionProvider provider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
		/// </summary>
		/// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
		/// <param name="appInfoOptions">The options provider for <c>AppInfo</c>.null</param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<AppInfo> appInfoOptions)
		{
			this.appInfo = appInfoOptions.Value;
			this.provider = provider;
		}

		/// <inheritdoc />
		public void Configure(SwaggerGenOptions options)
		{
			// add a swagger document for each discovered API version
			// note: you might choose to skip or document deprecated API versions differently
			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, appInfo));
			}
		}

		private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, AppInfo appInfo)
		{
			var info = new OpenApiInfo
			{
				Title = appInfo.Title,
				Version = description.GroupName,
				Description = appInfo.Description,
				License = new OpenApiLicense { Name = appInfo.License },
				Contact = new OpenApiContact
				{
					Name = appInfo.ContactName,
					Email = appInfo.ContactEmail,
					Url = new Uri(appInfo.ContactUrl),
				},
			};

			if (description.IsDeprecated)
			{
				info.Description = $@"<b style=""color: red"">[DEPRECATED]</b><br />{info.Description}";
			}

			return info;
		}
	}
}
