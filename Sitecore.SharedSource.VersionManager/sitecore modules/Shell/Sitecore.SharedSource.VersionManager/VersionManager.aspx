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
			    var logArea = jQuery("#log-control");
			    logArea.append(msg + "\n");
			    logArea.scrollTop(logArea[0].scrollHeight - logArea.height());
			};

			jQuery.connection.hub.start();
		    jQuery("#log-control").append("Version manager module.\n");
		});
	</script>
	
</head>
<body>
	<div class="sitecore-version-manager">
	    <input type="hidden" name="itemId" value="<%=EditorContext.Id.ToString() %>"/>
	    <input type="hidden" name="language" value="<%=EditorContext.Language.Name %>"/>
	    <input type="hidden" name="version" value="<%=EditorContext.Version.ToString() %>"/>
	    <input type="hidden" name="database" value="<%=EditorContext.Database.Name %>"/>

		<div class="section-capture">Common settings</div>
        <div class="settings">
			<div><input type="checkbox" name="reccursive" value="Reccursive"/> Reccursive</div>
		</div>
        <div class="section-capture">Statistics</div>
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
				<% foreach (var l in Manager.GetItemStatistics(false)){%>
				
					<tr id="<%=l.Name%>_<%=EditorContext.Id.ToString()%>">
						<td><img src="/temp/IconCache/<%=l.Flag%>"/></td>
						<td><div class="lang-name"><%=l.Name%></div></td>
						<td class="percent">
							<div class="progressbar">
								<div style="width:<%=l.Percents.ToString("#0.00", CultureInfo.InvariantCulture)%>%;"></div>
							</div>
						</td>
						<td class="percent_number">(<%=l.Percents.ToString("#0.0", CultureInfo.InvariantCulture) %>%)</td>
						<td><div class="from"><input type="radio" name="from" <%=EditorContext.Language.Name == l.Name ? "checked" : "" %>/></div></td>
						<td><div><input type="checkbox" name="to"/></div></td>
						<td><div class="clear-lang" id="<%=l.Name%>_clear">Clear</div></td>
					</tr>

				<%} %>
			</table>
		</div>
        <div class="section-capture">Copy settings</div>
        <div class="settings">
			<div><input type="checkbox" name="override" value="Override" /> Override existing versions</div>
			<div><input type="checkbox" name="exact" value="Exact" /> Exact copy (copies all existing versions matching numbers, if false - processes latest versions)</div>
			
		</div>
        <div class="section-capture">Process</div>
        <div class="settings">
            <div class="log"><textarea rows="6" readonly="readonly" id="log-control"></textarea></div>
            <div>
                <div class="process right">Process</div>
                <div class="clear"></div>
            </div>
        </div>
	</div>
</body>
</html>
