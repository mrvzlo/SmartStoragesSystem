// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var DatetimePickerJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            productId: null,
            url :null
        };
        settings = $.extend(true, defaults, options);
        
        $(document).on("change", "#Day", updateDaysInMonth);
        $(document).on("change", "#Month", updateDaysInMonth);
        $(document).on("change", "#Year", updateDaysInMonth);
        $(document).on("click", "#btnDateSave", dateUpdate);
        $(document).on("click", "#btnDateReset", setTodayDate);

        updateDaysInMonth();
        $("#Month").attr({
            "max": 12,
            "min": 1
        });
        $("#Day").attr({
            "min": 1
        });
        $("#Year").attr({
            "min": (new Date()).getFullYear()
        });
        setTodayDate();
    };

    var dateUpdate = function() {
        var value = $("#Day").val();
        var month = $("#Month").val();
        var year = $("#Year").val();
        var newDate = value + "/" + month + "/" + year;
        var url = settings.url + settings.productId + '&dateStr=' + newDate;
        $.post(url,
            function () {
                new MvcGrid(document.querySelector('.mvc-grid')).reload();
            });
    }

    var updateDaysInMonth = function() {
        var month = $("#Month").val();
        var max = 31;
        if (month == 4 || month == 6 || month == 9 || month == 11) max = 30;
        if (month == 2) {
            if ($("#Year").val() % 4 == 0) max = 29;
            else max = 28;
        }

        $("#Day").attr({
            "max": max
        });
        if ($("#Day").val() > max) $("#Day").val(max);
    }

    var showDatePicker = function(product) {
        settings.productId = product;
        var name = $("#name_" + product).text();
        $("#DateModalLabel").text("Choose expiration date for " + name);
    }

    var setTodayDate = function() {
        var date = new Date();
        $("#Month").val(date.getMonth() + 1);
        $("#Day").val(date.getDate());
        $("#Year").val(date.getFullYear());
    }


    var inputChange = function(id, value) {
        id = "#" + id;
        var max = $(id).attr('max');
        var min = $(id).attr('min');
        var val = $(id).val();
        var newVal = Number(val) + value;
        if (newVal > max) newVal = max;
        else if (newVal < min) newVal = min;
        $(id).val(newVal);
        updateDaysInMonth();
    }

    return {
        initialize: initialize,
        showDatePicker: showDatePicker,
        inputChange: inputChange
    };

})();