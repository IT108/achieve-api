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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace achieve_backend.Hubs
{
	[EnableCors("default")]
	[Authorize]
	public class ProjectHub : Hub
	{
		private readonly UserService _userService;
		private readonly ProjectService _projectService;


		public ProjectHub(UserService userService, ProjectService projectService)
		{
			_userService = userService;
			_projectService = projectService;
		}
		//[Authorize("Student")]
		public async Task GetProjects()
		{
			List<Project> projects = _projectService.Get();
			Clients.Caller.SendAsync("GetProjects", projects);
		}

		public async Task CreateProject(CreateProjectRequest request)
		{
			User user = _userService.GetByIdentity(Context.UserIdentifier);
			if (user is null)
				Clients.Caller.SendAsync("CreateProject", StatusCodes.Status404NotFound);
			if (user.Group == "Student")
				Clients.Caller.SendAsync("CreateProject", StatusCodes.Status403Forbidden);

			_projectService.Create(request.Project);
			Clients.Caller.SendAsync("CreateProject", StatusCodes.Status200OK);
		}
	}
}