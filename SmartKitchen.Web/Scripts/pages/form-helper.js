var FormHelperJs = (function () {

    var bindForm = function (formId, containerId) {
        var form = $(formId);
        form.off("submit");

        form.ajaxForm({
	        beforeSubmit: function() {
                $("#loading").show();
                $(formId + " > fieldset").prop("disabled", true);
	        },
            complete: function (data) {
                var variables = data.responseJSON;
                if (variables.success)
                    window.location = variables.url;
                else {
                    $(containerId).html(variables.formHTML).promise().done(function() {
                        bindForm(formId, containerId);
                    });
                }
                $("#loading").hide();
                $(formId + " > fieldset").prop("disabled", false);
            }		
        });
};

    return {
        bindForm: bindForm
    };

})();