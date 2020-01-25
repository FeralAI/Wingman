using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETStandardSamples.Web.Data;

namespace NETStandardSamples.Controllers
{
	[Route("api/people")]
	public class PeopleController : ApiController
	{
		private readonly TestPersonService personService;

		public PeopleController(TestPersonService personService)
		{
			this.personService = personService;
		}

		[HttpGet()]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<TestPerson>> Get()
		{
			return Ok(personService.GetData());
		}
	}
}
