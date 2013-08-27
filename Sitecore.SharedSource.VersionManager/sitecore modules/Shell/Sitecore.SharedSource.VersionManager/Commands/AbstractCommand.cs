using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SharedSource.VersionManager.Hubs;
using Sitecore.SharedSource.VersionManager.SitecoreEditor;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public abstract class AbstractCommand
    {
        public Item CurrentItem { get; private set; }

        protected AbstractCommand(Item currentItem)
        {
            CurrentItem = currentItem;
        }

        protected virtual void Evaluate()
        {
            Logger.Info(string.Format("Method {0}.Evaluate() is not implemented.", GetType().Name), this);
        }

	    public void Execute()
	    {
			Locker.LockUi(CurrentItem.ID.Guid);

		    Evaluate();

			Locker.UnlockUi(CurrentItem.ID.Guid);
	    }

	    protected IEnumerable<Language> Languages
	    {
			get { return VersionService.GetLanguages(CurrentItem.Database); }
	    }
    }
}