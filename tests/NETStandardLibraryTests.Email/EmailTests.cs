using System.Net.Mail;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailTests
	{
		[Fact]
		public void ToMailMessage()
		{
			var message = new MailMessage
			{
				From = new MailAddress("from@test.com"),
				Subject = "Test Subject",
				Body = "Test Body",
			};
			message.To.Add("to@test.com");

			Assert.NotNull(message);
			Assert.Equal("to@test.com", message.To[0].Address);
			Assert.Equal("from@test.com", message.From.Address);
			Assert.Equal("Test Subject", message.Subject);
			Assert.Equal("Test Body", message.Body);
		}
	}
}
