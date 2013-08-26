using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public abstract class CopyCommand : StatisticsBasedCommand
    {
        public bool IsExact { get; private set; }
        public bool IsOverride { get; private set; }
        public Language LanguageCopyTo { get; private set; }

        private CopyCommand(Item currentItem, int totalItemCount)
            : base(currentItem, totalItemCount)
        {
        }

        protected CopyCommand(Item currentItem, int totalItemCount, Language languageCopyTo, bool exact, bool @override)
            : this(currentItem, totalItemCount)
        {
            LanguageCopyTo = languageCopyTo;
            IsExact = exact;
            IsOverride = @override;
        }
    }
}