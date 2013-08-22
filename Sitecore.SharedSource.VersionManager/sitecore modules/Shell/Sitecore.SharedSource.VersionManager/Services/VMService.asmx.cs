using System;
using System.Web.Services;
using Sitecore.SharedSource.VersionManager.SitecoreEditor;

namespace Sitecore.SharedSource.VersionManager.Services
{
    /// <summary>
    /// Summary description for VMService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VMService : WebService
    {
        [WebMethod]
        public void Process(string id, string language, string database, string from, string to, bool reccursive, bool @override, bool exact)
        {
            var context = new SitecoreEditorContext(id, language, database);
            var manager = new VersionService(context);
            manager.Process(from, to.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries), reccursive, @override, exact);
        }

        [WebMethod]
        public void Stats(string id, string language, string database, bool reccursive)
        {
            var context = new SitecoreEditorContext(id, language, database);
            var manager = new VersionService(context);
            manager.GetItemStatistics(reccursive);
        }
    }
}
