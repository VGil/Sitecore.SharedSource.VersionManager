using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Logging;

namespace Sitecore.SharedSource.VersionManager.SitecoreEditor
{
    public class VersionService
    {
        private static readonly IDictionary<string, string> LanguageFlags = new Dictionary<string, string>();
        private readonly SitecoreEditorContext _context;

        public VersionService(SitecoreEditorContext context)
        {
            _context = context;
        }

        public IEnumerable<VersionStatistics> GetItemStatistics(bool reccursive)
        {
            Logger.Info(string.Format(
                    "Recalculating statistics... Item '{0}', Database '{1}', Reccursive '{2}'.",
                    _context.Item.Paths.FullPath,
                    _context.Database.Name,
                    reccursive),
                this);

            return Languages.Select(x => new VersionStatistics
                {
                    Name = x.Name,
                    Flag = GetFlag(x),
                    Percents = GetFilledVersionsPercent(_context.Id, x)
                });
        }

        public void Process(string from, IEnumerable<string> to, bool reccursive, bool @override, bool exact)
        {
            Logger.Info(string.Format(
                    "Start processing. Item '{0}', From '{1}', To {2},  Database '{3}', Reccursive '{4}', Override '{5}', Exact '{6}'.", 
                    _context.Item.Paths.FullPath,
                    from, 
                    string.Join("', '", to),
                    _context.Database.Name,
                    reccursive,
                    @override,
                    exact), 
                this);
        }

        public void Clear(Language language, bool reccursive)
        {
            Logger.Info(string.Format(
                   "Clear item versions started... Item '{0}', Language '{1}', Database '{2}', Reccursive '{3}'.",
                   _context.Item.Paths.FullPath,
                   language.Name,
                   _context.Database.Name,
                   reccursive),
               this);
        }

        private IEnumerable<Language> Languages
        {
            get
            {
                var languages = LanguageManager.GetLanguages(_context.Database)
                     .OrderBy(x => x.Name)
                     .ToList();

                if (languages.Any(x => x.Name == "en"))
                {
                    var en = languages.First(x => x.Name == "en");
                    languages.Remove(en);
                    languages.Insert(0, en);
                }

                return languages;
            }
        }

        private float GetFilledVersionsPercent(ID itemId, Language language)
        {
            var v = _context.Database.GetItem(itemId, language);
            return v.Versions.GetVersions().Any() ? 100 : 0;
        }

        public static string GetFlag(Language language)
        {
            var langName = language.Name.ToLower();
            if (!LanguageFlags.ContainsKey(langName))
            {
                LanguageFlags.Add(langName, Database.GetDatabase("master").GetItem(language.Origin.ItemId)[FieldIDs.Icon].Replace("16x16", "24x24"));
            }

            return LanguageFlags[langName];
        }
    }
}