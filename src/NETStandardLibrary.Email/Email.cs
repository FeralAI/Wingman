using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace NETStandardLibrary.Email
{
	/// <summary>
	/// A class which represents both the entity for creating an email, and also the data model for that email
	/// template.
	/// </summary>
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
		/// Generates a MailMessage object using base properties.
		/// Throws a <c>NullReferenceException</c> if any required fields are missing.
		/// </summary>
		/// <returns>A <c>MailMessage</c> object.</returns>
		public virtual MailMessage ToMailMessage()
		{
      var required = new Dictionary<string, string>
      {
        { "Body", Body },
        { "From", From },
        { "Subject", Subject },
        { "To", To }
      };

      var errors = required.Where(kv => string.IsNullOrWhiteSpace(kv.Value));
			if (errors.Count() > 0)
			{
				var prefix = string.Join(", ", errors.Select(kv => kv.Key));
				throw new NullReferenceException($"{prefix} must not be null to create a MailMessage object");
			}

      var message = new MailMessage
      {
        From = new MailAddress(From),
        Body = Body,
        Subject = Subject
      };
      message.To.Add(To);
			return message;
		}
	}
}
