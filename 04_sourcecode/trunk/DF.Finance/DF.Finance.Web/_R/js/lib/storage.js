/*�������ȡCookie*/
var Cookie = {}
Cookie.write = function (key, value, duration) {
    var d = new Date();
    d.setTime(d.getTime() + 1000 * 60 * 60 * 24 * 30);
    document.cookie = key + "=" + encodeURI(value) + "; expires=" + d.toGMTString();
};
Cookie.clear = function (key) {
    var d = new Date();
    d.setTime(d.getTime() - 1000);
    document.cookie = key + "=; expires=" + d.toGMTString();
};
Cookie.read = function (key) {
    var arr = document.cookie.match(new RegExp("(^| )" + key + "=([^;]*)(;|$)"));
    if (arr != null)
        return decodeURIComponent(arr[2]);
    return "";
};
//���屾�ش洢����
var storage = {
    getItem: function (key) {//���������֧�ֱ��ش洢���localStorage��getItem������Թ���Cookie
        return window.localStorage ? localStorage.getItem(key) : Cookie.read(key);
    },
    setItem: function (key, val) {//���������֧�ֱ��ش洢�����localStorage������Թ���Cookie
        if (window.localStorage) {
            localStorage.setItem(key, val);
        } else {
            Cookie.write(key, val);
        }
    },
    removeItem: function (key) {
        if (window.localStorage) {
            localStorage.removeItem(key);
        } else {
            Cookie.clear(key);
        }
    }
};