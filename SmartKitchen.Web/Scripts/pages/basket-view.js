// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var BasketViewJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            updUrl: null,
            removeUrl: null,
            finishUrl: null
        };
        new MvcGrid(document.querySelector(".mvc-grid")).reload();
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnFinish", finish);
        $(document).on("click", "#btnMark", markBought);
    }
    
    var markBought = function () {
        var id = StatusPickerJs.getProductId();
        var status = !$("#name_" + id).hasClass("bought");
        $.post({
            url: settings.updUrl + "?id=" + id+"&status="+status,
            success: function () {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            }
        });
    }
    
    var finish = function () {
        var name = $("#basketName").text();
        if (confirm(name + " will be locked and products will be send to storages")) {
            var url = settings.finishUrl;
            $.post(url, function () {
                window.location = location;
            });
        }
    }

    return {
        initialize: initialize,
        markBought: markBought
    };

})();