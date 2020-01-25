using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETStandardLibrary.Linq;
using NETStandardSamples.Web.Data;
using NETStandardSamples.Web.Services;

namespace NETStandardSamples.Controllers
{
	[Route("api/search/people")]
	public class SearchPeopleController : ApiController
	{
		private readonly TestPersonService personService;

		public SearchPeopleController(TestPersonService personService)
		{
			this.personService = personService;
		}

		[HttpPost()]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<SearchResults<TestPerson>> Search(SearchForm form)
		{
			var searchFields = SearchFieldList.FromObject(form, true);
			searchFields.RemoveAll(f => f.Value == null);
			var parameters = new SearchParameters
			{
				Fields = searchFields,
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = personService.GetData().Search(parameters);
			return Ok(results);
		}
	}

	public class SearchForm
	{
		[SearchField(WhereClauseType.Contains, typeof(string))]
		public string FirstName { get; set; }

		[SearchField(WhereClauseType.Contains, typeof(string))]
		public string LastName { get; set; }
	}
}
