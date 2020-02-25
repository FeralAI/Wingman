using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wingman.AspNetCore.Api;
using Wingman.Linq;
using WingmanSamples.Web.Data;
using WingmanSamples.Web.Models;

namespace WingmanSamples.Web.Controllers
{
	[ApiVersion(Api.v1_2)]
	public partial class PeopleController : ApiController
	{
		/// <summary>
		/// Returns all test person data.
		/// </summary>
		/// <returns>An <c>IEnumerable&lt;TestPerson&gt;</c> object.</returns>
		[HttpGet]
		[MapToApiVersion(Api.v1_2)]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
		public ActionResult<IEnumerable<TestPerson>> Get_v1_2() => Get_v1_1();

		/// <summary>
		/// Search the dummy database for test people
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///	POST /api/v1.2/people/search
		///	{
		///		"firstName": "Bob",
		///		"lastName": "Smith"
		///	}
		/// </remarks>
		/// <param name="form"></param>
		/// <returns>A <c>SearchResults&lt;TestPerson&gt;</c> object</returns>
		/// <response code="200">Returns the search results</response>
		/// <response code="400">If the query fails</response>
		[HttpPost("search")]
		[MapToApiVersion(Api.v1_2)]
		[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
		public ActionResult<SearchResults<TestPerson>> Search_v1_2(SearchForm form)
		{
			var searchFields = WhereClause.FromObject(form, ignoreNulls: true);
			var parameters = new SearchParameters
			{
				WhereClause = searchFields,
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = personService.GetData().Search(parameters);
			return Ok(results);
		}
	}
}
