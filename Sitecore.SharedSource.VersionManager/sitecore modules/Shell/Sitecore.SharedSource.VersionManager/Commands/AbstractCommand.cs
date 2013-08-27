using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Hubs;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public abstract class AbstractCommand
    {
        public IDictionary<Language, int> TotalCount { get; private set; }
        public Dictionary<Language, int> ExistingCount { get; private set; }

        public bool Reccursive { get; private set; }
        public Item CurrentItem { get; private set; }

        protected AbstractCommand(Item currentItem, bool reccursive)
        {
            CurrentItem = currentItem;
            Reccursive = reccursive;

            TotalCount = new Dictionary<Language, int>();
            ExistingCount = new Dictionary<Language, int>();

            foreach (var language in GetLanguages(CurrentItem.Database))
            {
                TotalCount.Add(language, 0);
                ExistingCount.Add(language, 0);
            }
        }

        protected virtual bool RelativeStatistics { get { return false; } }

        public float GetStatistics(Language language)
        {
            return 100*(float) ExistingCount[language] / TotalCount[language];
        }

        public virtual IEnumerable<Language> Languages
        {
            get { return GetLanguages(CurrentItem.Database); }
        }

        public void Execute()
	    {
            try
            {
                TotalCount.Clear();
                ExistingCount.Clear();

                Locker.LockUi(CurrentItem.ID.Guid);

                Logger.Info(string.Format(
                        "{0} start for item '{1}'{2}",
                        GetType().Name,
                        CurrentItem.Paths.FullPath,
                        Reccursive ? " reccursively" : ""),
                    this);

                foreach (var language in Languages)
                {
                    TotalCount.Add(language, 0);
                    ExistingCount.Add(language, 0);

                    var currentItem = CurrentItem.Database.GetItem(CurrentItem.ID, language);

                    CalculateTotal(currentItem);
                    EvaluateReccursive(currentItem);
                }

                Logger.Info(string.Format(
                        "{0} finish for item '{1}' {2}.",
                        GetType().Name,
                        CurrentItem.Paths.FullPath,
                        Reccursive ? " reccursively" : ""),
                    this);

                Locker.UnlockUi(CurrentItem.ID.Guid);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0}.Execute() Error.", GetType().Name), ex, this);
            }
	    }

        private void EvaluateReccursive(Item currentItem)
        {
            if (!RelativeStatistics)
            {
                TotalCount[currentItem.Language] += 1;
            }
           
            Evaluate(currentItem);

            if (!RelativeStatistics)
            {
                if (currentItem.Versions.GetVersions().Length > 0)
                {
                    ExistingCount[currentItem.Language] += 1;
                }
            }

            UpdateStatistics(currentItem);

            if (Reccursive)
            {
                foreach (Item child in currentItem.Children)
                {
                    EvaluateReccursive(child);
                }
            }
        }

        private void CalculateTotal(Item currentItem)
        {
            if (RelativeStatistics)
            {
                TotalCount[currentItem.Language] += 1;

                if (currentItem.Versions.GetVersions().Length > 0)
                {
                    ExistingCount[currentItem.Language] += 1;
                }

                if (Reccursive)
                {
                    foreach (Item child in currentItem.Children)
                    {
                        CalculateTotal(child);
                    }
                }
            }
        }

        private void UpdateStatistics(Item currentItem)
        {
            Statistics.StatisticsChange(
                currentItem.Language,
                CurrentItem.ID.Guid,
                GetStatistics(currentItem.Language),
                TotalCount[currentItem.Language],
                ExistingCount[currentItem.Language]);
        }

        protected virtual void Evaluate(Item currentItem)
        {
            Logger.Info(string.Format("Method {0}.Evaluate() is not implemented.", GetType().Name), this);
        }

        private readonly IDictionary<Database, IList<Language>> _languageDictionary = new Dictionary<Database, IList<Language>>();

        private IEnumerable<Language> GetLanguages(Database database)
        {
            if (!_languageDictionary.ContainsKey(database))
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

                _languageDictionary.Add(database, languages);
            }

            return _languageDictionary[database];
        }
    }
}