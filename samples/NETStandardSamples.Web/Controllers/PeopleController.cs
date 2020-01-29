using Microsoft.AspNetCore.Mvc;
using NETStandardSamples.Web.Services;

namespace NETStandardSamples.Web.Controllers
{
	[ControllerName("People")]
	[Route("api/v{version:apiVersion}/people")]
	public partial class PeopleController : ApiController
	{
		private readonly TestPersonService personService;

		public PeopleController(TestPersonService personService)
		{
			this.personService = personService;
		}
	}
}
