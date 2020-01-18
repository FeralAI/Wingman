namespace NETStandardLibrary.Email
{
	/// <summary>
	/// Contains email options from a configuration source.
	/// </summary>
	public class EmailOptions
	{
		/// <summary>
		/// The hostname or IP address.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// If you want to redirect an email, say for testing purposes, use this value.
		/// </summary>
		public string Override { get; set; }

		/// <summary>
		/// The SMTP server password.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// The pickup directory for local delivery.
		/// </summary>
		public string PickupDirectory { get; set; }

		/// <summary>
		/// The SMTP server port.
		/// </summary>
		public int? Port { get; set; }

		/// <summary>
		/// The SMTP user name.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Flag to determine use of SSL when sending.
		/// </summary>
		/// <value></value>
		public bool UseSSL { get; set; }
	}
}
