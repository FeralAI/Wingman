using System;
using NETStandardLibrary.Email;
using NETStandardLibraryTests.Email.Emails;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailTest
	{
		private IEmailService emailService;

		public EmailTest()
		{
			var options = new EmailOptions { PickupDirectory = "C:\\Windows\\Temp" };
			emailService = new TestEmailService<TestEmail>();
			emailService.Initialize(options);
		}

		[Fact]
		public void ToMailMessage()
		{
			var email = new TestEmail
			{
				To = "to@test.com",
				From = "from@test.com",
				Subject = "Test Subject",
				Body = "Test Body",
			};

			var mailMessage = email.ToMailMessage();
			Assert.NotNull(mailMessage);
			Assert.Equal("to@test.com", mailMessage.To[0].Address);
			Assert.Equal("from@test.com", mailMessage.From.Address);
			Assert.Equal("Test Subject", mailMessage.Subject);
			Assert.Equal("Test Body", mailMessage.Body);
		}

		[Fact]
		public void ToMailMessageException()
		{
			var email = new TestEmail();
			Assert.ThrowsAny<NullReferenceException>(email.ToMailMessage);
		}

		[Fact]
		public async void TestEmailInclude()
		{
			var email = new TestEmailInclude();
			var body = await emailService.Render(email);
			Assert.Equal("Hello test!\nI&#x27;m included!", body);
		}

		[Fact]
		public async void TestEmailRenders()
		{
			var email = new TestEmail();
			var body = await emailService.Render(email);
			Assert.Equal("Hello test!", body);
		}

		[Fact]
		public async void TestEmailSends()
		{
			var email = new TestEmail
			{
				From = "test@test.com",
				To = "test@test.com",
				Subject = "Test",
			};

			await emailService.Send(email);
			Assert.True(true);
		}
	}
}
