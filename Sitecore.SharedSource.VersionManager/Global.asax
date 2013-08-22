<%@Application Language='C#' Inherits="Sitecore.Web.Application" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">
  public void Application_Start() {
		RouteTable.Routes.MapHubs();
  }

  public void Application_End() {
  }

  public void Application_Error(object sender, EventArgs args) {
  }

</script>
