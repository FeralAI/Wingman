using Microsoft.AspNetCore.Mvc;
using NETStandardLibrary.Linq;
using NETStandardSamples.Web.Services;

namespace NETStandardSamples.Web.Controllers
{
	[ControllerName("People")]
	[Route("api/v{version:apiVersion}/people")]
	public partial class PeopleController : ApiController
	{
		public const string ControllerName = "People";
		private readonly TestPersonService personService;

		public PeopleController(TestPersonService personService)
		{
			this.personService = personService;
		}

		public class SearchForm
		{
			[SearchField(WhereClauseType.Contains, typeof(string))]
			public string FirstName { get; set; }

			[SearchField(WhereClauseType.Contains, typeof(string))]
			public string LastName { get; set; }
		}
	}
}
