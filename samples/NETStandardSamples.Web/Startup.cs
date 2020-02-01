using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
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
			services.AddControllers(options =>
			{
				options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
			});
			services.AddRazorPages();
			services.AddServerSideBlazor();

			// API versioning and documentation
			// https://github.com/microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
			// https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
			services.AddApiVersioning(options => {
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(
					ApiVersions.CurrentVersion.Item1,
					ApiVersions.CurrentVersion.Item2,
					ApiVersions.CurrentVersion.Item3
				);
			});
			services.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVVV";
				options.SubstituteApiVersionInUrl = true;
			});
			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			services.AddSwaggerGen(options =>
			{
				options.OperationFilter<SwaggerDefaultValues>();

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			// .NET services
			services.AddTransient(provider =>
			{
				var httpClientHandler = new HttpClientHandler
				{
					// Disable SSL validation since we're hitting localhost with a self-signed cert
					ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
				};

				return new HttpClient(httpClientHandler)
				{
					// TODO: Make this pull from config/env/???
					BaseAddress = new Uri("https://localhost:5001")
				};
			});

			// NETStandardLibrary services
			services.Configure<EmailOptions>(Configuration.GetSection(nameof(EmailOptions)));
			services.AddSingleton(provider =>
			{
				var options = provider.GetService<IOptions<EmailOptions>>().Value;
				return new EmailService(options);
			});
			services.AddSingleton(provider =>
			{
				var options = provider.GetService<IOptions<EmailOptions>>().Value;
				return new RazorEmailService<Startup>(options, () =>
				{
					return new SmtpClient(options.Host, options.Port.Value)
					{
						Credentials = new NetworkCredential(options.Username, options.Password),
						EnableSsl = options.UseSSL,
					};
				});
			});

			// NETStandardSamples services
			services.AddSingleton<PageService>();
			services.AddSingleton<TestPersonService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					foreach (var description in provider.ApiVersionDescriptions)
					{
						var url = $"/swagger/{description.GroupName}/swagger.json";
						options.SwaggerEndpoint(url, description.GroupName.ToUpperInvariant() );
					}
				});
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
