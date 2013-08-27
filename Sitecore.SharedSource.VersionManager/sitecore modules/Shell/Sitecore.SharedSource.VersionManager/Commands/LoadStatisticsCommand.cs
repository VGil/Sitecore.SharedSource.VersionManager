﻿using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Hubs;

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

        protected override void Evaluate()
        {
			TotalCount.Clear();
            ExistingCount.Clear();

            Logger.Info(string.Format("Start reloading statistics for item '{0}'{1}", CurrentItem.Paths.FullPath, IsReccursive ? " reccursively": ""), this);

            foreach (var language in Languages)
            {
                TotalCount.Add(language, 0);
                ExistingCount.Add(language, 0);

                LoadStatistics(CurrentItem.Database.GetItem(CurrentItem.ID, language));
            }

			Logger.Info(string.Format("Statistics for item '{0}' has been reloaded{1}.", CurrentItem.Paths.FullPath, IsReccursive ? " reccursively" : ""), this);
        }

        public void LoadStatistics(Item currentItem)
        {
            TotalCount[currentItem.Language] += 1;
            if (currentItem.Versions.GetVersions().Length > 0)
            {
                ExistingCount[currentItem.Language] += 1;
            }

            Statistics.StatisticsChange(
                currentItem.Language, 
                CurrentItem.ID.Guid, 
                TotalCount[currentItem.Language], 
                ExistingCount[currentItem.Language]);

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