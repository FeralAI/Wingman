using System;
using NETStandardLibrary.Email.Tests.Emails;
using Xunit;

namespace NETStandardLibrary.Email.Tests
{
	public class EmailServiceTest : IDisposable
	{
		private IEmailService emailService;

		public EmailServiceTest()
		{
			emailService = new TestEmailService();
		}

    public void Dispose()
    {
      emailService = null;
    }

    [Fact]
		public void Uninitialized()
		{
			Assert.Null(emailService.Engine);
		}

		[Fact]
		public async void UninitializedAction()
		{
			await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
			{
				await emailService.Render(new TestEmail());
			});
		}

		[Fact]
		public void UninitializedOptions()
		{
			Assert.ThrowsAny<ArgumentNullException>(() =>
			{
				emailService.Initialize(null);
			});
		}
	}
}
