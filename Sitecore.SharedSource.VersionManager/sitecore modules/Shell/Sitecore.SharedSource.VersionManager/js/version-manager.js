VersionManager = function() {
	this.serviceUrl = "/sitecore modules/Shell/Sitecore.SharedSource.VersionManager/Services/VMService.asmx";
	this.loggerHub = jQuery.connection.loggerHub;
	this.statisticsHub = jQuery.connection.statisticsHub;
	this.lockHub = jQuery.connection.lockHub;
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
	this.toOptions = this.versionManager.find("#toOptions");
	this.refresh = this.versionManager.find("#reload-statistics");
	this.clearButtons = this.versionManager.find(".clear-lang");
	this.processButton = this.versionManager.find(".process");
	
    this.init(this);
};

VersionManagerMethods = {
	_this: null,

	init: function(manager) {
		_this = manager;
		// Subscribe SignalR listeners...
		_this.loggerHub.client.logMessage = function (msg) { _this.logMessage(msg); };
		_this.lockHub.client.lockUi = function (itemId, lock) { _this.lockUi(itemId, lock); };
		_this.statisticsHub.client.statisticsChange = function (language, itemId, percent, existing, total) {
			_this.statisticsChange(language, itemId, percent, existing, total);
		};
		// Start listening to signalR hub.
		jQuery.connection.hub.start();

		_this.from.each(function() {
		    jQuery(this).bind("click", function () { _this.uncheckTo(jQuery(this)); });
		});
		
		_this.toOptions.bind("click", function () { _this.inverseTo(); });
		
		_this.unlockUi();
		
		_this.logMessage("Version manager module has been initialized.");
	},

	logMessage: function(msg) {
		_this.log.append(msg + "\n");
		_this.log.scrollTop(this.log[0].scrollHeight - this.log.height());
	},
	
	statisticsChange: function (language, itemId, percent, existing, total) {
	    var languageRow = _this.versionManager.find("#" + language + "_" + itemId);

	    languageRow.find(".progressbar div").attr("style", "width:" + percent.toFixed(2) + "%;");
	    languageRow.find(".percent_number").html("(" + percent.toFixed(1) + "%)");
	    languageRow.find(".items-processed .existing").html(existing);
	    languageRow.find(".items-processed .total").html(total);
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
		var to = _this.from.filter(":checked").val();

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
	
    inverseTo : function() {
        var checked = false;

        _this.to.each(function () {
            if (!jQuery(this).parent().parent().parent().find("input[name=from]").filter(":checked").val() != "") {
                if (jQuery(this).is(":checked")) {
                    checked = true;
                }
            }
        });
        
        _this.to.each(function () {
            if (!jQuery(this).parent().parent().parent().find("input[name=from]").filter(":checked").val() != "") {
                if (checked) {
                    jQuery(this).removeAttr("checked");
                } else {
                    jQuery(this).attr("checked", "checked");
                }
            } else {
                jQuery(this).removeAttr("checked");
            }
        });
    },
	
    uncheckTo : function(from) {
        var fromCorrTo = from.parent().parent().parent().find("input[name=to]");
        fromCorrTo.removeAttr("checked");
        _this.to.each(function() {
            jQuery(this).removeAttr("disabled");
        });
        fromCorrTo.attr("disabled", "disabled");
    },
	
    lockUi: function (itemId, lock) {
	    if (itemId == _this.itemId) {
		    if (lock) {
			    _this.clearButtons.each(function() {
				    jQuery(this).addClass("lock");
				    jQuery(this).unbind("click");
			    });
			    _this.processButton.unbind("click");
			    _this.reccursive.unbind("change");
			    _this.reccursive.attr("disabled", "disabled");
			    _this.refresh.unbind("click");
			    _this.reccursive.addClass("lock");
			    _this.processButton.addClass("lock");
			    _this.refresh.addClass("lock");
		    } else {
		        _this.clearButtons.each(function () {
		            jQuery(this).bind("click", function () { _this.removeItemVersions(jQuery(this)); });
		            jQuery(this).removeClass("lock");
		        });
		        _this.reccursive.bind("change", function () { _this.reloadStatistics(); });
			    _this.refresh.bind("click", function() { _this.reloadStatistics(); });
			    _this.processButton.bind("click", function() { _this.processCopyVersions(); });
			    _this.reccursive.removeAttr("disabled");
			    _this.reccursive.removeClass("lock");
			    _this.refresh.removeClass("lock");
			    _this.processButton.removeClass("lock");
		    }
	    }
    },
	
	unlockUi: function() {
		_this.lockUi(_this.itemId, false);
	},
};

VersionManager.prototype = VersionManagerMethods;