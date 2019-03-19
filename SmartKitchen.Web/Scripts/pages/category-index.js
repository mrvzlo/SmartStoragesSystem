// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
// ReSharper disable CoercedEqualsUsing
var CategoryIndexJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            removeUrl: null,
            list: [],
            category: null
        };
        settings = $.extend(true, defaults, options);

        $(function () {
            new MvcGrid(document.querySelector('.mvc-grid')).reload();
        });
    }

    var remove = function (id) {
        settings.category = id;
        loadNames(id);
        $("#CategoryModalLabel").text("Choose the category which will replace " + $("#name_" + id).text());
    }

    var removeConfirm = function (id) {
        var url = settings.removeUrl + settings.category + "&toId=" + id;
        $.post(url,
            function (success) {
                if (success) location.reload();
            });
    }

    var loadNames = function (id) {
        settings.list.forEach(function (elId) {
            var element = "#modal_" + elId;
            if (id != elId) {
                $(element).text($("#name_"+elId).text());
                $(element).show();
            }
            else $(element).hide();
        });
    }

    return {
        initialize: initialize,
        remove: remove,
        removeConfirm: removeConfirm
    };

})();