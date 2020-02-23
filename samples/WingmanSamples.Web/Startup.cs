using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Wingman.AspNetCore;
using Wingman.AspNetCore.Api;
using Wingman.AspNetCore.Middleware;
using Wingman.AspNetCore.Swagger;
using Wingman.AspNetCore.Validation;
using Wingman.Email;
using Wingman.RazorEmail;
using WingmanSamples.Web.Data;
using WingmanSamples.Web.Services;

namespace WingmanSamples.Web
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
			var serializerOptions = default(JsonSerializerOptions);
			services
				.AddControllers(options =>
				{
					options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
					options.Filters.Add(typeof(ValidateModelFilter));
				})
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
					serializerOptions = options.JsonSerializerOptions;
				});

			services.AddSingleton<JsonSerializerOptions>(provider => serializerOptions);

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.Configure<AppInfo>(Configuration.GetSection(nameof(AppInfo)));
			services.AddVersionedApi(Api.CurrentVersion, Api.GroupNameFormat);
			services.AddVersionedSwagger();

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

			// Wingman services
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

			// WingmanSamples services
			services.AddSingleton<IExceptionHandler, WingmanSamplesExceptionHandler>();
			services.AddSingleton<PageService>();
			services.AddSingleton<TestPersonService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger(provider);
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseMiddleware<ExceptionHandlerMiddleware>();
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
