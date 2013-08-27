using Sitecore.Data.Items;
using Sitecore.SharedSource.VersionManager.Hubs;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public class LoadStatisticsCommand : AbstractCommand
    {
        public LoadStatisticsCommand(Item currentItem, bool reccursive)
            : base(currentItem, reccursive)
        {
        }

        protected override void Evaluate(Item currentItem)
        {
        }
    }
}