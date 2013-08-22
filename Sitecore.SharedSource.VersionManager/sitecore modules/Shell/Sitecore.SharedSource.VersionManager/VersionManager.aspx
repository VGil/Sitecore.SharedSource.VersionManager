<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionManager.aspx.cs" Inherits="Sitecore.SharedSource.VersionManager.VersionManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/js/jquery-1.6.4.min.js" type="text/javascript"></script>
	<script src="/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/js/jquery.signalR-1.1.3.min.js" type="text/javascript"></script>
	<script src="/signalr/hubs"></script>
	
	<script type="text/javascript">
		jQuery(function () {
			var logger = jQuery.connection.logHub;

			logger.client.logMessage = function (msg) {
				jQuery("#logUl").append("<li>" + msg + "</li>");
			};

			jQuery.connection.hub.start();
			jQuery("#logUl").append("<li>Log...</li>");
		});
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<ul id="logUl">
			
		</ul>
    </div>
    </form>
</body>
</html>
