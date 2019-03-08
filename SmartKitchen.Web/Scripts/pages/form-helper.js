var FormHelperJs = (function () {

    var bindForm = function (formId, containerId) {
        var form = $(formId);
        form.off("submit");
        form.ajaxForm({
            complete: function (data) {
                var variables = data.responseJSON;
                if (variables.success)
                    window.location = variables.url;
                else {
                    $(containerId).html(variables.formHTML).promise().done(function() {
                        bindForm(formId, containerId);
                    });
                }
            }
        });
};

    return {
        bindForm: bindForm
    };

})();