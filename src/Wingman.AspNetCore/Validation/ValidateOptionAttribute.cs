using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wingman.AspNetCore.Validation
{
	public class ValidateOptionAttribute : ValidationAttribute
	{
		public List<object> Values;

		public ValidateOptionAttribute(params object[] values)
		{
			Values = values?.ToList();
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
				return ValidationResult.Success;

			if (Values == null || Values.Count == 0 || !Values.Contains(value))
				return new ValidationResult($"The {validationContext.DisplayName ?? validationContext.MemberName} field is invalid.");
			else
				return ValidationResult.Success;
		}
	}
}
