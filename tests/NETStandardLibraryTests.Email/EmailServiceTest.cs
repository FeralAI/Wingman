using System;
using NETStandardLibrary.Email;
using NETStandardLibraryTests.Email.Emails;
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
			var email = new TestEmail
			{
				From = "test@test.com",
				To = "test@test.com",
				Subject = "Test",
				Body = "Test",
			};

			await emailService.Send(email);
			Assert.True(true);
		}
	}
}
