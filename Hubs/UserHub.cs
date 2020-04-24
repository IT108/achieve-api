using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Hubs
{
	[EnableCors("default")]
	[Authorize]
	public class UserHub
	{
	}
}
