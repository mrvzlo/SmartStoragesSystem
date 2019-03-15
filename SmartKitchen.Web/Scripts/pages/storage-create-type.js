// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var StorageCreateTypeJs = (function () {

    var bCanPreview = true;
    var canvas = document.getElementById('picker');
    var settings = {};

    var initialize = function (options) {

        var defaults = {
            list: [],
            storageId: null,
            removeUrl: null
        };
        settings = $.extend(true, defaults, options);
        $(document).on("click", "#uploadBtn", function () { $("#upload").click(); });
        $(document).on("change", "#upload", uploadUpd);
        $(document).on("click", "body", updateColor);

        var ctx = canvas.getContext('2d');
        ctx.fillStyle = "#000";
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        var image = new Image();
        image.onload = function () {
            ctx.drawImage(image, 0, 0, image.width, canvas.height);
        }
        image.src = "../Content/ColorPicker.jpg";
        $('#picker').mousemove(function (e) {
            if (bCanPreview) {
                var canvasOffset = $(canvas).offset();
                var canvasX = Math.floor(e.pageX - canvasOffset.left);
                var canvasY = Math.floor(e.pageY - canvasOffset.top);
                canvasX *= 300 / $("#picker").width();
                canvasY *= 150 / $("#picker").height();
                var imageData = ctx.getImageData(canvasX, canvasY, 1, 1);
                var pixel = imageData.data;
                var pixelColor = "rgb(" + pixel[0] + ", " + pixel[1] + ", " + pixel[2] + ")";
                $('.preview').css('backgroundColor', pixelColor);
                var dColor = pixel[2] + 256 * pixel[1] + 65536 * pixel[0];
                $('#Background').val(dColor.toString(16).substr(-6).toUpperCase());
            }
        });
        $('#picker').click(function (e) { // click event handler
            bCanPreview = !bCanPreview;
        });
        $('#Background').click(function (e) { // preview click
            $('#pickerDiv').fadeToggle("fast", "linear");
            bCanPreview = true;
        });
    };

    var updateColor = function() {
        $("#storage").css("background-color", "#" + $("#Background").val());
    };

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
        $("#Background").val(color);
        updateColor();
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
        removeConfirm: removeConfirm
    };
})();