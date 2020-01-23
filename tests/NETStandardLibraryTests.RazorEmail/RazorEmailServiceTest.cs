using System;
using NETStandardLibrary.Email;
using NETStandardLibrary.RazorEmail;
using NETStandardLibraryTests.RazorEmail.Emails;
using Xunit;

namespace NETStandardLibraryTests.RazorEmail
{
	public class RazorEmailServiceTest : IDisposable
	{
		private IRazorEmailService emailService;
		private IRazorEmailService uninitializedService;

		public RazorEmailServiceTest()
		{
			var options = new EmailOptions { PickupDirectory = "C:\\Windows\\Temp" };
			emailService = new RazorEmailService<TestEmail>();
			emailService.Initialize(options);
			uninitializedService = new RazorEmailService<TestEmail>();
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
			var email = (NETStandardLibrary.RazorEmail.RazorEmail)Activator.CreateInstance(emailType);
			var result = await emailService.Render(email);
			Assert.Equal(expected, result);
		}
	}
}
