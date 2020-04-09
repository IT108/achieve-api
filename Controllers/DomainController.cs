using achieve_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Controllers
{
	[AllowAnonymous]
	[ApiController]
	public class DomainController: ControllerBase
	{
		[HttpGet]
		[Route("[controller]/connect")]
		public IActionResult ConnectDomain([FromQuery] [Required] string domain, [FromQuery] [Required] string password)
		{
			return Ok();
		}

		[HttpGet]
		[Route("[controller]/keys")]
		public IActionResult GetKeys([FromQuery] [Required] string api_key)
		{
			if (api_key != DomainsConfig.EdgeAPIToken)
				return StatusCode(StatusCodes.Status401Unauthorized, "Wrong api key");
			return Ok(DomainsConfig.Domains);
		}
	}
}
