using WingmanTests.RazorEmail.Emails;
using Xunit;

namespace WingmanTests.RazorEmail
{
	public class RazorEmailTests
	{
		[Fact]
		public void TemplateKey()
		{
			var templateKey = new TestEmail().TemplateKey;
			Assert.Equal("Emails.TestEmail", templateKey);
		}
	}
}
