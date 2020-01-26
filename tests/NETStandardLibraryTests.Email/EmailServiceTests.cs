using System;
using System.Net.Mail;
using NETStandardLibrary.Email;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailServiceTests : IDisposable
	{
		private IEmailService emailService;

		public EmailServiceTests()
		{
			emailService = new EmailService(Constants.Email_PickupOptions);
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
			{;
				var smtpService = new EmailService(Constants.Email_LocalOptions);
				var message = new MailMessage
				{
					From = new MailAddress("test@test.com"),
					Subject = "Test",
					Body = "Test",
				};
				message.To.Add(Constants.Email_LocalOptions.Override);

				await smtpService.Send(message);
			});
		}

		[Fact]
		public async void SetSmtpClientFactory()
		{
			var factoryService = new EmailService(Constants.Email_LocalOptions, () =>
			{
				var client = new SmtpClient
				{
					DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					PickupDirectoryLocation = Constants.Email_PickupOptions.PickupDirectory,
				};

				return client;
			});

			var message = new MailMessage
			{
				From = new MailAddress("test@test.com"),
				Subject = "Test",
				Body = "Test",
			};
			message.To.Add("test@test.com");

			await factoryService.Send(message);
			Assert.True(true);
		}
	}
}
