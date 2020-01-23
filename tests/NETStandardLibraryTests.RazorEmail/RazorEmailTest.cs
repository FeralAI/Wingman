using NETStandardLibraryTests.RazorEmail.Emails;
using Xunit;

namespace NETStandardLibraryTests.RazorEmail
{
	public class RazorEmailTest
	{
		[Fact]
		public void TemplateKey()
		{
			var templateKey = new TestEmail().TemplateKey;
			Assert.Equal("Emails.TestEmail", templateKey);
		}
	}
}
