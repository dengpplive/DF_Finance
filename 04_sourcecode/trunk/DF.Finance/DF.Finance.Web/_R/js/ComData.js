//webapi地址
webapiUrl = "http://www.dfweb.com";
//查询url参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}
//提示
function popMsg(msg, duration, callback) {
    if (duration == undefined || duration == null) duration = 1000;
    $.toast(msg, duration);
    setTimeout(function () {
        if (callback) callback()
    }, duration);
}

//检测是否登录
function CheckLogin() {
    $.get(webapiUrl + "/api/BaseWebApi/CheckLogin", null, function (data) {
        if (data.Code != 1) {
            popMsg('该页面需登录访问,请登录', 1000, function () {                
                $.router.load("#AgentLogin");
            })
        }
    }, "json");
}
