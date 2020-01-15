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
			emailService = new TestEmailService<TestEmail>();
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
