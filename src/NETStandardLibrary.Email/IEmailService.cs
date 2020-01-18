using System;
using System.Threading.Tasks;
using RazorLight;

namespace	NETStandardLibrary.Email
{
	/// <summary>
	/// An interface for handling rendering and sending of emails using Razor templating.
	/// </summary>
	public interface IEmailService
	{
		/// <summary>
		/// The <c>RazorLightEngine</c> instance to be used for rendering and caching email templates.
		/// </summary>
		RazorLightEngine Engine { get; }

		/// <summary>
		/// The email sending options.
		/// </summary>
		EmailOptions Options { get; }

		/// <summary>
		/// Initializes the <c>RazorLightEngine</c>.
		/// </summary>
		/// <param name="options">The mail delivery options.</param>
		void Initialize(EmailOptions options);

		/// <summary>
		/// Renders an email.
		/// </summary>
		/// <param name="email">The email object.</param>
		/// <returns>The rendered email body.</returns>
		Task<string> Render(Email email);

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="email">The email object.</param>
		Task Send(Email email);
	}
}
