using Microsoft.AspNetCore.Mvc;
using NETStandardLibrary.Linq;
using NETStandardSamples.Web.Data;
using NETStandardSamples.Web.Models;

namespace NETStandardSamples.Web.Controllers
{
	[ApiVersion(ApiVersions.v1_0, Deprecated = true)]
	public partial class PeopleController : ApiController
	{
		/// <summary>
		/// Search the dummy database for test people
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///	POST /api/v1/people/search
		///	{
		///		"firstName": "Bob",
		///		"lastName": "Smith"
		///	}
		/// </remarks>
		/// <param name="form"></param>
		/// <returns>A <c>SearchResults&lt;TestPerson&gt;</c> object</returns>
		/// <response code="200">Returns the search results</response>
		/// <response code="400">If the query fails</response>
		[HttpPost("search"), MapToApiVersion(ApiVersions.v1_0)]
		[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
		public ActionResult<SearchResults<TestPerson>> Search_v10(SearchForm form)
		{
			var searchFields = SearchFieldList.FromObject(form, true);
			var parameters = new SearchParameters
			{
				Fields = searchFields,
				OrderBys = new OrderByClauseList("FirstName ASC, LastName"),
			};

			var results = personService.GetData().Search(parameters);
			return Ok(results);
		}
	}
}
