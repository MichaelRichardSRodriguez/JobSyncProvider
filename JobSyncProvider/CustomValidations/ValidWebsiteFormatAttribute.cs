using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JobSyncProvider.CustomValidations
{
	public class ValidWebsiteFormatAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null) return true; //Allow Null if not required

			string website = value.ToString();

			var regex = new Regex(@"^www\.[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}(\.[a-zA-Z]{2,3})?$");

			return regex.IsMatch(website);
		}

		public override string FormatErrorMessage(string name)
		{
			return $"The {name} field must be a valid website format (e.g., www.google.com.ph).";
		}
	}
}
