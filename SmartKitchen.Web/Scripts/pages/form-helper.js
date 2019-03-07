var FormHelperJs = (function () {

    var bindForm = function (formId, containerId) {
        var frm = $(formId);
        frm.off("submit");
        frm.ajaxform({
            complete: function(data) {
                if (data.success) window.location = data.url;
                else {
                    $(containerId).html(data.formHTML).promise().done(function() {
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