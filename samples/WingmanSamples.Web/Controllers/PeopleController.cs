using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WingmanSamples.Web.Services;

namespace WingmanSamples.Web.Controllers
{
	[ControllerName("People")]
	[Route("api/v{version:apiVersion}/people")]
	public partial class PeopleController : ApiController
	{
		private readonly ILogger<PeopleController> logger;
		private readonly TestPersonService personService;

		public PeopleController(ILogger<PeopleController> logger, TestPersonService personService)
		{
			this.logger = logger;
			this.personService = personService;
		}
	}
}