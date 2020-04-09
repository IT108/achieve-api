using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace achieve_backend.Models
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("Name")]
		[JsonProperty("Name")]
		public string Name { get; set; }
		public int ActivitiesCount { get; set; }
		public string Category { get; set; }
	}
}
