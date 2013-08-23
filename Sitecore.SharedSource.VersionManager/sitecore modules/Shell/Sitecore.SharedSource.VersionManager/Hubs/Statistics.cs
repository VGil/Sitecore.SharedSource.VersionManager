using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Hubs
{
	public class Statistics
	{
		private static IHubContext LogHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<VersionManagerHub>(); }
		}

		public static void StatisticsChange(Language language, Guid id, float percent)
		{
			LogHubContext.Clients.All.statisticsChange(language.Name, id.ToString(), percent.ToString("#0.00", CultureInfo.InvariantCulture));
		}
	}
}