using System.ComponentModel.DataAnnotations;
using NETStandardLibrary.Linq;

namespace NETStandardSamples.Web.Models
{
	public class SearchForm
	{
		[Required]
		[SearchField(WhereOperator.Contains, typeof(string))]
		public string FirstName { get; set; }

		[SearchField(WhereOperator.Contains, typeof(string))]
		public string LastName { get; set; }
	}
}
