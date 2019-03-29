// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StorageIndexJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            removeUrl: null,
            updateUrl: null,
            storageId: 0
        };
        settings = $.extend(true, defaults, options);
    };
    
    var remove = function(name, id) {
        event.preventDefault();
        if (confirm("Storage " + name + " will be removed")) {
            window.location = settings.removeUrl + id;
        }
    }

    return {
        initialize: initialize,
        remove: remove
    };
})();