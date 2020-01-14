namespace NETStandardLibrary.Email.Tests.Emails
{
	public class TestEmailInclude : Email
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
