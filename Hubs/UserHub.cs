using achieve_backend.Services;
using achieve_lib.BL;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace achieve_backend.Hubs
{
	[EnableCors("default")]
	[Authorize]
	public class UserHub : Hub
	{
		private readonly UserService _userService;

		public UserHub(UserService userService)
		{
			_userService = userService;
		}
		//[Authorize("Student")]
		public async Task GetUser()
		{
			System.Console.WriteLine(Context.UserIdentifier);
			User user = _userService.GetByIdentity(Context.UserIdentifier);
			foreach (var claim in Context.User.Claims)
				System.Console.WriteLine(claim.Type + " " + claim.Value);
			System.Console.WriteLine(Context.User.IsInRole("Student"));

			if (user is null)
				Clients.Caller.SendAsync("GetUser", null);
			Clients.Caller.SendAsync("GetUser", user);
		}
	}
}
