using System;
using System.Globalization;
using System.Threading;
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

		public static void StatisticsChange(Language language, Guid id, float percent, int itemsProcessed)
		{
			StatisticsHubContext.Clients.All.statisticsChange(
                "#" + language.Name + "_" + id, 
                percent, 
                itemsProcessed.ToString(CultureInfo.InvariantCulture));
		}

        public static void StatisticsChange(Language language, Guid id, int totalCount, int existingCount)
        {
            StatisticsChange(language, id, 100 * (float)existingCount / totalCount, totalCount);
        }
	}
}