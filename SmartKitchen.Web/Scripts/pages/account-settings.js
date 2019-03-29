// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var AccountSettingsJs = (function () {

    var settings = {};
    var initialize = function (options) {
        var defaults = {
            updUrl: null
        };
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#updToken", updKeys);
        $(document).on("click", "#copyPublic", copy);
        FormHelperJs.bindForm("#passResetForm", "#pass");
    }
    
    var updKeys = function() {
	    $.post(url,
		    function (data) {
			    $("#publicKey").text(data);
		    });
    }

    var copy = function() {
        var $temp = $("<input>");
	    $("body").append($temp);
	    $temp.val($("#publicKey").text()).select();
	    document.execCommand("copy");
	    $temp.remove();
    }

    return {
        initialize: initialize
    };

})();