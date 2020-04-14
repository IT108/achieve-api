using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
		public static int KeyLength { get; set; } = 20;
		public DomainModel(string domainName, string displayName, string key)
		{
			DomainName = domainName;
			Key = key;
			DisplayName = displayName;
		}

		public DomainModel(string domainName, string displayName)
		{
			DomainName = domainName;
			Key = Utils.PasswordGenerator.Generate(KeyLength, 0, requireNonAlphanumeric: false);
			DisplayName = displayName;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get ;set; }

		[Required]
		[JsonProperty("domain")]
		public string DomainName { get; set; }

		[Required]
		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[Required]
		[JsonProperty("key")]
		public string Key { get; set; }
	}
}
