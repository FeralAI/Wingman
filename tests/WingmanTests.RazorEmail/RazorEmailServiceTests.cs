using System;
using System.Net.Mail;
using Wingman.RazorEmail;
using WingmanTests.RazorEmail.Emails;
using Xunit;

namespace WingmanTests.RazorEmail
{
	public class RazorEmailServiceTests : IDisposable
	{
		private IRazorEmailService emailService;

		public RazorEmailServiceTests()
		{
			emailService = new RazorEmailService<TestEmail>(Constants.Email_PickupOptions);
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
			var email = (Wingman.RazorEmail.RazorEmail)Activator.CreateInstance(emailType);
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

		[Fact]
		public async void Send()
		{
			var factoryService = new RazorEmailService<TestEmail>(Constants.Email_LocalOptions, () =>
			{
				var client = new SmtpClient
				{
					DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					PickupDirectoryLocation = Constants.Email_PickupOptions.PickupDirectory,
				};

				return client;
			});

			var email = new TestEmail
			{
				From = new MailAddress("test@test.com"),
				Subject = "Test",
			};
			email.To.Add("test@test.com");
			await emailService.Send(email);
			Assert.True(true);
		}

		[Fact]
		public async void Send_Body()
		{
			var email = new TestEmail
			{
				From = new MailAddress("test@test.com"),
				Subject = "Test",
				Body = "New body",
			};
			email.To.Add("test@test.com");
			await emailService.Send(email);
			Assert.True(true);
		}

		[Fact]
		public async void Send_EmailNull()
		{
			await Assert.ThrowsAnyAsync<NullReferenceException>(async () =>
			{
				await new RazorEmailService<TestEmail>(null).Send(null);
			});
		}
	}
}
