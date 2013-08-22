﻿using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.VersionManager.Logging
{
	public class Logger
	{
		private static IHubContext LogHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<VersionManagerHub>(); }
		}

		public static void Info(string message, object owner)
		{
			Log.Info(message, owner);

			LogHubContext.Clients.All.logMessage(string.Format(
				"{0} INFO: {1}", 
				DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture), 
				message));
		}

		public static void Error(string message, Exception exception, object owner)
		{
			Log.Error(message, exception, owner);

			LogHubContext.Clients.All.logMessage(string.Format(
				"{0} ERROR: {1}. {2}",
				DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture),
				message,
				exception));
		}
	}
}