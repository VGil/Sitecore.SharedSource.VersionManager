using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Logging;
using Version = Sitecore.Data.Version;

namespace Sitecore.SharedSource.VersionManager.SitecoreEditor
{
	public class SitecoreEditorContext
	{
		public SitecoreEditorContext()
		{
			try
			{
				Id = ID.Parse(HttpContext.Current.Request.QueryString["id"]);
				Language = LanguageManager.GetLanguage(HttpContext.Current.Request.QueryString["language"]);
				Version = int.Parse(HttpContext.Current.Request.QueryString["version"]);
				Database = Database.GetDatabase(HttpContext.Current.Request.QueryString["database"]);
				Item = Database.GetItem(Id, Language, new Version(Version));
				IsValid = true;
			}
			catch (Exception ex)
			{
				Logger.Error("Error loading sitecore editor context.", ex, this);
				IsValid = false;
			}
		}

		public ID Id { get; protected set; }
		public Language Language { get; protected set; }
		public int Version { get; protected set; }
		public Database Database { get; protected set; }
		public Item Item { get; protected set; }


		public bool IsValid { get; protected set; }
	}
}