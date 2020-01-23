namespace NETStandardLibraryTests.RazorEmail.Emails
{
	public class TestEmailInclude : NETStandardLibrary.RazorEmail.RazorEmail
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
