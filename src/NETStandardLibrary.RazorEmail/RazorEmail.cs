using System.Net.Mail;

namespace NETStandardLibrary.RazorEmail
{
	/// <summary>
	/// A class which represents both the entity for creating an email, and also the data model for that email
	/// template.
	/// </summary>
	public abstract class RazorEmail : MailMessage
	{
		/// <summary>
		/// The key of the Razor template for the email.
		/// </summary>
		public virtual string TemplateKey
		{
			get
			{
				var type = this.GetType();
				var assemblyName = type.Assembly.GetName().Name;
				return type.FullName.Replace(assemblyName + ".", "");
			}
		}
	}
}
