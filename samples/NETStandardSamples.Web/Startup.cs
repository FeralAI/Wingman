using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETStandardLibrary.Email;
using NETStandardLibrary.RazorEmail;
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
			services.AddControllers();
			services.AddRazorPages();
			services.AddServerSideBlazor();

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
