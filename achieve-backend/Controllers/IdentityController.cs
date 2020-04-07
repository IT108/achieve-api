using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using achieve_backend.Services;
using achieve_backend.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace achieve_backend.Controllers
{
	[Route("identity")]
	[Authorize]
	public class IdentityController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
		}
	}
}
