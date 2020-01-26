using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NETStandardLibrary.Email
{
	/// <summary>
	/// A class for handling the sending of emails.
	/// </summary>
	public class EmailService : IEmailService
	{
		public EmailService()
		{
			SetSmtpClientFactory();
		}
		public EmailService(EmailOptions options)
			: this()
		{
			Options = options;
		}
		public EmailService(EmailOptions options, Func<SmtpClient> smtpClientFactory)
		{
			Options = options;
			SetSmtpClientFactory(smtpClientFactory);
		}

		/// <summary>
		/// The email sending options.
		/// </summary>
		public virtual EmailOptions Options { get; set; }

		/// <summary>
		/// The method for creating the <c>SmtpClient</c> when sending emails.
		/// </summary>
		/// <value></value>
		protected Func<SmtpClient> SmtpClientFactory { get; set; }

		/// <summary>
		/// Sets a reference to the smtp factory function.
		/// </summary>
		/// <param name="factory"></param>
		public void SetSmtpClientFactory(Func<SmtpClient> factory = null)
		{
			SmtpClientFactory = factory ?? (() => CreateSmtpClient(Options));
		}

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="email">The email object.</param>
		public virtual async Task Send(MailMessage message)
		{
			if (message == null)
				throw new ArgumentNullException("MailMessage must not be null");

			using (var client = SmtpClientFactory())
				await client.SendMailAsync(message);
		}

		/// <summary>
		/// Create the SMTP client using the provided email options.
		/// </summary>
		/// <param name="options">The mail delivery options.</param>
		/// <returns>An <c>SmtpClient</c> object.</returns>
		protected static SmtpClient CreateSmtpClient(EmailOptions options)
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
