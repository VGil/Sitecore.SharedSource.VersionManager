using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Sitecore.SharedSource.VersionManager.Commands
{
    public class CopyCommand : AbstractCommand
    {
        public bool Exact { get; private set; }
        public bool Override { get; private set; }
        public Language From { get; private set; }
        public IEnumerable<Language> To { get; private set; }

        public CopyCommand(Item currentItem, Language from, IEnumerable<Language> to, bool reccursive, bool exact, bool @override)
            : base(currentItem, reccursive)
        {
            From = from;
            To = to;
            Exact = exact;
            Override = @override;
        }

        public override IEnumerable<Language> Languages
        {
            get { return To; }
        }

        protected override void Evaluate(Item currentItem)
        {
            var targetItem = currentItem.Versions.GetLatestVersion();
            var initialItem = currentItem.Database.GetItem(currentItem.ID, From);

            if (Override)
            {
            }
            else
            {
            }

            if (Exact)
            {
            }
            else
            {
                initialItem = initialItem.Versions.GetLatestVersion();
                if (initialItem.Versions.Count > 0)
                {
                    if (targetItem.Versions.GetVersions().Length == 0)
                    {
                        targetItem = targetItem.Versions.AddVersion();
                    }

                    CopyFields(initialItem, targetItem);
                }
            }
        }

        private static void CopyFields(BaseItem initialVersion, Item targetVersion)
        {
            var sourceFields = initialVersion.Fields
            .Where(x => !string.IsNullOrEmpty(x.Name) && (!x.Name.StartsWith("__") || _requiredSystemFieldsToCopy.Contains(x.Name)))
            .ToList();

            using (new EditContext(targetVersion))
            {
                foreach (var sourceField in sourceFields)
                {
                    targetVersion.Fields[sourceField.Name].SetValue(sourceField.Value, false);
                }
            }
        }

        protected static IEnumerable<string> _requiredSystemFieldsToCopy = new List<string>
        {
            "__Valid from", 
            "__Valid to", 
            "__Workflow state"                                                    
        };
    }
}