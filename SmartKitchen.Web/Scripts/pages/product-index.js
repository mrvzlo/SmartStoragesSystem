var toggle = function (id, type) {
    if (type == 0) {
        $("#0_" + id).show();
        $("#1_" + id).hide();
    } else if (type == 1) {
        $("#1_" + id).show();
        $("#0_" + id).hide();
    }
    if (type == 2) {
        $("#2_" + id).show();
        $("#3_" + id).hide();
    } else if (type == 3) {
        $("#3_" + id).show();
        $("#2_" + id).hide();
    }
}

$(function () {
    new MvcGrid(document.querySelector('.mvc-grid')).reload();
});