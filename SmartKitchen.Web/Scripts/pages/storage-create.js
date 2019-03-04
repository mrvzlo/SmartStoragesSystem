// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StorageCreateJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            first: 0
        };
        settings = $.extend(true, defaults, options);
        change(settings.first);
    };
    
    var change = function(id) {
        $("#Name").val($("#name_" + id).val());
        $("#TypeId").val(id);
        $("form").css("background", $("#s_" + id).css("background-color"));
    }

    return {
        initialize: initialize,
        change: change
    };
})();