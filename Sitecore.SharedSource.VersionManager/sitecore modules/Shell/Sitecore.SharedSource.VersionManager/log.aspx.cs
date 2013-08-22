using System;
using Sitecore.SharedSource.VersionManager.Logging;

namespace Sitecore.SharedSource.VersionManager
{
	public partial class log : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Logger.Info("log message 1", this);
		}
	}
}