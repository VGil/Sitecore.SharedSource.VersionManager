using System;
using Microsoft.AspNet.SignalR;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Hubs
{
	public class StatisticsHub : Hub
	{
	}

	public class Statistics
	{
		private static IHubContext StatisticsHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<StatisticsHub>(); }
		}

		public static void StatisticsChange(Language language, Guid id, float percent, int totalCount, int existingCount)
		{
			StatisticsHubContext.Clients.All.statisticsChange(
				language.Name,
				id.ToString(),
                percent,
				existingCount,
				totalCount);

            Locker.LockUi(id);
		}
	}
}