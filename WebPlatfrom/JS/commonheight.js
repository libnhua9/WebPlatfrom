// 获取浏览器窗口的可视区域的高度
function getViewPortHeight() {
    return document.documentElement.clientHeight || document.body.clientHeight;
}
$(function () {
    resizeContainer();
    $(window).resize(resizeContainer);
});

function resizeContainer() {
    var h = getViewPortHeight();
    var ch = $("#container-body").height();
    //alert(ch);
    if (ch < h - 15) {
        $("#container-body").height(h - 15);
    }
    //else {
    //    $("#container-body").css("height", "auto");
    //}
}