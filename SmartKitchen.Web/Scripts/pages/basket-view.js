// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var BasketViewJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            updUrl: null,
            removeUrl: null,
            finishUrl: null,
            reopenUrl: null
        };
        new MvcGrid(document.querySelector(".mvc-grid")).reload();
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnFinish", finish);
        $(document).on("click", "#btnReopen", reopen);
        $(document).on("click", "#btnMark", markBought);
    }

    var markBought = function (id) {
        var status = !$("#name_" + id).hasClass("bought");
        $.post({
            url: settings.updUrl + "?id=" + id + "&status=" + status,
            success: function () {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            }
        });
    }

    var finish = function () {
        if (confirm("This basket will be locked and bought products will be send to storages")) {
            var url = settings.finishUrl;
            $.post(url, function () {
                window.location = location;
            });
        }
    }

    var reopen = function () {
        var url = settings.reopenUrl;
        $.post(url, function () {
            window.location = location;
        });
    }

    var removeProduct = function (id, name) {
        if (confirm(name + " will be completely removed")) {
            var url = settings.deleteUrl + id;
            $.post(url,
                function () {
                    new MvcGrid(document.querySelector(".mvc-grid")).reload();
                });
        }
    }

    return {
        initialize: initialize,
        markBought: markBought,
        removeProduct: removeProduct
    };

})();