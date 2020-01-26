using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NETStandardLibrary.Email;
using NETStandardLibrary.RazorEmail;
using NETStandardSamples.Web.Data;
using NETStandardSamples.Web.Services;

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
			services.AddControllers(o =>
			{
				o.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
			});
			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "NETStandardSamples.Web API",
					Version = "v1",
					Description = "Sample Web API using NETStandardLibrary",
					License = new OpenApiLicense { Name = "UNLICENSED" },
					Contact = new OpenApiContact
					{
						Name = "Jason Skuby",
						Email = "jskuby@gmail.com",
						Url = new Uri("https://github.com/jskuby"),
					},
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			services.AddTransient(s =>
			{
				var httpClientHandler = new HttpClientHandler();
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
				return new HttpClient(httpClientHandler) { BaseAddress = new Uri("https://localhost:5001") };
			});

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
			services.AddSingleton<PageService>();
			services.AddSingleton<TestPersonService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "NETStandardSamples.Web API v1");
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
