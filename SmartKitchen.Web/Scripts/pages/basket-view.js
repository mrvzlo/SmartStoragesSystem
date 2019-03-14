// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var BasketViewJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            updUrl: null,
            removeUrl: null
        };
        new MvcGrid(document.querySelector(".mvc-grid")).reload();
        settings = $.extend(true, defaults, options);
    }

    var markBought = function(id) {
        $.post({
            url: settings.updUrl + "?id=" + id,
            success: function() {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            }
        });
    }

    return {
        initialize: initialize,
        markBought: markBought,
        remove: remove
    };

})();