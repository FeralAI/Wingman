using System;
using NETStandardLibraryTests.Email.Emails;
using Xunit;

namespace NETStandardLibraryTests.Email
{
	public class EmailTest
	{
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
		public void ToMailMessage_Exception()
		{
			var email = new TestEmail();
			Assert.ThrowsAny<NullReferenceException>(email.ToMailMessage);
		}
	}
}
