// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
// ReSharper disable CoercedEqualsUsing
var NamePickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            productId: null,
            updateUrl: null
        };

        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnNameSave", updateConfirm);
    };

    var updateConfirm = function () {
        var id = NamePickerJs.getProductId();
        var name = $("#NamePicker").val();
        var url = settings.updateUrl + "?name=" + name + "&id=" + id;
        $.post(url,
            function (data) {
                console.log(data);
                if (data == "True") $("#name_" + id).text(name);
            });
    }

    var showNamePicker = function (id) {
        event.preventDefault();
        settings.productId = id;
        var name = $("#name_" + id).text();
        $("#NamePicker").val(name);
        $("#NamePickerLabel").text("Choose new name instead of " + name);
    }

    var getProductId = function() {
        return settings.productId;
    }

    return {
        initialize: initialize,
        showNamePicker: showNamePicker,
        getProductId: getProductId
    };
})();