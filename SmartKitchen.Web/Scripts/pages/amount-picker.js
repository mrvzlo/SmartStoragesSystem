// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var AmountPickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            cellId: null,
            deleteUrl: null,
            setUrl: null
        };

        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnAmountRemove", removeProduct);
    };

    var changeProductAmount = function(amount) {
        var url = settings.setUrl + settings.cellId + "&amount=" + amount;
        $.post(url,
            function () {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            });
    }

    var removeProduct = function() {
        var name = $("#name_" + settings.cellId).text();
        if (confirm(name + " cell will be completely removed")) {
            var url = settings.deleteUrl + settings.cellId;
            $.post(url,
                function () {
                    new MvcGrid(document.querySelector(".mvc-grid")).reload();
                });
        }
    }

    var showAmountPicker = function(product) {
        settings.cellId = product;
        var name = $("#name_" + product).text();
        $('#AmountModalLabel').text("Choose amount of " + name);
    }

    return {
        initialize: initialize,
        showAmountPicker: showAmountPicker,
        changeProductAmount: changeProductAmount
    };
})();