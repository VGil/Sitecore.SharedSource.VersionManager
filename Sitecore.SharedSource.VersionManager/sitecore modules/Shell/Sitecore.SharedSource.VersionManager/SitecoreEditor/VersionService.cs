using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Commands;
using Sitecore.SharedSource.VersionManager.Hubs;

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

        public IEnumerable<VersionStatistics> GetLanguagePreview(bool reccursive)
        {
            var preview = Languages.Select(x => new VersionStatistics
                {
                    Name = x.Name,
                    Flag = GetFlag(x),
                    Percents = 0
                });

            LoadStatistics(reccursive);

            return preview;
        }

        public void LoadStatistics(bool reccursive)
        {
            Logger.Info(string.Format(
                    "Recalculating statistics... Item '{0}', Database '{1}', Reccursive '{2}'.",
                    _context.Item.Paths.FullPath,
                    _context.Database.Name,
                    reccursive),
                this);

            var thread = new Thread(new LoadStatisticsCommand(_context.Item, reccursive).Execute);
			thread.Start();
        }

        public void Process(string from, IEnumerable<string> to, bool reccursive, bool @override, bool exact)
        {
            Logger.Info(string.Format(
                    "Start processing. Item '{0}', From '{1}', To ('{2}'),  Database '{3}', Reccursive '{4}', Override '{5}', Exact '{6}'.", 
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
            get { return GetLanguages(_context.Database); }
        }

        public static IEnumerable<Language> GetLanguages(Database database)
        {
            var languages = LanguageManager.GetLanguages(database)
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