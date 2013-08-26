using System;
using System.Threading;
using System.Web.Services;
using Sitecore.Data.Managers;
using Sitecore.SharedSource.VersionManager.Commands;
using Sitecore.SharedSource.VersionManager.Hubs;
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
		public ServiceCallResult Process(string id, string database, string from, string to, bool reccursive, bool @override, bool exact)
        {
	        try
	        {
		        var context = new SitecoreEditorContext(id, database);
		        var manager = new VersionService(context);
		        manager.Process(from, to.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries), reccursive, @override, exact);
		        return new ServiceCallResult {Success = true};
	        }
	        catch (Exception ex)
	        {
				Logger.Error("'Process' Service method call error.", ex, this);
		        return new ServiceCallResult {Success = false, Error = ex.Message};
	        }
        }

        [WebMethod]
		public ServiceCallResult Stats(string id, string database, bool reccursive)
        {
	        try
	        {
		        var context = new SitecoreEditorContext(id, database);
                var thread = new Thread(new LoadStatisticsCommand(context.Item, reccursive).Evaluate);
                thread.Start();
		        return new ServiceCallResult {Success = true};
	        }
	        catch (Exception ex)
	        {
		        Logger.Error("'Stats' Service method call error.", ex, this);
		        return new ServiceCallResult {Success = false, Error = ex.Message};
	        }
        }

        [WebMethod]
		public ServiceCallResult Clear(string id, string language, string database, bool reccursive)
        {
	        try
	        {
		        var context = new SitecoreEditorContext(id, language, database);
		        var manager = new VersionService(context);
		        manager.Clear(LanguageManager.GetLanguage(language), reccursive);
		        return new ServiceCallResult {Success = true};
	        }
	        catch (Exception ex)
	        {
		        Logger.Error("'Clear' Service method call error.", ex, this);
		        return new ServiceCallResult {Success = false, Error = ex.Message};
	        }
        }
    }
}
