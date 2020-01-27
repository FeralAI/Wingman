using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NETStandardLibrary.Email;
using NETStandardLibrary.RazorEmail;
using NETStandardSamples.Web.Data;
using NETStandardSamples.Web.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NETStandardSamples.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// Add core services
			services.AddControllers(o =>
			{
				o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
			});
			services.AddRazorPages();
			services.AddServerSideBlazor();

			// API versioning and documentation
			// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
			// https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
			services.AddApiVersioning(o => {
				o.ReportApiVersions = true;
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new ApiVersion(
					ApiVersions.CurrentVersion.Item1,
					ApiVersions.CurrentVersion.Item2,
					ApiVersions.CurrentVersion.Item3
				);
			});
			services.AddVersionedApiExplorer(o =>
			{
				o.GroupNameFormat = "'v'VVVV";
				o.SubstituteApiVersionInUrl = true;
			});
			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			services.AddSwaggerGen(o =>
			{
				o.OperationFilter<SwaggerDefaultValues>();

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				o.IncludeXmlComments(xmlPath);
			});

			// .NET services
			services.AddTransient(s =>
			{
				var httpClientHandler = new HttpClientHandler();
				// Disable SSL validation since we're hitting localhost with a self-signed cert
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
				return new HttpClient(httpClientHandler)
				{
					// TODO: Make this pull from config/env/???
					BaseAddress = new Uri("https://localhost:5001")
				};
			});

			// NETStandardLibrary services
			services.Configure<EmailOptions>(Configuration.GetSection(nameof(EmailOptions)));
			services.AddSingleton(s =>
			{
				var options = new EmailOptions();
				Configuration.GetSection(nameof(EmailOptions)).Bind(options);
				var emailService = new EmailService(options);
				return emailService;
			});
			services.AddSingleton(s =>
			{
				var options = new EmailOptions();
				Configuration.GetSection(nameof(EmailOptions)).Bind(options);
				var emailService = new RazorEmailService<Startup>(options);
				return emailService;
			});

			// NETStandardSamples services
			services.AddSingleton<PageService>();
			services.AddSingleton<TestPersonService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			app.UseSwagger();
			app.UseSwaggerUI(o =>
			{
				foreach (var description in provider.ApiVersionDescriptions)
				{
					o.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );
				}
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
