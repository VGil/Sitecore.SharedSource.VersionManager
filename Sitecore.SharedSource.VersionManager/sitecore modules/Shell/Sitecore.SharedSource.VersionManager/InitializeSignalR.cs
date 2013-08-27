using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.SharedSource.VersionManager
{
	public class InitializeSignalR
	{
		public void Process(PipelineArgs args)
		{
			RouteTable.Routes.MapHubs();
		}
	}
}