using achieve_lib.AD;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace achieve_backend.Utils
{
	public static class Edge
	{
		private static string edgeAddress = null;
		private static string apiKey = null;

		private static HubConnection connection;

		public static void ConfigureEdge(IConfiguration configuration)
		{
			edgeAddress = configuration["EDGE_ADDRESS"];
			apiKey = configuration["EDGE_API_TOKEN"];

			connection = new HubConnectionBuilder()
				.WithUrl($"{edgeAddress}internal", HttpTransportType.WebSockets)
				.WithAutomaticReconnect()
				.ConfigureLogging(logging =>
				{
					logging.SetMinimumLevel(LogLevel.Information);
				})
				.Build();

			RegisterHandlers();
			RegisterService();
		}

		public async static void RegisterService()
		{
			await connection.StartAsync();
		}

		private static void RegisterHandlers()
		{
			connection.Closed += async (error) =>
			{
				await Task.Delay(new Random().Next(0, 5) * 1000);
				await connection.StartAsync();
			};

			connection.Reconnecting += error =>
			{
				Debug.Assert(connection.State == HubConnectionState.Reconnecting);


				return Task.CompletedTask;
			};

			connection.Reconnected += async (connectionId) =>
			{
				Debug.Assert(connection.State == HubConnectionState.Connected);

			};

			connection.On<ADAuthRequest>("UserInfo", new Action<ADAuthRequest>(OnGetUser));
		}

		public static async void ADAuth(string domain, string login, string password)
		{
			ADAuthRequest request = new ADAuthRequest() { ApiKey = apiKey, Domain = domain,
				Username = login, Password = password };
			connection.InvokeAsync("GetUser", request);
		}

		private static void OnGetUser(ADAuthRequest response)
		{
			//TODO: обработка респонса
		}
	}
}
