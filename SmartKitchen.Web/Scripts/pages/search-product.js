// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var SearchProductJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            inputId: null,
            searchUrl: null
        };
        settings = $.extend(true, defaults, options);
        $(document).on("input", settings.inputId, search);
    }

    var search = function() {
        var text = $(settings.inputId).val();
        var list = $("#products");
        list.empty();
        if (text.length > 0) {
	        $.post({
                url: settings.searchUrl + "?name=" + text,
                success: function (jsonResult) {
	                jsonResult.list.forEach(function(element) {
                        var opt = $("<option>");
                        opt.val(element);
                        list.append(opt);
                    });
                }
	        });
        }
    }

    return {
        initialize: initialize
    };

})();