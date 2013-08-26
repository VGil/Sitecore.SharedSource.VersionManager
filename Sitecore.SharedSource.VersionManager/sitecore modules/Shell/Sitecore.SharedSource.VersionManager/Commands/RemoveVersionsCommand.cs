using Sitecore.Data.Items;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public class RemoveVersionsCommand : StatisticsBasedCommand
    {
        public RemoveVersionsCommand(Item currentItem, int totalItemCount)
            : base(currentItem, totalItemCount)
        {
        }
    }
}