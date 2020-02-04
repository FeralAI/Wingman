namespace WingmanTests.RazorEmail.Emails
{
	public class TestEmailInclude : Wingman.RazorEmail.RazorEmail
	{
		public string Include
		{
			get => "I'm included!";
		}

		public string Title
		{
			get => "test";
		}
	}
}
