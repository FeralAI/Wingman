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

		/// <summary>
		/// Search the dummy database for test people
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///	POST /api/search/people
		///	{
		///		"firstName": "Bob",
		///		"lastName": "Smith"
		///	}
		/// </remarks>
		/// <param name="form"></param>
		/// <returns>A <c>SearchResults&lt;TestPerson&gt;</c> object</returns>
		/// <response code="200">Returns the search results</response>
		/// <response code="400">If the query fails</response>
		[HttpPost()]
		[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
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
