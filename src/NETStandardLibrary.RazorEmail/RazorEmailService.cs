using System;
using System.Net.Mail;
using System.Threading.Tasks;
using NETStandardLibrary.Email;
using RazorLight;

namespace NETStandardLibrary.RazorEmail
{
	/// <summary>
	/// A class for handling rendering of emails using Razor templating.
	/// </summary>
	/// <typeparam name="T">Any type property from the assembly with your email assets (classes, templates, etc.)</typeparam>
	public class RazorEmailService<T> : EmailService, IRazorEmailService
	{
		public RazorEmailService()
			: base()
		{
			Initialize();
		}
		public RazorEmailService(EmailOptions options)
			: this()
		{
			Initialize(options);
		}
		public RazorEmailService(EmailOptions options, Func<SmtpClient> smtpClientFactory)
			: this(options)
		{
			SmtpClientFactory = smtpClientFactory;
		}

		/// <summary>
		/// The <c>RazorLightEngine</c> instance to be used for rendering and caching email templates.
		/// </summary>
		public RazorLightEngine Engine { get; protected set; }

		/// <summary>
		/// Initializes the <c>RazorLightEngine</c>.
		/// </summary>
		public virtual void Initialize(EmailOptions options = null)
		{
			Options = options;
			Engine = new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(T))
				.UseMemoryCachingProvider()
				.Build();
		}

		/// <summary>
		/// Renders an email.
		/// </summary>
		/// <param name="email">The email object.</param>
		/// <returns>The rendered email body.</returns>
		public virtual async Task<string> Render(RazorEmail email)
		{
			var body = await Engine.CompileRenderAsync(email.TemplateKey, email);
			return body;
		}

		/// <summary>
		/// Renders an email.
		/// </summary>
		/// <param name="template">The email template as a string.</param>
		/// <param name="model">The model for binding to the email template.</param>
		/// <returns>The rendered email body.</returns>
		public virtual async Task<string> Render(string template, object model = null)
		{
			var body = await Engine.CompileRenderStringAsync(template, template, model);
			return body;
		}

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="email">The <c>RazorEmail</c> object.</param>
		public async Task Send(RazorEmail email)
		{
			if (email == null)
				throw new NullReferenceException("Email is required");

			if (string.IsNullOrWhiteSpace(email.Body))
				email.Body = await Render(email);

			using (var client = SmtpClientFactory())
			{
				await client.SendMailAsync(email);
			}
		}
	}
}
