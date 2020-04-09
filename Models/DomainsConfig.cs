using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace achieve_backend.Models
{
	public static class DomainsConfig
	{
		private static IConfiguration Configuration;

		public static string EdgeAddress { get; set; }
		public static string EdgeAPIToken { get; set; }

		private static Dictionary<string, DomainModel> domains = new Dictionary<string, DomainModel>();

		public static IEnumerable<DomainModel> Domains
		{
			get {
				return domains.Values;
			}
		}

		public static void DefineDomains(List<DomainModel> domainModels, IConfiguration configuration)
		{
			Configuration = configuration;

			EdgeAddress = configuration["EDGE_ADDRESS"];
			EdgeAPIToken = configuration["EDGE_API_TOKEN"];

			if (domains.Count != 0)
				throw new AccessViolationException("Domains already defined");
			foreach (DomainModel domain in domainModels)
			{
				domains.Add(domain.Key, domain);
			}
		}

		public static void UpdateEdgeConfig()
		{
			WebRequest request = WebRequest.Create(EdgeAddress + "/Domain/update");
			request.Method = "GET";

			// Create POST data and convert it to a byte array.  
			byte[] byteArray = Encoding.UTF8.GetBytes(((JObject)Domains).ToString());

			// Set the ContentLength property of the WebRequest.  
			request.ContentLength = byteArray.Length;

			// Get the request stream.  
			Stream dataStream = request.GetRequestStream();
			// Write the data to the request stream.  
			dataStream.Write(byteArray, 0, byteArray.Length);
			// Close the Stream object.  
			dataStream.Close();


			WebResponse response = request.GetResponse();
			// Display the status.  
			Console.WriteLine(((HttpWebResponse)response).StatusDescription);

			// Get the stream containing content returned by the server.  
			// The using block ensures the stream is automatically closed.
			using (dataStream = response.GetResponseStream())
			{
				// Open the stream using a StreamReader for easy access.  
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.  
				string responseFromServer = reader.ReadToEnd();
				// Display the content.  
				Console.WriteLine(responseFromServer);
			}

			// Close the response.  
			response.Close();
		}
	}
}
