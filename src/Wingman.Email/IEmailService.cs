using System.Net.Mail;
using System.Threading.Tasks;

namespace Wingman.Email
{
  /// <summary>
  /// An interface for handling rendering and sending of emails using Razor templating.
  /// </summary>
  public interface IEmailService
	{
		/// <summary>
		/// The email sending options.
		/// </summary>
		EmailOptions Options { get; }

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="message">The <c>MailMessage</c> object.</param>
		Task Send(MailMessage message);
	}
}
