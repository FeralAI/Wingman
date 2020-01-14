namespace NETStandardLibrary.Email
{
	/// <summary>
	/// Contains email options from a configuration source.
	/// </summary>
	public class EmailOptions
	{
		public string Host { get; set; }
		public string Override { get; set; }
		public string Password { get; set; }
		public string PickupDirectory { get; set; }
		public int? Port { get; set; }
		public string Username { get; set; }
		public bool UseSSL { get; set; }
	}
}
