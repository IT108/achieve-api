﻿using achieve_backend.Hubs;
using achieve_lib.AD;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace achieve_backend.Utils
{
	public static class Edge
	{
		private static Dictionary<string, string> pending = new Dictionary<string, string>();

		private static string edgeAddress = null;
		private static string apiKey = null;

		private static IHubContext<AuthHub> authHub;

		private static HubConnection connection;

		public static async Task<bool> ConfigureEdge(IConfiguration configuration, IHubContext<AuthHub> hub)
		{

			edgeAddress = configuration["EDGE_ADDRESS"];
			apiKey = configuration["EDGE_API_TOKEN"];
			authHub = hub;

			connection = new HubConnectionBuilder()
				.WithUrl($"{edgeAddress}internal", HttpTransportType.WebSockets)
				.WithAutomaticReconnect()
				.ConfigureLogging(logging =>
				{
					logging.SetMinimumLevel(LogLevel.Information);
				})
				.Build();

			RegisterHandlers();

			return true;
		}

		public async static Task Connect()
		{
			await connection.StartAsync();
		}

		public async static void Disconnect()
		{
			await connection.StopAsync();
		}

		private static void RegisterHandlers()
		{
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

		private static async Task<bool> ConnectToSignalRServer()
		{
			Console.WriteLine("Trying to connect");
			bool connected = false;
			try
			{
				await connection.StartAsync();

				if (connection.State == HubConnectionState.Connected)
				{
					connected = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
			return connected;
		}

		public async static Task TryConnect()
		{
			while (true)
			{
				bool connected = await ConnectToSignalRServer();
				if (connected)
					return;
				Thread.Sleep(1000);
			}
		}

		public static async void ADAuth(string domain, string login, string password, string caller)
		{
			ADAuthRequest request = new ADAuthRequest()
			{
				ApiKey = apiKey,
				Domain = domain,
				Username = login,
				Password = password
			};
			if (connection.State != HubConnectionState.Connected)
				await TryConnect();
			
			pending.Add(request.RequestNumber, caller);
			connection.InvokeAsync("GetUser", request);
		}

		private static void OnGetUser(ADAuthRequest response)
		{
			response.ApiKey = "";
			authHub.Clients.Client(pending[response.RequestNumber]).SendAsync("Connect", response);
		}
	}
}
