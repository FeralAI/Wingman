using System.ComponentModel.DataAnnotations;
using Wingman.Linq;

namespace WingmanSamples.Web.Models
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
