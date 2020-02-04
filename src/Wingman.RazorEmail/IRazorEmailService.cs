using System.Threading.Tasks;
using Wingman.Email;
using RazorLight;

namespace Wingman.RazorEmail
{
  /// <summary>
  /// An interface for handling rendering and sending of emails using Razor templating.
  /// </summary>
  public interface IRazorEmailService : IEmailService
	{
		/// <summary>
		/// The <c>RazorLightEngine</c> instance to be used for rendering and caching email templates.
		/// </summary>
		RazorLightEngine Engine { get; }

		/// <summary>
		/// Initializes the RazorLight rendering engine.
		/// </summary>
		/// <param name="options">The email options.</param>
		void Initialize(EmailOptions options);

		/// <summary>
		/// Renders an email with an embedded template.
		/// </summary>
		/// <param name="email">The email object.</param>
		/// <returns>The rendered email body.</returns>
		Task<string> Render(RazorEmail email);

		/// <summary>
		/// Renders a Razor string template.
		/// </summary>
		/// <param name="template"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<string> Render(string template, object model);

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="email">The <c>RazorEmail</c> object.</param>
		Task Send(RazorEmail email);
	}
}
