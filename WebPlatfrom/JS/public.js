//时间转换  李彬华
function ChangeDateFormatToLocaleDate(jsondate) {
    try {
        jsondate = jsondate.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        } else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }
        var date = new Date(parseInt(jsondate, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        var temp = new Date(date.getFullYear() + "/" + month + "/" + currentDate + " " + hours + ":" + minutes + ":" + seconds);
        return temp.toLocaleDateString();
    } catch (e) {
        return "";
    }
}


//时间转换  黎国劲
function convertTime(jsonTime, format) {
    var date = new Date(parseInt(jsonTime.replace("/Date(", "").replace(")/", ""), 10));
    var formatDate = date.format(format);
    return formatDate;
}
/////format:yyyy-MM-dd hh:mm:ss
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };

    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }

    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }

    return format;
}

///时间比较大小
function comptime(LastUpdate) {
    var UpdateTime = new Date(LastUpdate);
    UpdateTime.setMinutes(UpdateTime.getMinutes() + 3);
    var beginTime = UpdateTime.format("yyyy-MM-dd hh:mm:ss");
    var endTime = new Date().format("yyyy-MM-dd hh:mm:ss");
    var beginTimes = beginTime.substring(0, 10).split('-');
    var endTimes = endTime.substring(0, 10).split('-');

    beginTime = beginTimes[1] + '/' + beginTimes[2] + '/' + beginTimes[0] + ' ' + beginTime.substring(10, 19);
    endTime = endTimes[1] + '/' + endTimes[2] + '/' + endTimes[0] + ' ' + endTime.substring(10, 19);

    //alert(beginTime + "aaa" + endTime);
    //alert(Date.parse(endTime));
    //alert(Date.parse(beginTime));
    var a = (Date.parse(endTime) - Date.parse(beginTime)) / 3600 / 1000;
    if (a < 0) {
        //alert("endTime小!");掉线
        return true;
    } else if (a > 0) {
        //alert("endTime大!");在线
        return false;
    } else if (a == 0) {
        return false;
    } else {
        return 'exception';
    }
}


function arrMaxNum(arr) {
    var maxNum = -Infinity;
    for (var i = 0; i < arr.length; i++) {
        arr[i] > maxNum ? maxNum = arr[i] : null;
    };
    return maxNum;
}

function arrMinNum(arr) {
    var minNum = Infinity;
    for (var i = 0; i < arr.length; i++) {
        arr[i] < minNum ? minNum = arr[i] : null;
    };
    return minNum;
}
function arrAverageNum(arr) {
    var sum = 0;
    for (var i = 0; i < arr.length; i++) {
        sum += arr[i];
    };
    return ~~(sum / arr.length * 100) / 100;
}

//ip地址
function isValidIP(ip) {
    var reg =/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
    return reg.test(ip);
}