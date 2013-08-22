<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionManager.aspx.cs" Inherits="Sitecore.SharedSource.VersionManager.VersionManager" %>
<%@ Import Namespace="System.Globalization" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Version Manager</title>
	<link href="/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/css/version-manager.css" rel="stylesheet"/>
	<script src="/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/js/jquery-1.6.4.min.js" type="text/javascript"></script>
	<script src="/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/js/jquery.signalR-1.1.3.min.js" type="text/javascript"></script>
	<script src="/signalr/hubs"></script>
	
	<script type="text/javascript">
		jQuery(function () {
			var logger = jQuery.connection.versionManagerHub;

			logger.client.logMessage = function (msg) {
				jQuery("#logUl").append("<li>" + msg + "</li>");
			};

			jQuery.connection.hub.start();
			jQuery("#logUl").append("<li>Log...</li>");
		});
	</script>
	
</head>
<body>
	<div class="sitecore-version-manager">
		<div class="settings">
			<div><input type="checkbox" name="reccursive" value="Reccursive"/> Reccursive</div>
			<div><input type="checkbox" name="override" value="Override existing versions" /> Override existing versions</div>
		</div>
		<div class="version-statistic">
			<table>
				<tr>
					<td></td>
					<td></td>
					<td>Filled percent</td>
					<td></td>
					<td>From</td>
					<td>To</td>
					<td></td>
				</tr>
				<% foreach (var l in Languages) {%>
				
					<tr id="<%=l.Name%>_<%=EditorContext.Id.ToShortID()%>">
						<td><img src="/temp/IconCache/<%=l.Flag%>"/></td>
						<td><div class="lang-name"><%=l.Name%></div></td>
						<td class="percent">
							<div class="progressbar">
								<div></div>
							</div>
						</td>
						<td class="percent_number">(<%=l.Percents.ToString("#00.0", CultureInfo.InvariantCulture) %>%)</td>
						<td><input type="radio" name="from" <%=EditorContext.Language.Name == l.Name ? "checked" : "" %>/></td>
						<td><input type="checkbox" name="to"/></td>
						<td><div class="clear-lang" id="<%=l.Name%>_clear">Clear</div></td>
					</tr>

				<%} %>
			</table>
			<div class="process">
				<button>Process...</button>
			</div>
		</div>
		<div class="log">	
			<ul id="logUl"></ul>
		</div>
	</div>
</body>
</html>
