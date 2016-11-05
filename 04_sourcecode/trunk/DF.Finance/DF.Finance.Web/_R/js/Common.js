/*
版权信息：jim 2016
作    者：jim
内容说明：公用js类库
*/
DFCommon = {
    Vue: null,//公共Vue对象 用来封装用户信息及一些全局变量
    /*
    Ajax 访问Api接口方法
    异步加载请求返回统一使用 RetObj 包裹
    Jim 2016/05/09
    option: 该参数与jQuery.ajax的option参数一致，比jQuery.ajax的option多一项noAccess的回调
    */
    ajax: function (option) {
        var success = option.success || null;
        var error = option.error || null;
        var noAccess = option.noAccess || null;
        var beforeSend = option.beforeSend || null;
        var isShowLoading = option.isShowLoading || false;
        delete option.noAccess;
        delete option.isShowLoading;

        option.timeout = option.timeout || 1000 * 60;
        option.dataType = option.dataType || "json";
        option.success = function (result) {

            if (typeof result.Code == 'undefined') {
                $.toast("服务器异常！");
                return;
            } else if (result.Code == 1) {
                if (success != null) {
                    success(result.Data, result);
                } else {
                    console.log(result.Data);
                }
            } else if (result.Code == -1) {
                if (noAccess != null) {
                    noAccess(result.Msg, result.Data, result);
                } else {
                    //此处跳转到登录  
                    location.href = "/wx/selectUserType.html";
                }
            } else if (result.Code == -2) {
                //跳转到登录
            } else {
                if (error != null) {
                    error(result.Msg, result);
                } else {
                    $.toast(result.Msg);
                }
            }
            if (isShowLoading) {
                $.hidePreloader();
            }
        };
        option.error = function (xhr, msg) {
            if (isShowLoading) {
                $.hidePreloader();
            }
            if (error != null) {
                error(msg, null);
            } else {
                $.toast(msg);
            }
        };
        option.beforeSend = function (xhr) {
            if (isShowLoading) {
                $.showPreloader();
            }
            if (beforeSend != null) {
                return beforeSend(xhr);
            } else {
                return true;
            }
        };

        return $.ajax(option);
    },
    //获取登录用户信息
    getUserInfo: function (callback) {
        DFCommon.ajax({
            type: 'get',
            url: "/api/BaseWebApi/GetCurrentUserInfo",
            async: false,
            success: function (data) {
                console.log(data);
                if (!!data) {
                    if (!!!DFCommon.Vue) {
                        DFCommon.Vue = new Vue({
                            el: "#body",
                            data: { userModel: data }
                        });
                    }
                }
            }
        });
    },

    //检测是否登录
    checkLogin: function (url) {
        $.get(webapiUrl + "/api/BaseWebApi/CheckLogin", null, function (data) {
            // console.log(data);
            if (data.Code != 1) {
                $.router.load(url, true);
            }
        }, "json");
    },
    //注销
    logout: function () {
        DFCommon.ajax({
            type: 'get',
            url: "/api/BaseWebApi/UserLoginOut",
            success: function (data) {
                storage.setItem("userId", "");
            }
        });
        storage.setItem("userId", "");
        storage.setItem("orderId", "0");
        storage.setItem("saleUserId", "0");
        storage.setItem("cusId", "0");
        storage.setItem("pId", "0");
        storage.setItem("userAgentId", "0");
        storage.setItem("autoLogin", "");
    },
    //格式化金额 3位 带有￥
    formatCurrency: function (s) {
        if (/[^0-9\.]/.test(s)) return "invalid value";
        s = s.replace(/^(\d*)$/, "$1.");
        s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
        s = s.replace(".", ",");
        var re = /(\d)(\d{3},)/;
        while (re.test(s))
            s = s.replace(re, "$1,$2");
        s = s.replace(/,(\d\d)$/, ".$1");
        return "￥" + s.replace(/^\./, "0.")
    }
};

