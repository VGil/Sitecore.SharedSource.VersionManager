using System;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Hubs;

namespace Sitecore.SharedSource.VersionManager.SitecoreEditor
{
	public class SitecoreEditorContext
	{
		public SitecoreEditorContext(string id, string database)
		{
			try
			{
				Id = ID.Parse(id);
				Database = Database.GetDatabase(database);
				Item = Database.GetItem(Id);
				Version = Item.Version.Number;
				IsValid = true;
			}
			catch (Exception ex)
			{
				Logger.Error("Error loading sitecore editor context.", ex, this);
				IsValid = false;
			}
		}

	    public SitecoreEditorContext(string id, string language, string database)
			: this(id, database)
	    {
            try
            {
                Language = LanguageManager.GetLanguage(language);
                Item = Database.GetItem(Id, Language);
                Version = Item.Version.Number;
                IsValid = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading sitecore editor context.", ex, this);
                IsValid = false;
            }
        }

		public SitecoreEditorContext()
            : this(
                HttpContext.Current.Request.QueryString["id"], 
                HttpContext.Current.Request.QueryString["language"], 
                HttpContext.Current.Request.QueryString["database"])
		{
		    int version;
		    if (int.TryParse(HttpContext.Current.Request.QueryString["version"], out version))
		    {
		        Version = version;
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