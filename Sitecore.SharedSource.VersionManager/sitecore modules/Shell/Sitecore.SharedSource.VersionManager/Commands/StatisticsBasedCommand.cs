using Sitecore.Data.Items;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public abstract class StatisticsBasedCommand : AbstractCommand
    {
        public int TotalItemCount { get; private set; }

        protected StatisticsBasedCommand(Item currentItem, int totalItemCount)
            : base(currentItem)
        {
            TotalItemCount = totalItemCount;
        }
    }
}