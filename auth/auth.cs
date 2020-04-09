﻿using achieve_backend.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace achieve_backend.auth
{
	public class auth
	{
		public static bool validateUser(string username, string password)
		{
			bool result = true;
			var credentials = new NetworkCredential(username, password);
			var serverId = new LdapDirectoryIdentifier("dev.it108.org");

			var conn = new LdapConnection(serverId, credentials);
			try
			{
				conn.Bind();
			}
			catch (Exception)
			{
				result = false;
			}

			conn.Dispose();

			return result;
		}
	}
}