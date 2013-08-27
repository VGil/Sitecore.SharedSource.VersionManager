using System;
using System.Linq;
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
            return ExecuteCommand(() => delegate
            {
                var context = new SitecoreEditorContext(id, database);

                var command = new CopyCommand(
                    context.Item,
                    LanguageManager.GetLanguage(from),
                    to.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(LanguageManager.GetLanguage),
                    reccursive,
                    exact,
                    @override);

                command.Execute();
            });
        }

        [WebMethod]
		public ServiceCallResult Stats(string id, string database, bool reccursive)
        {
            return ExecuteCommand(() => delegate
            {
                var context = new SitecoreEditorContext(id, database);
                var command = new LoadStatisticsCommand(context.Item, reccursive);

                command.Execute();
            });
        }

        [WebMethod]
		public ServiceCallResult Clear(string id, string language, string database, bool reccursive)
        {
            return ExecuteCommand(() => delegate
            {
                var context = new SitecoreEditorContext(id, database);

                var command = new RemoveVersionsCommand(
                    context.Item,
                    LanguageManager.GetLanguage(language),
                    reccursive);

                command.Execute();
            });
        }

        private ServiceCallResult ExecuteCommand(Func<Action> func)
        {
            try
            {
                func().Invoke();

                return new ServiceCallResult { Success = true };
            }
            catch (Exception ex)
            {
                Logger.Error("Service method call error.", ex, this);
                return new ServiceCallResult { Success = false, Error = ex.Message };
            }
        }
    }
}
