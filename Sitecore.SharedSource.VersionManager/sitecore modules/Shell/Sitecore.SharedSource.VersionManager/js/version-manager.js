﻿VersionManager = function() {
	this.serviceUrl = "/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/Services/VMService.asmx";
	this.hub = jQuery.connection.versionManagerHub;
	this.versionManager = jQuery(".sitecore-version-manager");
	this.log = this.versionManager.find("#log-control");
	this.itemId = this.versionManager.find("input[name=itemId]").val();
	this.language = this.versionManager.find("input[name=language]").val();
	this.version = this.versionManager.find("input[name=version]").val();
	this.database = this.versionManager.find("input[name=database]").val();
	this.reccursive = this.versionManager.find("input[name=reccursive]");
	this.override = this.versionManager.find("input[name=override]");
	this.exact = this.versionManager.find("input[name=exact]");
	this.from = this.versionManager.find("input[name=from]");
	this.to = this.versionManager.find("input[name=to]");
	
    this.init(this);
};

VersionManagerMethods = {
	_this: null,

	init: function(manager) {
		_this = manager;
		// Subscribe SignalR listeners...
		_this.hub.client.logMessage = function(msg) { _this.logMessage(msg); };
		_this.hub.client.statisticsChange = function(language, itemId, percent) {
			 _this.statisticsChange(language, itemId, percent);
		};
		// Start listening to signalR hub.
		jQuery.connection.hub.start();

		// Reload stats on reccursive change

		// Initialize button clicks.
		_this.versionManager.find(".clear-lang").each(function() {
			jQuery(this).bind("click", function() { _this.removeItemVersions(jQuery(this)); });
		});
		_this.versionManager.find(".process").each(function() {
			jQuery(this).bind("click", function() { _this.processCopyVersions(); });
		});

		_this.logMessage("Version manager module has been initialized.");
	},

	logMessage: function(msg) {
		_this.log.append(msg + "\n");
		_this.log.scrollTop(this.log[0].scrollHeight - this.log.height());
	},
	
	statisticsChange: function(language, itemId, percent) {
		_this.versionManager.find("#" + language + "_" + itemId)
			.find(".progressbar div").attr("style", "width:" + percent + "%;");
		
		_this.versionManager.find("#" + language + "_" + itemId)
			.find(".percent_number").html("(" + percent + "%)");
	},

	postToWebService: function(serviceMethod, parameters) {
		jQuery.ajax({
			type: "POST",
			url: _this.serviceUrl + "/" + serviceMethod,
			data: parameters,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function(result) {
				if (!result.d.Success) {
					_this.logMessage("POST to webservice error. " + result.d.Error + ". Parameters: " + parameters);
				}
			},
			error: function(xmlHttpRequest, textStatus, errorThrown) {
				_this.logMessage("POST to webservice" + " | " + textStatus + " | " + errorThrown + ". Parameters: " + parameters);
			}
		});
	},

	removeItemVersions: function(button) {
		var recc = "";
		if (_this.reccursive.is(":checked")) {
			recc = " including all subitems reccursively";
		}
		var confirmMessage = "You are going to remove all versions of item {" + _this.itemId + "} in '" + button.attr("id") + "' language" + recc + ". Are you sure?";

		if (confirm(confirmMessage)) {
			var sendInfo = JSON.stringify({
				"id": _this.itemId,
				"language": button.attr("id"),
				"database": _this.database,
				"reccursive": _this.reccursive.is(":checked")
			});

			_this.postToWebService("Clear", sendInfo);
		}
	},

	reloadStatistics: function() {
		var sendInfo = JSON.stringify({
			"id": _this.itemId,
			"database": _this.database,
			"reccursive": _this.reccursive.is(":checked")
		});

		_this.postToWebService("Stats", sendInfo);
	},

	processCopyVersions: function() {
		// Process(string id, string database, string from, string to, bool reccursive, bool @override, bool exact)
		var sendInfo = JSON.stringify({
			"id": _this.itemId,
			"database": _this.database,
			"from": _this.getCopyFrom(),
			"to": _this.getCopyTo(),
			"reccursive": _this.reccursive.is(":checked"),
			"override": _this.override.is(":checked"),
			"exact": _this.exact.is(":checked"),
		});

		_this.postToWebService("Process", sendInfo);
	},

	getCopyFrom: function() {
		var to = _this.from.filter(':checked').val();

		if (to == "") {
			alert("Unable to find language to copy from. Make sure you habve selected it.");
		}

		return to;
	},
	
	getCopyTo: function() {
		var result = "";

		_this.to.each(function () {
			if (jQuery(this).is(":checked")) {
				if (result != "") {
					result += ",";
				}
				result += jQuery(this).val();
			}
		});

		if (result == "") {
			alert("Unable to find language to copy to. Make sure you habve selected at least one of them.");
		}

		return result;
	},
};

VersionManager.prototype = VersionManagerMethods;