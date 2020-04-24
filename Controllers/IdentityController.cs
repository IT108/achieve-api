using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using achieve_backend.Services;
using achieve_backend.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;
using achieve_lib.AD;

namespace achieve_backend.Controllers
{
	[ApiController]
	public class IdentityController : ControllerBase
	{
		[Route("[controller]/updateGroups")]
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Get([FromQuery] [Required] string username)
		{
			return Ok();
		}

		[EnableCors("default")]
		[Route("[controller]/identityCheck")]
		[HttpGet]
		[Authorize]
		public IActionResult GetIdent()
		{
			return Ok("Authenticated");
		}
	}
}
