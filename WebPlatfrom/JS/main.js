var server = {
    $mouse: null,
    loadPage: function (link) {
        if (link) {
            if (window.top !== window.self) {
                var frm = window.top.document.getElementById("ifrm");
                $(frm).attr("src", link);
            } else {
                $("#ifrm").attr("src", link);
            }
        }
    },
    initPage: function () {
        $("#nav-main .dropdown").mouseover(function () {
            $(this).addClass("open");
        });
        $("#nav-main .dropdown").mouseout(function () {
            $(this).removeClass("open");
        });

        $("#sidebar li>a").hover(function () {
            var src = $(this).find(".img-icon img").attr("src");
            if (src) {
                src = src.replace("2.png", "1.png");
                $(this).find(".img-icon img").attr("src", src);
            }

            var srccaret = $(this).find(".glyphicon-caret>img").attr("src");
            if (srccaret) {
                srccaret = srccaret.replace("1.jpg", "2.jpg");
                $(this).find(".glyphicon-caret>img").attr("src", srccaret);
            }

        }, function () {
            var srccaret = $(this).find(".glyphicon-caret>img").attr("src");
            if (srccaret) {
                srccaret = srccaret.replace("2.jpg", "1.jpg");
                $(this).find(".glyphicon-caret>img").attr("src", srccaret);
            }
            if (!$(this).hasClass("active")) {
                var src = $(this).find(".img-icon img").attr("src");
                if (src) {
                    src = src.replace("1.png", "2.png");
                    $(this).find(".img-icon img").attr("src", src);
                }
            }
        });

        $("#sidebar ul>li>ul a").hover(function () {
            var nodes = $(this).attr("nodes");
            if (!nodes) {
                return;
            }
            var array = nodes.split(",");
            var n1 = array[0];
            var n2 = array[1];
            var n3 = array[2];
            if (menuBody.menulist[n1].Items[n2].Items[n3].Items && menuBody.menulist[n1].Items[n2].Items[n3].Items.length > 0) {
                $("#slider-menu-list .dropdown-menu>li").remove();
                for (var i = 0; i < menuBody.menulist[n1].Items[n2].Items[n3].Items.length; i++) {
                    var node4 = n1 + "," + n2 + "," + n3 + "," + i;
                    var item = menuBody.menulist[n1].Items[n2].Items[n3].Items[i];
                    $("#slider-menu-list .dropdown-menu").append("<li><a nodes=\"" + node4 + "\" href='javascript:void(0);'>" + item.Name + "</a></li>");
                }

                server.$mouse = $(this);
                $(this).addClass("mouse");
                var top = $(this).position().top;
                var width = $(this).parent().width();
                $("#slider-menu-list").addClass("open");
                $("#slider-menu-list .dropdown-menu").css({ 'top': top - 130, 'left': width - 1 });


                $("#slider-menu-list a").click(function () {
                    var nodes4 = $(this).attr("nodes");
                    if (!nodes4) {
                        return;
                    }
                    var array4 = nodes4.split(",");
                    var na1 = array4[0];
                    var na2 = array4[1];
                    var na3 = array4[2];
                    var na4 = array4[3];
                    var link = menuBody.menulist[na1].Items[na2].Items[na3].Items[na4].Link;
                    $(".menu-main li>a").removeClass("active");
                    server.$mouse.addClass("active");
                    server.loadPage(link);
                    $("#slider-menu-list").removeClass("open");
                });
            }

        }, function () {
            $(this).removeClass("mouse");
            $("#slider-menu-list").removeClass("open");

        });

        $("#slider-menu-list").hover(function () {
            $("#slider-menu-list").addClass("open");
            if (server.$mouse) {
                server.$mouse.addClass("mouse");
            }
        }, function () {
            if (server.$mouse) {
                server.$mouse.removeClass("mouse");
            }
            $("#slider-menu-list").removeClass("open");
        });


    }
};

var menuBody;

