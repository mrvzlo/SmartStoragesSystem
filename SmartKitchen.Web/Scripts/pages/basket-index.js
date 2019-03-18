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
        remove: remove,
        initialize: initialize
    };

})();