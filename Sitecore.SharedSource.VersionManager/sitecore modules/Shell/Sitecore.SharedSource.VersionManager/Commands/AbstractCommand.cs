using Sitecore.Data.Items;
using Sitecore.SharedSource.VersionManager.Hubs;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public abstract class AbstractCommand
    {
        public Item CurrentItem { get; private set; }

        protected AbstractCommand(Item currentItem)
        {
            CurrentItem = currentItem;
        }

        public virtual void Evaluate()
        {
            Logger.Info(string.Format("Method {0}.Evaluate() is not implemented.", GetType().Name), this);
        }
    }
}