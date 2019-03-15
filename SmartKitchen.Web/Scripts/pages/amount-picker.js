// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var AmountPickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            productId: null,
            deleteUrl: null,
            setUrl: null
        };

        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnAmountRemove", removeProduct);
        $(document).on("click", "#btnAmountSave", changeProductAmount);
        $(document).on("click", "#btnAmountPlus", function () { plusProductAmount(1)});
        $(document).on("click", "#btnAmountMinus", function () { plusProductAmount(-1) });
    };

    var plusProductAmount = function (i) {
        var amount = $("#AmountValue");
        if (Number(amount.val()) + i < amount.attr("min")) return;
        var value = (Number(amount.val()) + i).toFixed(3);
        amount.val(value);
    }

    var changeProductAmount = function () {
        var amount = $("#AmountValue").val()*1000;
        var url = settings.setUrl + settings.productId + "&amount=" + amount;
        $.post(url,
            function () {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            });
    }

    var removeProduct = function() {
        var name = $("#name_" + settings.productId).text();
        if (confirm(name + " will be completely removed")) {
            var url = settings.deleteUrl + settings.productId;
            $.post(url,
                function () {
                    new MvcGrid(document.querySelector(".mvc-grid")).reload();
                });
        }
    }

    var showAmountPicker = function(product, amount) {
        settings.productId = product;
        amount /= 1000;
        var name = $("#name_" + product).text();
        $('#AmountModalName').text("Choose amount of " + name + " in kg");
        $("#AmountValue").val(amount);
    }

    return {
        initialize: initialize,
        showAmountPicker: showAmountPicker,
        changeProductAmount: changeProductAmount
    };
})();