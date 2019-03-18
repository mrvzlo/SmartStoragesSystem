// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StatusPickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            productId: null,
            deleteUrl: null
        };

        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnRemove", removeProduct);
    };
    
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

    var showStatusPicker = function(product, status) {
        settings.productId = product;
        var name = $("#name_" + product).text();
        $('#StatusModalName').text(name);
        $("#btnMark").text(status);
    }

    var getProductId = function() {
        return settings.productId;
    }

    return {
        initialize: initialize,
        showStatusPicker: showStatusPicker,
        getProductId: getProductId
    };
})();