// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
// ReSharper disable CoercedEqualsUsing
var StorageCreateTypeJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            list: [],
            storageId: null,
            removeUrl: null,
            red: 255,
            green: 255,
            blue: 255
        };
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#uploadBtn", function () { $("#upload").click(); });
        $(document).on("change", "#upload", uploadUpd);
    };

    var changeColor = function () {
        var pixelColor = "rgb(" + settings.red + ", " + settings.green + ", " + settings.blue + ")";
        $("#storage").css("backgroundColor", pixelColor);
        var dColor = Number(settings.red).toString(16).toUpperCase() + Number(settings.green).toString(16).toUpperCase() + Number(settings.blue).toString(16).toUpperCase();
        $("#Background").val(dColor);
    }

    var changeRed = function (val) {
        settings.red = val;
        changeColor();
    }

    var changeGreen = function (val) {
        settings.green = val;
        changeColor();
    }

    var changeBlue = function (val) {
        settings.blue = val;
        changeColor();
    }

    var updateForm = function (id, name, color) {
        if (id !== 0) $("#submit").val("Update");
        else {
            $("#upload").wrap('<form>').closest('form').get(0).reset();
            $("#upload").unwrap();
            $("#upload").trigger("change");
            $("#submit").val("Create");
        }
        $("#Name").val(name);
        $("#Id").val(id);
        var red = parseInt(color.substring(0, 2), 16);
        var green = parseInt(color.substring(2, 4), 16);
        var blue = parseInt(color.substring(4, 6), 16);
        settings.red = red;
        settings.green = green;
        settings.blue = blue;
        $("#rangeRed").val(red);
        $("#rangeGreen").val(green);
        $("#rangeBlue").val(blue);
        changeColor();
    };

    var uploadUpd = function () {
        var filename = $("#upload").val();
        if (filename.match(/fakepath/)) filename = filename.replace(/C:\\fakepath\\/i, '');
        if (filename.match(/png/) || filename == "") {
            $("#IconError").text("");
            $("#submit").removeAttr("disabled");
        } else {
            $("#IconError").text("Invalid file type");
            $("#submit").attr("disabled", "disabled");
        }
        $("#UploadStatus").val(filename);
    };

    var remove = function (id) {
        event.stopPropagation();
        $("#StorageTypeModal").modal();
        settings.storageId = id;
        loadNames(id);
        $("#StorageTypeLabel").text("Choose the storage type which will replace " + $("#name_" + id).val());
    };

    function removeConfirm(id) {
        var url = settings.removeUrl + settings.storageId + "&toId=" + id;
        $.post(url,
            function (success) {
                if (success) location.reload();
            });
    };

    var loadNames = function (id) {
        settings.list.forEach(function (elId) {
            var element = $("#modal_" + elId);
            if (id !== elId) {
                element.text($("#name_" + elId).val());
                element.show();
            }
            else element.hide();
        });
    }

    return {
        initialize: initialize,
        updateForm: updateForm,
        remove: remove,
        removeConfirm: removeConfirm,
        changeRed: changeRed,
        changeGreen: changeGreen,
        changeBlue: changeBlue
    };
})();