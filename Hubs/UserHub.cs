using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
		public async Task getUser()
		{

		}
	}
}