$(function () {
    $.ajax({
        url: '/home/GetAllMenu',
        type: 'get',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataObj = data.Data;
            menuBody = {
                mFirst: -1,
                menulist: dataObj,
                init: function () {

                    for (var i = 0; i < menuBody.menulist.length; i++) {
                        $("#nav-main").append(" <li class=\"dropdown\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\"  onclick=\"menuBody.selectNav(" + i + ",0,0)\" href=\"javascript:void(0)\"><span class=\"glyphicon\"><img src=\"../../Images/" + menuBody.menulist[i].Icon + "\" /></span> " + menuBody.menulist[i].Name + "</a></li>");
                        var $now = $("#nav-main").find("li:last");
                        $now.append("<ul class=\"dropdown-menu\"> </ul>");
                        for (var j = 0; j < menuBody.menulist[i].Items.length; j++) {
                            $now.children("ul").append("<li><a onclick=\"menuBody.selectNav(" + i + ", " + j + " ,0)\"  href=\"javascript:void(0)\">" + menuBody.menulist[i].Items[j].Name + "</a></li>");
                        }
                    }
                    var count = $("#nav-main>li").length;
                    $("#nav-main").width(count * 177);

                    menuBody.selectNav(0, 0, 0);

                    server.initPage();
                },
                selectNav: function (first, second, thrid) {
                    if (first == undefined || first === menuBody.mFirst) {
                        menuBody.selectMenu(second, thrid);
                        return;
                    }
                    $(".menu-main li").remove();
                    menuBody.mFirst = first;

                    $("#nav-main>li>a").removeClass("active");
                    $("#nav-main>li:eq(" + first + ")>a").addClass("active");
                    for (var i = 0; i < menuBody.menulist[first].Items.length; i++) {
                        var sitem = menuBody.menulist[first].Items[i];
                        $(".menu-main").append("<li><a onclick=\"menuBody.selectMenu(" + i + ",-1); \" href=\"javascript:void(0)\"> <span class=\"glyphicon img-icon\"><img src=\"../../Images/" + sitem.Icon + "\" /></span> &nbsp;" + sitem.Name + "</a></li>");
                        var $vli = $(".menu-main>li:eq(" + i + ")");
                        //链接地址
                        if (sitem.Link !== undefined) {
                            $vli.children("a").attr("link", sitem.Link);
                        }

                        if (sitem.Items !== undefined && sitem.Items!=null) {
                            $vli.children("a").append("<span class=\"glyphicon glyphicon-caret\"><img src=\"../../Images/down1.jpg\" /></span>");
                            $vli.append("<ul></ul>");
                            for (var j = 0; j < sitem.Items.length; j++) {
                                var titem = sitem.Items[j];
                                var nodes3 = menuBody.mFirst + "," + i + "," + j;
                                $vli.children("ul").append(" <li><a nodes=\"" + nodes3 + "\" link=\"" + titem.Link + "\" onclick=\"menuBody.selectMenu(" + i + "," + j + ");\" href=\"javascript:void(0)\" >" + titem.Name + "</a></li>");

                                //if (titem.Items && titem.Items.length > 0) {
                                //    var $fourLi = $vli.find("ul>li:last");
                                //    $fourLi.addClass("dropdown");
                                //    $fourLi.attr("role", "presentation");
                                //    $fourLi.append("<ul class=\"dropdown-menu\"></ul>");
                                //    for (var k = 0; k < titem.Items.length; k++) {
                                //        $fourLi.children("ul").append(" <li><a link=\"" + titem.Items[k].Link + "\" onclick=\"menuBody.selectMenu(" + i + "," + j + ");\" href=\"javascript:void(0)\" >" + titem.Items[k].Name + "</a></li>");
                                //    }
                                //}
                            }
                        }
                    }
                    menuBody.selectMenu(second, thrid);
                    server.initPage();
                },
                selectMenu: function (i, j) {

                    var $pli = $(".menu-main>li:eq(" + i + ")");
                    var len = $pli.children("ul").length;
                    var srccaret = $pli.find(".glyphicon-caret>img").attr("src");

                    var urlLink = "";
                    if (len > 0) {
                        var select = $pli.attr("selected");
                        if (select === undefined) {
                            $pli.children("ul").slideDown(200);
                            srccaret = srccaret.replace("down", "up");
                            $pli.find(".glyphicon-caret>img").attr("src", srccaret);

                            $pli.attr("selected", "selected");
                        } else if (j < 0) {
                            srccaret = srccaret.replace("up", "down");
                            $pli.find(".glyphicon-caret>img").attr("src", srccaret);

                            $pli.children("ul").slideUp(200);
                            $pli.removeAttr("selected");
                        }

                        if (j >= 0) {
                            $(".menu-main li>a").removeClass("active");
                            $(".menu-main>li:eq(" + i + ")>ul>li:eq(" + j + ")>a").addClass("active");
                            var node3 = menuBody.menulist[menuBody.mFirst].Items[i].Items[j];
                            if (node3.Items && node3.Items.length > 0 && node3.Items[0].Link) {
                                urlLink = node3.Items[0].Link;
                            } else {
                                urlLink = node3.Link;
                            }

                        }
                    } else {
                        var $active = $(".menu-main li>a.active");
                        var src = $active.find(".img-icon img").attr("src");
                        if (src) {
                            src = src.replace("1.png", "2.png");
                            $active.find(".img-icon img").attr("src", src);
                            $active.removeClass("active");

                        }
                        $pli.children("a").addClass("active");
                        urlLink = menuBody.menulist[menuBody.mFirst].Items[i].Link;
                        src = $pli.children("a").find(".img-icon img").attr("src");
                        src = src.replace("2.png", "1.png");
                        $pli.children("a").find(".img-icon img").attr("src", src);

                    }

                    //if (j === -1) {
                    //    j = 0;
                    //}
                    //debugger;
                    //var secondItem = menuBody.menulist[menuBody.mFirst].Menus[i];
                    //if (secondItem.Link !== undefined && secondItem.Link.length > 0) {
                    //    urlLink = secondItem.Link;
                    //} else if (secondItem.Items !== undefined && secondItem.Items[j].Link.length > 0) {
                    //    urlLink = secondItem.Items[j].Link;
                    //}

                    //alert(urlLink);
                    //加载页面
                    server.loadPage(urlLink);
                    //alert("aa");

                }
            };
            menuBody.init();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('主界面列表获取失败');
        }
    });
        
    
           
        });

// 获取浏览器窗口的可视区域的宽度
function getViewPortWidth() {
    return document.documentElement.clientWidth || document.body.clientWidth;
}

// 获取浏览器窗口的可视区域的高度
function getViewPortHeight() {
    return document.documentElement.clientHeight || document.body.clientHeight;
}

// 获取浏览器窗口水平滚动条的位置
function getScrollLeft() {
    return document.documentElement.scrollLeft || document.body.scrollLeft;
}

// 获取浏览器窗口垂直滚动条的位置
function getScrollTop() {
    return document.documentElement.scrollTop || document.body.scrollTop;
}

$(function () {
    resizeContainer();
    $(window).resize(resizeContainer);
});
function resizeContainer() {
    var h = getViewPortHeight() - $("#header").height();
    $("#sidebar").height(h);
    $("#main").height(h - 15);
}