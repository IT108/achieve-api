﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using achieve_backend.Services;
using achieve_backend.Models;
using achieve_backend.auth;


namespace achieve_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserService _userService;

		public UsersController(UserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_userService.Get());
		}

		[HttpGet("{id:length(24)}", Name = "GetUser")]
		public ActionResult<User> Get(string id)
		{
			var user = _userService.Get(id);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}



		[HttpPost]
		public ActionResult<User> Create(User user)
		{
			_userService.Create(user);

			return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
		}

		[HttpPut("{id:length(24)}")]
		public IActionResult Update(string id, User userIn)
		{
			var user = _userService.Get(id);

			if (user == null)
			{
				return NotFound();
			}

			_userService.Update(id, userIn);

			return NoContent();
		}

		[HttpDelete("{id:length(24)}")]
		public IActionResult Delete(string id)
		{
			var user = _userService.Get(id);

			if (user == null)
			{
				return NotFound();
			}

			_userService.Remove(user.Id);

			return NoContent();
		}
	}
}