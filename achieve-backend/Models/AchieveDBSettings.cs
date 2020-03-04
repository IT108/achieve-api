using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Models
{
	public class AchieveDBSettings : IAchieveDBSettings
	{
		public string UsersCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}

	public interface IAchieveDBSettings
	{
		string UsersCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}
