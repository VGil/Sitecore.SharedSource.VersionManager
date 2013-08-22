using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sitecore.Data.Managers;
using Sitecore.SharedSource.VersionManager.SitecoreEditor;

namespace Sitecore.SharedSource.VersionManager
{
    public partial class VersionManager : Page
    {
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			EditorContext = new SitecoreEditorContext();
			Visible = EditorContext.IsValid;
		}

		protected SitecoreEditorContext EditorContext { get; private set; }

	    protected IEnumerable<LanguageData> Languages
	    {
			get 
			{ 
				return LanguageManager.GetLanguages(EditorContext.Database)
					.OrderBy(x => x.Name)
					.Select(x => new LanguageData
						{
							Name = x.Name,
							Flag = EditorContext.Database.GetItem(x.Origin.ItemId)[FieldIDs.Icon].Replace("16x16", "24x24"),
							Percents = 40
						}); 
			}
	    }
    }
}