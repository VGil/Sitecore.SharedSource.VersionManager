using System;
using Microsoft.AspNet.SignalR;

namespace Sitecore.SharedSource.VersionManager.Hubs
{
	public class LockHub : Hub
	{
	}

	public static class Locker
	{
		private static IHubContext LockHubContext
		{
			get { return GlobalHost.ConnectionManager.GetHubContext<LockHub>(); }
		}

		public static void LockUi(Guid itemId)
		{
			LockHubContext.Clients.All.lockUi(itemId.ToString(), true);
		}

		public static void UnlockUi(Guid itemId)
		{
			LockHubContext.Clients.All.lockUi(itemId.ToString(), false);
		}
	}
}