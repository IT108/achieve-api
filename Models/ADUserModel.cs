using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Models
{
	public class ADUserModel
	{
		[Required]
		[JsonRequired]
		[JsonProperty("username")]
		public string Username { get; set; }

		[Required]
		[JsonRequired]
		[JsonProperty("display_name")]
		string DisplayName { get; set; }

		[Required]
		[JsonRequired]
		[JsonProperty("givenname")]
		string FirstName { get; set; }

		[Required]
		[JsonRequired]
		[JsonProperty("surname")]
		string Surname { get; set; }

		[Required]
		[JsonRequired]
		[JsonProperty("principal_name")]
		string PrincipalName { get; set; }

		[Required]
		[JsonRequired]
		[JsonProperty("groups")]
		List<string> Groups { get; set; }
	}
}
