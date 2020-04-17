using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using achieve_lib.AD;
using achieve_backend.Utils;
using System.Collections.Generic;
using achieve_lib;
using Microsoft.AspNetCore.Authorization;

namespace achieve_backend.Hubs
{
	[AllowAnonymous]
	public class AuthHub : Hub
	{
		public async Task Connect(ADAuthRequest req)
		{
			Edge.ADAuth(req.Domain, req.Username, req.Password, Context.ConnectionId);
		}
	}
}
