// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var BasketIndexJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            lockUrl: null, 
            removeUrl: null
        };
        settings = $.extend(true, defaults, options);
    }

    var lock = function(id, name) {
        event.stopPropagation();
        if (confirm(name + " will be locked but products will not be send to storages")) {
            var url = settings.lockUrl + id;
            $.post(url,
                function (data) {
                    $("#" + id).remove();
                    $("#tab2").prepend(data);
                });
        }
    }

    var remove = function(id, name) {
        event.stopPropagation();
        if (confirm(name + " will be removed")) {
            var url = settings.removeUrl + id;
            $.post(url,
                function (success) {
                    if (success) document.getElementById(id).outerHTML = "";
                });
        }
    }

    return {
        lock: lock,
        remove: remove,
        initialize: initialize
    };

})();