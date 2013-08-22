VersionManager = function() {
    this.hub = jQuery.connection.versionManagerHub;
    this.log = jQuery("#log-control");
    this.itemId = jQuery("input[name=itemId]").val();
    this.language = jQuery("input[name=language]").val();
    this.version = jQuery("input[name=version]").val();
    this.database = jQuery("input[name=database]").val();

    this.isReccursive = function() {
        return jQuery("input[name=reccursive]:checked");
    };
    
    this.isOverride = function () {
        return jQuery("input[name=override]:checked");
    };
    
    this.isExact = function () {
        return jQuery("input[name=exact]:checked");
    };

    this.init();
};

VersionManagerMethods = {

    init: function () {
        this.hub.client.logMessage = function (msg) { this.logMessage(msg); };
        jQuery.connection.hub.start();
    },
    
    logMessage: function (msg) {
        this.log.append(msg + "\n");
        this.log.scrollTop(this.log[0].scrollHeight - this.log.height());
    },
};

VersionManager.prototype = VersionManagerMethods;