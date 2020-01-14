using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace NETStandardLibrary.Email
{
	public abstract class Email
	{
		/// <summary>
		/// The email body content in string form.
		/// </summary>
		/// <value></value>
		public virtual string Body { get; set; }

		/// <summary>
		/// The email "From" address.
		/// </summary>
		public virtual string From { get; set; }

		/// <summary>
		/// The email subject.
		/// </summary>
		public virtual string Subject { get; set; }

		/// <summary>
		/// The email "To" address.
		/// </summary>
		public virtual string To { get; set; }

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

		/// <summary>
		/// Generates a MailMessage object using base properties.
		/// Throws a <c>NullReferenceException</c> if any required fields are missing.
		/// </summary>
		/// <returns>A <c>MailMessage</c> object.</returns>
		public virtual MailMessage ToMailMessage()
		{
			var required = new Dictionary<string, string>();
			required.Add("Body", Body);
			required.Add("From", From);
			required.Add("Subject", Subject);
			required.Add("To", To);

			var errors = required.Where(kv => string.IsNullOrWhiteSpace(kv.Value));
			if (errors.Count() > 0)
			{
				var prefix = string.Join(", ", errors.Select(kv => kv.Key));
				throw new NullReferenceException($"{prefix} must not be null to create a MailMessage object");
			}

			var message = new MailMessage();
			message.From = new MailAddress(From);
			message.To.Add(To);
			message.Body = Body;
			message.Subject = Subject;
			return message;
		}
	}
}
