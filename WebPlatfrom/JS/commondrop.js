$(".dropdown-menu-right li").click(function () {
    var $item = $(this).children("a");
    $("#input-li").val($item.html()).attr("roleId", $item.attr("roleId"));
    if ($("#input-li").val() == "---请选择---") {
        $("#input-li").val("");
    }
});