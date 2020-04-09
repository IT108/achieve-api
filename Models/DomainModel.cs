using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace achieve_backend.Models
{
	public class DomainModel
	{
		public DomainModel(string domainName, string displayName, string key)
		{
			DomainName = domainName;
			Key = key;
			DisplayName = displayName;
		}

		public DomainModel(string domainName, string displayName)
		{
			DomainName = domainName;
			Key = Utils.PasswordGenerator.Generate(20);
			DisplayName = displayName;
		}

		[Required]
		[JsonProperty("domain")]
		public string DomainName { get; set; }

		[Required]
		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[Required]
		public string Key { get; set; }
	}
}
