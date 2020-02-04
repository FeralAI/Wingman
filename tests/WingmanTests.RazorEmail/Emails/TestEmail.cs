namespace WingmanTests.RazorEmail.Emails
{
	public class TestEmail : Wingman.RazorEmail.RazorEmail
	{
		public string Title
		{
			get { return "test"; }
		}
	}
}
