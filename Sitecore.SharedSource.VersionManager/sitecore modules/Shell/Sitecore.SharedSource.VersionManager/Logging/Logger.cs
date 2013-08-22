using System;
using Microsoft.AspNet.SignalR;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.VersionManager.Logging
{
	public class Logger
	{
		private static IHubContext LogHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<LogHub>(); }
		}

		public static void Info(string message, object owner)
		{
			Log.Info(message, owner);

			LogHubContext.Clients.All.logMessage(string.Format(
				"{0} INFO: {1}", 
				DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), 
				message));
		}
	}
}