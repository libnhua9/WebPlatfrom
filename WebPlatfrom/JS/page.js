var tablePaging;
$(function () {
    //全选
    $("#checkAll").click(function () {
        if ($(this).is(":checked")) {
            $("input[name='checkbox']").each(function () {
                $(this).attr("checked", true);
            });
        } else {
            $("input[name='checkbox']").each(function () {
                $(this).attr("checked", false);
            });
        }
    });
    //点击全选边框变色
    $("input:checkbox").each(function () {
        $(this).click(function () {
            //alert("aaa");
            $(this).toggleClass("checkon");
        });
    });

    /*
    * 分页设置
    */
    tablePaging = {
        $obj: null,
        nowIndx: 0,
        pageTotal: 100,
        gotoPage: function (index) {
            if (tablePaging.nowIndx === index || index < 1 || index > tablePaging.pageTotal || tablePaging.$obj === undefined) {
                return;
            }

            var pageList = [];
            var i = 0;
            if (tablePaging.pageTotal <= 9) {
                for (i = 0; i < tablePaging.pageTotal; i++) {
                    pageList[i] = i + 1;
                }
            } else {
                pageList[0] = 1;
                pageList[8] = tablePaging.pageTotal;
                var p = tablePaging.pageTotal - 1;

                if (index <= 5) {
                    for (i = 1; i <= 6; i++) {
                        pageList[i] = i + 1;
                    }
                    pageList[7] = -1;
                } else if (index >= tablePaging.pageTotal - 4) {
                    pageList[1] = -1;

                    for (i = 7; i >= 2; i--) {
                        pageList[i] = p--;
                    }
                } else {
                    pageList[1] = -1;
                    pageList[7] = -1;
                    p = index - 2;
                    for (i = 2; i <= 6; i++) {
                        pageList[i] = p++;
                    }
                }
            }

            //alert(tablePaging.$obj.find("li:first").html());

            tablePaging.nowIndx = index;
            tablePaging.$obj.find("li").remove();
            var next = index + 1;
            var perr = index - 1;


            tablePaging.$obj.children("ul").append("<li><a onclick=\"tablePaging.gotoPage(" + perr + ")\" href=\"javascript:void(0);\"> 上一页 </a></li>");
            if (index === 1) {
                tablePaging.$obj.find("li:last").addClass("disabled");
            }

            for (i = 0; i < pageList.length; i++) {
                if (pageList[i] === -1) {
                    tablePaging.$obj.children("ul").append("<li><span>...</span></li>");
                } else {
                    tablePaging.$obj.children("ul").append("<li><a onclick=\"tablePaging.gotoPage(" + pageList[i] + " )\" href=\"javascript:void(0);\"> " + pageList[i] + " </a></li>");
                    if (pageList[i] === index) {
                        tablePaging.$obj.find("li:last").addClass("active");
                    }
                }

            }
            tablePaging.$obj.children("ul").append("<li><a onclick=\"tablePaging.gotoPage(" + next + ")\" href=\"javascript:void(0);\"> 下一页 </a></li>");
            if (index === tablePaging.pageTotal) {
                tablePaging.$obj.find("li:last").addClass("disabled");
            }
            loadData(index,"","")
        },
        init: function ($n, total) {
            tablePaging.$obj = $n;
            tablePaging.pageTotal = total;
            tablePaging.gotoPage(1);
        }
    }
    // 总页数
    

    //function loadData(index, type, name) {
    //    $.get("",{index:index,type:type,name:name},function(data) {
    //        alert(data);
    //    });
    //}
    //$().click(function() {

    //    $(this).attr("")
    //})
});