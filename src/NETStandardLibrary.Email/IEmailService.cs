using System;
using System.Threading.Tasks;
using RazorLight;

namespace	NETStandardLibrary.Email
{
	public interface IEmailService
	{
		RazorLightEngine Engine { get; }
		EmailOptions Options { get; }

		void Initialize(EmailOptions options);
		Task<string> Render(Email email);
		Task Send(Email email);
	}
}
