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

		public RazorEmailServiceTest()
		{
			emailService = new RazorEmailService<TestEmail>();
		}

		public void Dispose()
		{
			emailService = null;
		}

		[Fact]
		public void Render_ArgumentNullException()
		{
			Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
			{
				await emailService.Render(null);
			});

			Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
			{
				await emailService.Render(null, null);
			});
		}

		[Theory]
		[InlineData(typeof(TestEmail), "Hello test!")]
		[InlineData(typeof(TestEmailInclude), "Hello test!\nI&#x27;m included!")]
		public async void Render_RazorEmail(Type emailType, string expected)
		{
			var email = (NETStandardLibrary.RazorEmail.RazorEmail)Activator.CreateInstance(emailType);
			var result = await emailService.Render(email);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("Testing", null, "Testing")]
		[InlineData("@Model.Text", "Testing", "Testing")]
		public async void Render_StringTemplate(string template, string text, string expected)
		{
			var model = new { Text = text };
			var result = await emailService.Render(template, model);
			Assert.Equal(expected, result);
		}
	}
}
