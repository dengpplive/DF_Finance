/// <reference path="lib/knockout-3.4.0.js" />
/// <reference path="http://cdn.bootcss.com/knockout/3.4.0/knockout-debug.js" />

/**
 * Created by liurongchang on 2016/5/31.
 */
//微信自带头部，去掉
if ($.device.isWeixin) {
    $('header').remove();
}
//查询url参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}
//提示
function popMsg(msg, duration, callback) {
    $.toast(msg, duration);
    setTimeout(function () {
        callback()
    }, duration);
}

//客户登录ViewModel
// var CustomersLoginViewModel = function () {
//     var self = this;
//     self.autoLogin = ko.observable(false);
// }
//
// //金融业代登录ViewModel
// var SalesLoginViewModel = function () {
//     this.phone = ko.observable();
//     this.pwd = ko.observable();
//     //记住密码
//     this.remember = ko.observable(false);
//
//     var self = this;
//     this.init = function () {
//         //是否记住密码
//         if (this.remember()) {
//             var remember_val = storage.getItem("remember");
//             if (remember_val) self.pwd(remember_val);
//         }
//     }
//     this.submit = function () {
//         if (!self.phone()) {
//             $.alert("请输入手机号码");
//             return;
//         }
//         if (!self.pwd()) {
//             $.alert("请填写密码");
//             return;
//         }
//         $.showPreloader('登录中')
//         $.ajax({
//             url: '/FinancialBusiness/AjaxLogin',
//             data: { phone: self.phone(), pwd: self.pwd() },
//             type: 'post',
//             dataType: 'json',
//             success: function (data) {
//                 $.hidePreloader();
//                 if (data.Code == 1) {
//                     popMsg('登录成功', 1000, function () {
//                         //存储
//                         storage.setItem("userinfo", JSON.stringify(data.Data));
//                         storage.setItem("user", self.phone());
//                         if (this.remember()) storage.setItem("remember", self.pwd());
//                         window.location = 'index.html';
//                     })
//                 } else {
//                     $.alert(data.Msg);
//                 }
//             }, error: function (e) {
//                 $.hidePreloader();
//                 //console.dir(e)
//             }
//         })
//     }
// }
// var sales = new SalesLoginViewModel();
// sales.init();
// ko.applyBindings(sales);
// $.init();