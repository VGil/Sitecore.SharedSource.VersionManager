using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Commands;
using Sitecore.SharedSource.VersionManager.SitecoreEditor;

namespace Sitecore.SharedSource.VersionManager
{
    public partial class VersionManager : Page
    {
        protected AbstractCommand _command;

        protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			EditorContext = new SitecoreEditorContext();
			Visible = EditorContext.IsValid;

            if (EditorContext.IsValid)
            {
                _command = new LoadStatisticsCommand(EditorContext.Item, false);
                _command.Execute();
            }
		}

		protected SitecoreEditorContext EditorContext { get; private set; }

        private static readonly IDictionary<string, string> LanguageFlags = new Dictionary<string, string>();

        private static string GetFlag(Language language)
        {
            var langName = language.Name.ToLower();
            if (!LanguageFlags.ContainsKey(langName))
            {
                LanguageFlags.Add(langName, Database.GetDatabase("master").GetItem(language.Origin.ItemId)[FieldIDs.Icon].Replace("16x16", "24x24"));
            }

            return LanguageFlags[langName];
        }

        public IEnumerable<LanguagePreview> GetLanguagePreview()
        {
            var preview = _command.Languages.Select(x => new LanguagePreview
            {
                Name = x.Name,
                Flag = GetFlag(x),
                Percents = _command.GetStatistics(x),
                Total = _command.TotalCount[x],
                Existing = _command.ExistingCount[x]
            });

            return preview;
        }
    }
}