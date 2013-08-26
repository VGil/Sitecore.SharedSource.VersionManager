using System;
using Microsoft.AspNet.SignalR;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Hubs
{
	public class Statistics
	{
		private static IHubContext StatisticsHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<VersionManagerHub>(); }
		}

		public static void StatisticsChange(Language language, Guid id, float percent)
		{
			StatisticsHubContext.Clients.All.statisticsChange(language.Name, id.ToString(), percent);
		}

        public static void StatisticsChange(Language language, Guid id, int totalCount, int existingCount)
        {
            StatisticsChange(language, id, 100 * (float) existingCount / totalCount);
        }
	}
}