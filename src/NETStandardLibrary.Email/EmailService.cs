using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using RazorLight;

namespace NETStandardLibrary.Email
{
	/// <summary>
	/// An abstract class for handling rendering and sending of emails using Razor templating.
	/// </summary>
	/// <typeparam name="T">Any type property from the assembly with your email assets (classes, templates, etc.)</typeparam>
	public abstract class EmailService<T> : IEmailService
	{
		/// <summary>
		/// The <c>RazorLightEngine</c> instance to be used for rendering and caching email templates.
		/// </summary>
		public RazorLightEngine Engine { get; protected set; }

		/// <summary>
		/// The email sending options.
		/// </summary>
		public EmailOptions Options { get; protected set; }

		/// <summary>
		/// Initializes the <c>RazorLightEngine</c>.
		/// </summary>
		/// <param name="options">The mail delivery options.</param>
		public virtual void Initialize(EmailOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("EmailOptions must be provided");

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
		public virtual async Task<string> Render(Email email)
		{
			CheckEngine();

			var body = await Engine.CompileRenderAsync(email.TemplateKey, email);
			return body;
		}

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="email">The email object.</param>
		public virtual async Task Send(Email email)
		{
			CheckEngine();

			if (email == null)
				throw new ArgumentNullException("Email must not be null");

			using (var client = CreateSmtpClient(Options))
			{
				email.Body = await Render(email);
				var message = email.ToMailMessage();
				await client.SendMailAsync(message);
			}
		}

		/// <summary>
		/// Verifies the RazorLight engine is ready to be used.
		/// </summary>
		protected void CheckEngine()
		{
			if (Engine == null)
				throw new InvalidOperationException("You must call EmailService.Initialize before using the email engine");
		}

		/// <summary>
		/// Create the SMTP client using the given email options.
		/// </summary>
		/// <param name="options">The mail delivery options.</param>
		/// <returns>An <c>SmtpClient</c> object.</returns>
		protected SmtpClient CreateSmtpClient(EmailOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("EmailOptions must not be null");

			var client = new SmtpClient();
			if (!string.IsNullOrWhiteSpace(options.PickupDirectory))
			{
				client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				client.PickupDirectoryLocation = options.PickupDirectory;
			}
			else
			{
				client.Host = options.Host;
				if (options.Port != null)
					client.Port = options.Port.Value;

				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(options.Username, options.Password);
				client.EnableSsl = options.UseSSL;
			}

			return client;
		}
	}
}
