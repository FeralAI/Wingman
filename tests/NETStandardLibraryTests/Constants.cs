using NETStandardLibrary.Email;

namespace NETStandardLibraryTests
{
	public static class Constants
	{
		public static readonly EmailOptions Email_LocalOptions = new EmailOptions
		{
			Host = "localhost",
			Port = 587,
			Override = "test-override@test.com",
			Username = "test",
			Password = "test",
			UseSSL = true,
		};

		public static readonly EmailOptions Email_PickupOptions = new EmailOptions
		{
			PickupDirectory = "C:\\Windows\\Temp",
		};
	}
}
