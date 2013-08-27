using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public class RemoveVersionsCommand : AbstractCommand
    {
        public Language Language { get; private set; }

        public RemoveVersionsCommand(Item currentItem, Language language, bool reccursive)
            : base(currentItem, reccursive)
        {
            Language = language;
        }

        public override System.Collections.Generic.IEnumerable<Language> Languages
        {
            get { return new[] {Language}; }
        }

        protected override void Evaluate(Item currentItem)
        {
            if (currentItem.Versions.Count > 0)
            {
                currentItem.Versions.RemoveAll(false);
                ExistingCount[Language] -= 1;
            }
        }

        protected override bool RelativeStatistics
        {
            get { return true; }
        }
    }
}