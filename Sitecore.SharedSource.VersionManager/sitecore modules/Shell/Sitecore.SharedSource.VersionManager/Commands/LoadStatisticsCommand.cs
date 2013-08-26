using System.Collections.Generic;
using System.Threading;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Hubs;
using Sitecore.SharedSource.VersionManager.SitecoreEditor;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public class LoadStatisticsCommand : AbstractCommand
    {
        public bool IsReccursive { get; private set; }

        private IDictionary<Language, int> TotalCount { get; set; }
        private Dictionary<Language, int> ExistingCount { get; set; }

        public LoadStatisticsCommand(Item currentItem, bool reccursive)
            : base(currentItem)
        {
            IsReccursive = reccursive;
            TotalCount = new Dictionary<Language, int>();
            ExistingCount = new Dictionary<Language, int>();
        }

        public override void Evaluate()
        {
            TotalCount.Clear();
            ExistingCount.Clear();

            Thread.Sleep(1000);

            Logger.Info(string.Format("Start reloading statistics for item '{0}'{1}", CurrentItem.Paths.FullPath, IsReccursive ? " reccursively": ""), this);

            foreach (var language in VersionService.GetLanguages(CurrentItem.Database))
            {
                TotalCount.Add(language, 0);
                ExistingCount.Add(language, 0);

                LoadStatistics(CurrentItem.Database.GetItem(CurrentItem.ID, language));
            }

            Logger.Info(string.Format("Statistics for item '{0}'{1} has been reloaded.", CurrentItem.Paths.FullPath, IsReccursive ? " reccursively" : ""), this);
        }

        public void LoadStatistics(Item currentItem)
        {
            TotalCount[currentItem.Language] += 1;
            if (currentItem.Versions.GetVersions().Length > 0)
            {
                ExistingCount[currentItem.Language] += 1;
            }

            Statistics.StatisticsChange(currentItem.Language, currentItem.ID.Guid, TotalCount[currentItem.Language], ExistingCount[currentItem.Language]);

            if (IsReccursive)
            {
                foreach (Item child in currentItem.Children)
                {
                    LoadStatistics(child);
                }
            }
        }
    }
}