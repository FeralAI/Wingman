using System;
using NETStandardLibrary.Common;
using NETStandardLibrary.Email;
using NETStandardLibraryTests.Email.Emails;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailServiceTest : IDisposable
	{
		private IEmailService emailService;
		private IEmailService uninitializedService;

		public EmailServiceTest()
		{
			var options = new EmailOptions { PickupDirectory = "C:\\Windows\\Temp" };
			emailService = new TestEmailService<TestEmail>();
			emailService.Initialize(options);

			uninitializedService = new TestEmailService<TestEmail>();
		}

		public void Dispose()
		{
			emailService = null;
			uninitializedService = null;
		}

		[Fact]
		public void EmailService_Uninitialized()
		{
			Assert.Null(uninitializedService.Engine);
		}

		[Fact]
		public async void EmailService_UninitializedAction()
		{
			await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
			{
				await uninitializedService.Render(new TestEmail());
			});
		}

		[Fact]
		public void EmailService_UninitializedOptions()
		{
			Assert.ThrowsAny<ArgumentNullException>(() =>
			{
				uninitializedService.Initialize(null);
			});
		}

		[Theory]
		[InlineData(typeof(TestEmail), "Hello test!")]
		[InlineData(typeof(TestEmailInclude), "Hello test!\nI&#x27;m included!")]
		public async void Render(Type emailType, string expected)
		{
			var email = (NETStandardLibrary.Email.Email)Activator.CreateInstance(emailType);
			var result = await emailService.Render(email);
			Assert.Equal(expected, result);
		}

		[Fact]
		public async void Send()
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
