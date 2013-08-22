using System.Web.UI;
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
            if (EditorContext.IsValid)
            {
                Manager = new VersionService(EditorContext);
            }
		}

		protected SitecoreEditorContext EditorContext { get; private set; }
        protected VersionService Manager { get; private set; }
    }
}