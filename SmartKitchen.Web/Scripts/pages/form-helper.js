var FormHelperJs = (function () {

    var bindForm = function (formId, containerId) {
        var frm = $(formId);
        frm.off("submit");
        frm.on("submit", function () {
            if (!$(this).valid()) return false;
            var url = frm.attr("action");
            $.post(url,
                frm.serialize(),
                function (data) {
                    if (data.success) window.location = data.url;
                    else {
                        $(containerId).html(data.formHTML).promise().done(function () {
                            bindForm(formId, containerId);
                        });
                    }
                });
            return false;
        });
    };

    return {
        bindForm: bindForm
    };

})();