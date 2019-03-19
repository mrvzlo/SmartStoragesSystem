// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var PricePickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            productId: null,
            setUrl: null,
            measurement: null
        };

        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnPriceSave", changeProductPrice);
        $(document).on("click", "#btnPricePlus", function () { plusProductPrice(1)});
        $(document).on("click", "#btnPriceMinus", function () { plusProductPrice(-1) });
    };

    var plusProductPrice = function (i) {
        var price = $("#PriceValue");
        if (Number(price.val()) + i < price.attr("min")) return;
        var value = (Number(price.val()) + i).toFixed(2);
        price.val(value);
    }

    var changeProductPrice = function () {
        var price = $("#PriceValue").val();
        var url = settings.setUrl + settings.productId + "&price=" + price;
        $.post(url,
            function () {
                new MvcGrid(document.querySelector(".mvc-grid")).reload();
            });
    }

    var showPricePicker = function(product, price) {
        settings.productId = product;
        var name = $("#name_" + product).text();
        $('#PriceModalName').text("Choose the price for " + name + " in " + settings.measurement);
        $("#PriceValue").val(price);
    }

    return {
        initialize: initialize,
        showPricePicker: showPricePicker,
        changeProductPrice: changeProductPrice
    };
})();