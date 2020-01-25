using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETStandardLibrary.Linq;
using NETStandardSamples.Web.Data;

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
			var searchFields = SearchField.FromObject(form).Where(f => f.Value != null).ToList();
			var parameters = new SearchParameters
			{
				Fields = searchFields,
				OrderBys = new List<OrderByClause>
				{
					new OrderByClause { Name = "LastName", Direction = OrderByDirection.ASC },
				},
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
