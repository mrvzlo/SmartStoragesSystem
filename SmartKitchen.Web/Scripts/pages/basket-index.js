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
        if (confirm(name + " will be locked")) {
            var url = settings.lockUrl + id;
            $.post(url,
                function (data) {
                    document.getElementById(id).outerHTML = "";
                    var a = document.createElement('a');
                    document.getElementById("tab2").appendChild(a);
                    a.outerHTML = data;
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