// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
// ReSharper disable CoercedEqualsUsing
var StorageViewJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            sendUrl: null,
            storageId: 0
        };
        new MvcGrid(document.querySelector(".mvc-grid")).reload();
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnMark0", function () { markGroup(0) });
        $(document).on("click", "#btnMark1", function () { markGroup(1) });
        $(document).on("click", "#btnMark2", function () { markGroup(2) });
        $(document).on("click", "#btnMark3", function () { markGroup(3) });
        $(document).on("change", "#basket", basketNameInputUpd);
        $(document).on("click", "#sendToBasket", sendToBasket);
        $(document).on("click", "#btnMark", mark);
        basketNameInputUpd();
    };

    var sendToBasket = function () {
        $.post({
            url: settings.sendUrl,
            data: {
                storage: settings.storageId,
                basket: $("#basket").val(),
                name: $("#basketName").val(),
                cells: arrayOfCell()
            },
            success: function(data) {
                if (data.success) window.location = data.url;
                else $("#toBasketError").text(data.error);
            }
        });
    }

    var arrayOfCell = function() {
        var array = [];
        var table = $(".table")[0];
        for (var i = 1; i < table.rows.length; i++) {
            var row = table.rows[i];
            var id = row.id;
            if (id > 0) {
                var name = $("#name_" + id);
                var marked = name.hasClass("font-weight-bold");
                if (marked) array.push(id);
            }
        }
        return array;
    }

    var basketNameInputUpd = function () {
        var selected = $("#basket option:selected").val();
        if (selected != 0) $("#basketBox").hide();
        else $("#basketBox").show();
    }

    var mark = function () {
        var id = StatusPickerJs.getProductId();
        var e = $("#name_" + id);
        if (e.hasClass("font-weight-bold"))
            e.removeClass("font-weight-bold");
        else e.addClass("font-weight-bold");
    }

    var markGroup = function (type) {
        var table = $(".table")[0];
        for (var i = 1; i < table.rows.length; i++) {
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