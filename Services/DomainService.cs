using MongoDB.Driver;
using achieve_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Services
{
	public class DomainService
	{
		private readonly IMongoCollection<DomainModel> _domains;

		public DomainService(IAchieveDBSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_domains = database.GetCollection<DomainModel>(settings.DomainCollectionName);
		}

		public List<DomainModel> Get() => _domains.Find(domain => true).ToList();

		public DomainModel Get(string id) => _domains.Find<DomainModel>(domain => domain.Id == id).FirstOrDefault();

		public DomainModel GetByDomain(string name) => _domains.Find<DomainModel>(domain => domain.DomainName == name).FirstOrDefault();

		public DomainModel Create(DomainModel domain)
		{
			_domains.InsertOne(domain);
			return domain;
		}

		public void Update(string id, DomainModel domainIn) => _domains.ReplaceOne(domain => domain.Id == id, domainIn);

		public void Remove(User domainIn) => _domains.DeleteOne(domain => domain.Id == domainIn.Id);

		public void Remove(string id) => _domains.DeleteOne(domain => domain.Id == id);
	}
}
