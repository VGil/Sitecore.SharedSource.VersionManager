using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
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

		protected override string LogFormat
		{
			get { return "From: '{0}', To: '{1}', Exact: '{2}', Override: '{3}'"; }
		}

		protected override object[] LogParameters
		{
			get { return new object[] { From, string.Join(", ", To.Select(x => x.ToString())), Exact, Override }; }
		}

        protected override void Evaluate(Item currentItem)
        {
            var targetItem = currentItem.Versions.GetLatestVersion();
            var initialItem = currentItem.Database.GetItem(currentItem.ID, From);

            if (Exact)
            {
				foreach (var initialVersion in initialItem.Versions.GetVersions())
				{
					var targetVersion = initialVersion.Database.GetItem(initialVersion.ID, From, initialVersion.Version);
				}
            }
            else
            {
                initialItem = initialItem.Versions.GetLatestVersion();
                if (initialItem.Versions.Count > 0)
                {
	                var isNew = false;
                    if (targetItem.Versions.GetVersions().Length == 0)
                    {
                        targetItem = targetItem.Versions.AddVersion();
	                    isNew = true;
                    }

					CopyFields(initialItem, targetItem, Override || isNew);
                }
            }
        }

        private static void CopyFields(BaseItem initialVersion, Item targetVersion, bool @override)
        {
	        var fields = GetItemFields(initialVersion);

	        using (new EditContext(targetVersion))
            {
                foreach (var sourceField in fields)
                {
					if (@override || targetVersion[sourceField.ID] == string.Empty)
	                {
		                targetVersion.Fields[sourceField.ID].SetValue(sourceField.Value, false);
	                }
                }
            }
        }

	    private static IEnumerable<Field> GetItemFields(BaseItem initialVersion)
	    {
			return initialVersion.Fields
				.Where(x => 
					!string.IsNullOrEmpty(x.Name) && 
					(!x.Name.StartsWith("__") || _requiredSystemFieldsToCopy.Contains(x.Name)));
	    }

	    protected static IEnumerable<string> _requiredSystemFieldsToCopy = new List<string>
        {
            "__Valid from", 
            "__Valid to", 
            "__Workflow state"                                                    
        };
    }
}