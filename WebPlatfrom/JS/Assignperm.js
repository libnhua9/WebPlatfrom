

function GetList(Type, Id) {
    $(".admintable-TVM").html(' <table class="table table-striped tabtr" id="tbDeviceList"></table>');
    $("#tbDeviceList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetTVMList",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //"<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />"
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" disabled="disabled" data-id="TVM_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" disabled="disabled" data-id="TVM_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="TVM_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="TVM_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}

function GetM4List(Type, Id) {
    $(".admintable-M4Channel").html(' <table class="table table-striped tabtr" id="tbDeviceM4List"></table>');
    $("#tbDeviceM4List").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetM4Channellist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="M4_check" disabled="disabled"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="M4_check" disabled="disabled"  class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="M4_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="M4_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                        
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}


///安全检票闸机
function GetTicketList(Type, Id) {
    $(".admintable-Ticket").html(' <table class="table table-striped tabtr" id="tbDeviceTicketList"></table>');
    $("#tbDeviceTicketList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetTicketlist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="Ticket_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="Ticket_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="Ticket_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="Ticket_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                        
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}

///水公园储物柜
function GetParkStoreList(Type, Id) {
    $(".admintable-ParkStroe").html(' <table class="table table-striped tabtr" id="tbDeviceParkStroeList"></table>');
    $("#tbDeviceParkStroeList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetParkStorelist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="ParkStroe_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="ParkStroe_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="ParkStroe_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="ParkStroe_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                        
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}


///自助储物柜
function GetSelfCabinetList(Type, Id) {
    $(".admintable-SelfCabine").html(' <table class="table table-striped tabtr" id="tbDeviceSelfCabineList"></table>');
    $("#tbDeviceSelfCabineList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetSelfCabinetlist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="SelfCabine_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="SelfCabine_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="SelfCabine_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="SelfCabine_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                       
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}

///海康DVR
function GetDVRList(Type, Id) {
    $(".admintable-DVR").html(' <table class="table table-striped tabtr" id="tbDeviceDVRList"></table>');
    $("#tbDeviceDVRList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetDVRlist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="DVR_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="DVR_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="DVR_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="DVR_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceID",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}


///LED显示屏
function GetLEDList(Type, Id) {
    $(".admintable-LED").html(' <table class="table table-striped tabtr" id="tbDeviceLEDList"></table>');
    $("#tbDeviceLEDList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GetLEDlist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type > 1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="LED_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="LED_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="LED_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="LED_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                        
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceID",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}


///客流计
function GeteSensorList(Type, Id) {
    $(".admintable-ESensor").html(' <table class="table table-striped tabtr" id="tbDeviceESensorList"></table>');
    $("#tbDeviceESensorList").dataTable({
        "bProcessing": true, //DataTables载入数据时，是否显示‘进度’提示
        "bServerSide": true, //是否启动服务器端数据导入
        bLengthChange: false,//去掉每页多少条框体
        "bJQueryUI": true, //是否使用 jQury的UI theme
        "bAutoWidth": false, //是否自适应宽度
        "bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变
        "bFilter": false, //是否启动过滤、搜索功能
        "ordering": false,  //去掉页头排序
        "ajax": {
            "url": "/ApplicationRight/GeteSensorlist",
            "type": "POST",
            "data": { UserType: Type, UserId: Id }
        },
        "aoColumns": [
                {
                    //<input type=\"checkbox\" name=\"checkbox\" id=\"TVMCheck\"  class=\"checkchild\"  value=\"\" />
                    "mDataProp": "DeviceName", "sTitle": "", "sDefaultContent": "", "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        if (Type>1) {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="ESensor_check" disabled="disabled" class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="ESensor_check" disabled="disabled" class="checkchild"  value="' + list[1] + '" />';
                            }
                        } else {
                            if (list.length == 3) {
                                return '<input type="checkbox" name="checkbox" data-id="ESensor_check"  class="checkchild" checked="checked"  value="' + list[1] + '" />';
                            } else {
                                return '<input type="checkbox" name="checkbox" data-id="ESensor_check" class="checkchild"  value="' + list[1] + '" />';
                            }
                        }
                    },
                    "bSortable": false
                },
                {
                    "mDataProp": "DeviceName",
                    "sTitle": "设备名称",
                    "sDefaultContent": "",
                    "sClass": "center",
                    "render": function (data, type, full, meta) {
                        var list = data.split("*");
                        return list[0];
                    },
                    "bSortable": false
                },

                {
                    "mDataProp": "DeviceSN",
                    "sTitle": "设备编号",
                    "sDefaultContent": "",
                    "sClass": "center"
                },


        ], "language": {
            "processing": "加载中...",
            "lengthMenu": "每页显示 _MENU_ 条数据",
            "zeroRecords": "没有匹配结果",
            "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "emptyTable": "没有匹配结果",
            "loadingRecords": "载入中...",
            "thousands": ",",
            "paginate": {
                "first": "首页",
                "previous": "上一页",
                "next": "下一页",
                "last": "末页"
            }
        }
    });

}