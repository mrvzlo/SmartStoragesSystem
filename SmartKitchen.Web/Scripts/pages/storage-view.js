// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StorageViewJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            removeUrl: null
        };
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnMark0", function () { markGroup(0) });
        $(document).on("click", "#btnMark1", function () { markGroup(1) });
        $(document).on("click", "#btnMark2", function () { markGroup(2) });
        $(document).on("click", "#btnMark3", function () { markGroup(3) });
    };

    var mark = function (id) {
        var e = $("#name_" + id);
        if (e.hasClass("font-weight-bold"))
            e.removeClass("font-weight-bold");
        else e.addClass("font-weight-bold");
    }

    var markGroup = function (type) {
        var table = $(".table")[0];
        for (var i = 0; i < table.rows.length; i += 1) {
            var row = table.rows[i];
            var id = row.id;
            if (id > 0) {
                var name = $("#name_" + id);
                var marked = name.hasClass("font-weight-bold");
                if (type === 0 && marked) {
                    name.removeClass("font-weight-bold");
                } else if (!marked) {
                    if (type === 1 ||
                        type === 2 && $("#amount_" + id).hasClass("text-secondary") ||
                        type === 3 && $("#safety_" + id).hasClass("text-danger"))
                        name.addClass("font-weight-bold");
                }
            }
        }
    }

    return {
        initialize: initialize,
        mark: mark
};
})();