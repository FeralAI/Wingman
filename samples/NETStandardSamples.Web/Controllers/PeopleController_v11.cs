using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NETStandardLibrary.Linq;
using NETStandardSamples.Web.Data;

namespace NETStandardSamples.Web.Controllers
{
	[ApiVersion(ApiVersions.v11)]
	public partial class PeopleController : ApiController
	{
		/// <summary>
		/// Returns all test person data.
		/// </summary>
		/// <returns>An <c>IEnumerable&lt;TestPerson&gt;</c> object.</returns>
		[HttpGet, MapToApiVersion(ApiVersions.v10), MapToApiVersion(ApiVersions.v11)]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
		public ActionResult<IEnumerable<TestPerson>> Get()
		{
			return Ok(personService.GetData());
		}

		/// <summary>
		/// Search the dummy database for test people
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///	POST /api/v1.1/people/search
		///	{
		///		"firstName": "Bob",
		///		"lastName": "Smith"
		///	}
		/// </remarks>
		/// <param name="form"></param>
		/// <returns>A <c>SearchResults&lt;TestPerson&gt;</c> object</returns>
		/// <response code="200">Returns the search results</response>
		/// <response code="400">If the query fails</response>
		[HttpPost("search"), MapToApiVersion(ApiVersions.v11)]
		[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
		public ActionResult<SearchResults<TestPerson>> Search_v11(SearchForm form)
		{
			var searchFields = SearchFieldList.FromObject(form, true);
			var parameters = new SearchParameters
			{
				Fields = searchFields,
				OrderBys = new OrderByClauseList("LastName ASC"),
			};

			var results = personService.GetData().Search(parameters);
			return Ok(results);
		}
	}
}