(function ($, w) {
    //微信自带头部，去掉
    if ($.device.isWeixin) {
        $('header').remove();
    }
    $("[data-url]").on("tap", function () {
        $.router.load($(this).attr("data-url"), true);
    });
    var regs = {
        //手机号码
        mobile: /^1(3|4|5|7|8)\d{9}$/
    };
    var regs_msg = {
        //手机号码
        mobile: "手机号码有误！"
    };
    //验证表单数据
    //调用方式:var isValide = $("body").valide();
    //注意事项:
    //1.适用于 input/textarea
    //2.文本框不能为空的，需要加属性 data-empty-alt="手机号不能为空"
    //3.需要使用正则验证的文本框需要实现属性，如 data-regex="/^\d*$/"
    //4.手机号码文本框验证加 data-regex="mobile"
    $.fn.valide = function () {
        var isValide = true;
        $(this).find("input[data-regex],textarea[data-regex],input[data-empty-alt],textarea[data-empty-alt]").each(function () {
            if ($(this).val().trim() == "" && $(this).attr("data-empty-alt")) {
                $.toast($(this).attr("data-empty-alt"));
                isValide = false;
                $(this).focus();
                return false;
            } else if ($(this).attr("data-regex")) {
                var reg = null;
                if (!!!regs_msg[$(this).attr("data-regex")]) {
                    reg = eval($(this).attr("data-regex"));
                } else {
                    reg = regs[$(this).attr("data-regex")];
                }
                if (!reg.test($(this).val())) {
                    var alt = $(this).attr("data-regex-alt");
                    if (!alt) {
                        alt = "值输入有误！";

                    }
                    $.toast(alt);
                    isValide = false;
                    $(this).focus();
                    return false;
                }
            }
        });
        if (!isValide) {
            return false;
        }
        $("input[min]").each(function () {
            if (parseFloat($(this).val()) < parseFloat($(this).attr("min"))) {
                $.toast($(this).attr("placeholder"));
                $(this).focus();
                isValide = false;
                return false;
            }
        });
        if (!isValide) {
            return false;
        }
        $("input[max]").each(function () {
            if (parseFloat($(this).val()) > parseFloat($(this).attr("max"))) {
                $.toast($(this).attr("placeholder"));
                $(this).focus();
                isValide = false;
                return false;
            }
        });
        return isValide;
    };
    //========================格式化字符串 调用方式: var newString = String.format("{1}好{0}好{2}好，{1}还好吗？","我","你","他"); jim 2016-05-16 ========================
    String.format = function (s) {
        var t = s;
        for (var i = 1; i < arguments.length; i++) {
            t = t.replace(eval("/\\{" + (i - 1) + "\\}/ig"), arguments[i]);
        }
        return t;
    };
    //========================o=model 序列化后的json 把表单元素转换成json data-json="" 则不转换 jim 2016-05-12 ========================
    $.fn.serializeObject = function (o) {
        if (!o) {
            o = {};
        }
        var a = this.serializeArray();
        $.each(a, function () {
            var _no = !$("[name='" + this.name + "']").attr("data-json");
            if (_no) {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    //========日期格式化=======调用： $.formatDate('格式','日期')=====================================
    //自定义函数
    $.formatDate = function (pattern, date) {
        //如果不设置，默认为当前时间  
        if (!date) date = new Date();
        if (typeof (date) === "string") {
            if (date == "") date = new Date();
            else date = new Date(date.replace(/-/g, "/"));
        }
        /*补00*/
        var toFixedWidth = function (value) {
            var result = 100 + value;
            return result.toString().substring(1);
        };

        /*配置*/
        var options = {
            regeExp: /(yyyy|M+|d+|h+|m+|s+|ee+|ws?|p)/g,
            months: ['January', 'February', 'March', 'April', 'May',
                     'June', 'July', 'August', 'September',
                      'October', 'November', 'December'],
            weeks: ['Sunday', 'Monday', 'Tuesday',
                    'Wednesday', 'Thursday', 'Friday',
                        'Saturday']
        };

        /*时间切换*/
        var swithHours = function (hours) {
            return hours < 12 ? "AM" : "PM";
        };

        /*配置值*/
        var pattrnValue = {
            "yyyy": date.getFullYear(),                      //年份  
            "MM": toFixedWidth(date.getMonth() + 1),           //月份  
            "dd": toFixedWidth(date.getDate()),              //日期  
            "hh": toFixedWidth(date.getHours()),             //小时  
            "mm": toFixedWidth(date.getMinutes()),           //分钟  
            "ss": toFixedWidth(date.getSeconds()),           //秒  
            "ee": options.months[date.getMonth()],           //月份名称  
            "ws": options.weeks[date.getDay()],              //星期名称  
            "M": date.getMonth() + 1,
            "d": date.getDate(),
            "h": date.getHours(),
            "m": date.getMinutes(),
            "s": date.getSeconds(),
            "p": swithHours(date.getHours())
        };

        return pattern.replace(options.regeExp, function () {
            return pattrnValue[arguments[0]];
        });
    };
})(Zepto, window);
