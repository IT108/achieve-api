using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using achieve_lib.AD;
using achieve_backend.Utils;
using System.Collections.Generic;
using achieve_lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using achieve_backend.Models;

namespace achieve_backend.Hubs
{
	[AllowAnonymous]
	[EnableCors("default")]
	public class AuthHub : Hub
	{
		[EnableCors("default")]
		public async Task Connect(ADAuthRequest req)
		{
			Edge.ADAuth(req.Domain, req.Username, req.Password, Context.ConnectionId);
		}
    }
}
