// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StorageIndexJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            removeUrl: null,
            updateUrl: null,
            storageId: 0
        };
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#btnNameSave", updateConfirm);
    };

    var updateConfirm = function() {
        var id = settings.storageId;
        var name = $("#StorageName").val();
        var url = settings.updateUrl + "?name=" + name + "&id=" + id;
        $.post(url,
            function (data) {
                console.log(data);
                if (data == "True") $("#name_" + id).text(name);
            });
    }

    var showModal = function (id) {
        event.preventDefault();
        settings.storageId = id;
        $("#StorageName").val($("#name_"+id).text());
        $("#StorageNameLabel").text("Choose new name instead of "+ name);
    }

    var remove = function(name, id) {
        event.preventDefault();
        if (confirm("Storage " + name + " will be removed")) {
            window.location = settings.removeUrl + id;
        }
    }

    return {
        initialize: initialize,
        showModal: showModal,
        remove: remove
    };
})();