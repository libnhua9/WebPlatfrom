//点击tr选中
$(".container-table").on("click", "tr", function () {
    var input = $(this).find("input");
    if (!$(input).prop("checked")) {
        $(input).prop("checked", true);
    } else {
        $(input).prop("checked", false);
    }
});
$(".container-table").on("click", "input", function (event) {
    event.stopImmediatePropagation();
});