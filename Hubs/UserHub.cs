using achieve_backend.Services;
using achieve_lib.BL;
using achieve_lib.Requests;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
			User user = _userService.GetByIdentity(Context.UserIdentifier);
			if (user is null)
				Clients.Caller.SendAsync("GetUser", null);
			Clients.Caller.SendAsync("GetUser", user);
		}

		public async Task UpdateUser(UpdateUserRequest request)
		{
			User user = _userService.GetByIdentity(Context.UserIdentifier);
			if (user is null)
				Clients.Caller.SendAsync("UpdateUser", StatusCodes.Status404NotFound);

			user.UpdateUser(request);
			_userService.Update(user.Id, user);
			Clients.Caller.SendAsync("UpdateUser", StatusCodes.Status200OK);
		}
	}
}