using Sitecore.Data.Items;

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

		protected override bool RelativeStatistics
		{
			get { return true; }
		}
    }
}