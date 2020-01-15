namespace NETStandardLibraryTests.Email.Emails
{
	public class TestEmailInclude : NETStandardLibrary.Email.Email
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
