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

		[Fact]
		public async void Send_ArgumentNullException()
		{
			var badEmailService = new EmailService();
			await Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
			{
				await badEmailService.Send(null);
			});

			await Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
			{
				var message = new MailMessage();
				await badEmailService.Send(message);
			});
		}

		[Fact]
		public async void Send_SmtpException()
		{
			await Assert.ThrowsAnyAsync<SmtpException>(async () =>
			{
				var options = new EmailOptions
				{
					Host = "localhost",
					Port = 587,
					Override = "test-override@test.com",
					Username = "test",
					Password = "test",
					UseSSL = true,
				};
				var smtpService = new EmailService(options);
				var message = new MailMessage
				{
					From = new MailAddress("test@test.com"),
					Subject = "Test",
					Body = "Test",
				};
				message.To.Add(options.Override);

				await smtpService.Send(message);
			});
		}


	}
}
