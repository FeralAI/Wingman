using Microsoft.AspNetCore.Mvc;
using Wingman.Linq;
using WingmanSamples.Web.Data;
using WingmanSamples.Web.Models;

namespace WingmanSamples.Web.Controllers
{
	[ApiVersion(Api.v1_0, Deprecated = true)]
	public partial class PeopleController : ApiController
	{
		/// <summary>
		/// Search the dummy database for test people
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///	POST /api/v1/people/search
		///	POST /api/v1.0/people/search
		///	{
		///		"firstName": "Bob",
		///		"lastName": "Smith"
		///	}
		/// </remarks>
		/// <param name="form"></param>
		/// <returns>A <c>SearchResults&lt;TestPerson&gt;</c> object</returns>
		/// <response code="200">Returns the search results</response>
		/// <response code="400">If the query fails</response>
		[HttpPost("search"), MapToApiVersion(Api.v1_0)]
		[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
		public ActionResult<SearchResults<TestPerson>> Search_v10(SearchForm form)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var searchFields = WhereClause.FromObject(form, true);
			var parameters = new SearchParameters
			{
				WhereClause = searchFields,
				OrderBys = new OrderByClauseList("FirstName ASC, LastName"),
			};

			var results = personService.GetData().Search(parameters);
			return Ok(results);
		}
	}
}
