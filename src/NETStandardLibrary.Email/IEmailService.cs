using System.Threading.Tasks;

namespace NETStandardLibrary.Email
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
		/// <param name="email">The email object.</param>
		Task Send(Email email);
	}
}
