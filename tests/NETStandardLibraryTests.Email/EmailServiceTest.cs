using System;
using System.Net.Mail;
using NETStandardLibrary.Email;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailServiceTest : IDisposable
	{
		private IEmailService emailService;

		public EmailServiceTest()
		{
			var options = new EmailOptions { PickupDirectory = "C:\\Windows\\Temp" };
			emailService = new EmailService(options);
		}

		public void Dispose()
		{
			emailService = null;
		}

		[Fact]
		public async void Send()
		{
			var message = new MailMessage
			{
				From = new MailAddress("test@test.com"),
				Subject = "Test",
				Body = "Test",
			};
			message.To.Add("test@test.com");

			await emailService.Send(message);
			Assert.True(true);
		}
	}
}
