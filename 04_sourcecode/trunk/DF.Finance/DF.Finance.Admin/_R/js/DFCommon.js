/*
版权信息：jim 2016
作    者：jim
内容说明：公用js类库
*/
DFCommon = {
    /*初始化百度编辑器
     jim.li 2016/05/09
   */
    UELoad: function () {
        var editorIdArr = [];
        $("[type='text/plain']").each(function () {
            var id = $(this).attr("id");
            var ue = UE.getEditor(id);
            editorIdArr.push(id);
        });
        $("#" + editorIdArr[0]).after(' <input type="hidden" name="editId" id="editId" value="' + editorIdArr.join(",") + '" />');

    },
    /*在iframe 中打开新的页面 并增加到导航选项卡中
      jim.li 2016/05/27
      _dataUrl:跳转地址, _menuName 导航名称
    */
    OpenUrl: function (_dataUrl, _menuName) {
        menuItem(_dataUrl, _menuName);
    },
    /*设置权限按钮是否显示
     jim.li 2016/05/20
   */
    Role: function () {
        var navName = $("#hiNavName").val();
        if (!!navName) {
            var role = $.parseJSON($("#hiRole").val());
            if (role != null) {
                if (role.ActionTypeList != null) {
                    $('*[data-role]').each(function () {
                        if ($.inArray($(this).attr("data-role"), role.ActionTypeList) <= -1) { $(this).hide() }
                    })
                }
            }
        }
    }
    ,
    /*
    Ajax 访问Api接口方法
    异步加载请求返回统一使用 RetObj 包裹
    Jim 2016/05/09
    option: 该参数与jQuery.ajax的option参数一致，比jQuery.ajax的option多一项noAccess的回调
    */
    ajax: function (option) {
        var success = option.success;
        var error = option.error;
        var noAccess = option.noAccess;
        delete option.noAccess;
        option.success = function (result) {
            if (result.Code == 1) {
                if (success != null) {
                    success(result.Data, result);
                } else {
                    console.log(result.Data);
                }
            } else if (result.Code == -1) {
                if (noAccess != null) {
                    noAccess(result.Msg, result.Data, result);
                } else {
                    MsgBox.Error(result.Msg)
                }
            } else {
                if (error != null) {
                    error(result.Msg, result);
                } else {
                    MsgBox.Error(result.Msg);
                }
            }
        };
        option.error = function (XMLHttpRequest, textStatus, errorThrown) {
            var msg = textStatus;
            if (typeof (errorThrown) == "undefined") {
                msg = errorThrown;
            }
            if (error != null) {
                error("服务器错误：" + msg);
            } else {
                MsgBox.Error("服务器错误：" + msg);
            }

        };
        $.ajax(option);
    },
    /*
        List 基于bootstrap-table封装
        Jim 2016/05/13
        参数：formObj ：表单元素 #fromId 或者 input[name=fromId] 建议采用id 
        successCallback:加载成功后执行事件
        pagination：是否分页
        clickToSelect:点击行是否选中
        expandRow:展开行事件
    */
    TableInit: function (formObj, successCallback, pagination, clickToSelect) {
        var oTableInit = new Object();
        var jqForm = $(formObj);
        if (pagination != false) {
            pagination = true;
        }
        if (clickToSelect != false) {
            clickToSelect = true;
        }
        //初始化Table
        oTableInit.Init = function (options) {
            options = options || {};
            $(jqForm.attr("data-table")).bootstrapTable({
                url: jqForm.attr("action"),
                method: jqForm.attr("method") ? jqForm.attr("method") : "post",
                toolbar: '#toolbar',
                striped: true,
                cache: false,
                pagination: pagination,
                sortable: true,
                queryParams: oTableInit.queryParams,
                queryParamsType: "limit",
                sidePagination: "server",
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                showColumns: true,
                showRefresh: true,
                minimumCountColumns: 2,              
                clickToSelect: clickToSelect,
                //detailView: true,//父子表
                onExpandRow: options.expandRow == undefined ? null : options.expandRow,
                //是否显示导出
                showExport: options.showExport == undefined ? false : options.showExport,
                //basic', 'all', 'selected'.
                exportDataType: options.exportDataType == undefined ? "selected" : options.exportDataType,
                exportTypes: options.exportTypes == undefined ? ['excel'] : options.exportTypes,
                //编辑单元格数据
                onEditableSave: options.editableSave == undefined ? null : options.editableSave,
                onLoadSuccess: function () {
                    $('.J_menuItem').on('click', menuItem);
                    DFCommon.Role();
                    //设置多选按钮样式
                    //$('input[type=checkbox]').iCheck({
                    //    checkboxClass: 'icheckbox_square-green',
                    //    radioClass: 'iradio_square-green',
                    //});
                    //$(jqForm.attr("data-table")).bootstrapTable('hideColumn', "Title");;
                    successCallback && successCallback();
                }
            });
            //条件查询click事件注册
            $(jqForm.attr("data-query")).click(function () {
                oTableInit.refresh();
            });
        };
        //刷新
        oTableInit.refresh = function () {
            $(jqForm.attr("data-table")).bootstrapTable('refresh');
        }

        //显示列，根据字段名称显示,array为数组
        oTableInit.showColumn = function (array) {
            for (var i = 0; i < array.length; i++) {
                $(jqForm.attr("data-table")).bootstrapTable('showColumn', array[i]);
            }
        }

        //隐藏列，根据字段名称隐藏,array为数组
        oTableInit.hideColumn = function (array) {
            for (var i = 0; i < array.length; i++) {
                $(jqForm.attr("data-table")).bootstrapTable('hideColumn', array[i]);
            }
        }

        //删除
        oTableInit.Delete = function (url) {
            var arrselections = $(jqForm.attr("data-table")).bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                MsgBox.Prompt("错误", "请选择需要删除的数据");
                return;
            }
            MsgBox.Confirm("您确定要删除这条信息吗?", "确认要删除选择的数据",
            function () {
                var ids = [];
                for (var i = 0; i < arrselections.length; i++) {
                    ids.push(arrselections[i].Id);
                }
                DFCommon.ajax({
                    type: 'POST',
                    url: url,
                    data: { ids: ids.join(",") },
                    dataType: "json",
                    success: function (data, res) {
                        if (res.Code == 1) {
                            MsgBox.Success("删除成功", "您已经永久删除了这条信息");
                            $(jqForm.attr("data-table")).bootstrapTable('refresh');
                        }
                        else {
                            MsgBox.Error("错误", res);
                        }
                    }
                });
            });
        };
        //导出Excel
        oTableInit.exportExcel = function () {
            $(jqForm.attr("data-table")).tableExport({
                type: 'excel',
                escape: false
            });
        }
        //得到查询的参数
        oTableInit.queryParams = function (params) {

            return jqForm.serializeObject(params);
        };
        return oTableInit;
    }

}
/*
消息提示框封装
jim.li 2016/5/12
*/
MsgBox = {
    //基本提示
    Prompt: function (title, msg, refresh) {
        if (arguments.length == 0) {
            title = "系统消息";
        }
        swal({
            title: title,
            text: msg
        },
        function (isConfirm) {
            MsgBox.RefreshMode(refresh);
        });
    },
    //成功提示
    Success: function (title, msg, refresh) {
        if (arguments.length == 0) {
            title = "操作成功";
        }
        swal({
            title: title,
            text: msg,
            type: "success"

        },
        function (isConfirm) {
            MsgBox.RefreshMode(refresh);
        });

    },
    //错误提示
    Error: function (title, msg, refresh) {
        if (arguments.length == 0) {
            title = "操作失败";
        }
        swal({
            title: title,
            text: msg,
            type: "error"
        },
        function (isConfirm) {
            MsgBox.RefreshMode(refresh);
        });
    },
    //没有按钮 自动消失timer 时间，allowoutsideclick 是否可以点击关闭
    Timer: function (title, msg, timer, type) {

        swal({ title: title, text: msg, timer: 1000, type: type, showConfirmButton: false, allowOutsideClick: true });

    },
    //确认操作
    Confirm: function confirm(title, msg, callback) {
        swal({
            title: title,
            text: msg,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "确认",
            cancelButtonText: "取消",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                if (callback != null) {
                    callback();
                }

            } else {
                MsgBox.Prompt("已取消", "您取消了该次操作！");
            }
        });
    },
    //不刷新不需要传递refresh参数,刷新方式参数说明：true=刷新当前页，"go"=返回上一页 其他=跳转地址
    RefreshMode: function (refresh) {
        if (refresh) {
            if (refresh == true) {
                window.location.reload();
            } else if (refresh == "go") {
                history.go(-1);
            }
            else {
                window.location.href = refresh;
            }
        }
    }
}

//========================o=model 序列化后的json 把表单元素转换成json data-nojson="" 则不转换 jim 2016-05-12 ========================
$.fn.serializeObject = function (o) {
    if (!o) {
        o = {};
    }
    var a = this.serializeArray();
    $.each(a, function () {
        var _no = !$("input[name='" + this.name + "']").attr("data-nojson");
        //console.log(_no);
        if (_no) {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
//========================o=model 序列化后的json 给表单元素赋值 jim 2016-05-12 ========================
$.fn.initValue = function (o) {
    var formObj = $(this);
    $.each(o, function (key, value) {
        //input
        var inputTextObj = formObj.find("input[type=text][name=" + key + "][data-noval!=false]");
        if (inputTextObj.length) {
            inputTextObj.val(value);
        }
        //textarea
        var textareaObj = formObj.find("textarea[name=" + key + "][data-noval!=false]");
        if (textareaObj.length) {
            textareaObj.val(value);
        }
        //input radio
        var inputRadioObj = formObj.find("input[type=radio][name=" + key + "][data-noval!=false]");
        if (inputRadioObj.length) {
            for (var i = 0; i < inputRadioObj.length; i++) {
                if ($(inputRadioObj[i]).val() == value) {
                    $(inputRadioObj[i]).attr("checked", "checked");
                }
            }
        }
        //input hidden
        var inputHiddenObj = formObj.find("input[type=hidden][name=" + key + "][data-noval!=false]");
        if (inputHiddenObj.length > 0) {
            inputHiddenObj.val(value);
        }
    });
}
//========================格式化字符串 调用方式: var newString = String.format("{1}好{0}好{2}好，{1}还好吗？","我","你","他"); jim 2016-05-16 ========================
String.format = function (s) {
    var t = s;
    for (var i = 1; i < arguments.length; i++) {
        t = t.replace(eval("/\\{" + (i - 1) + "\\}/ig"), arguments[i]);
    }
    return t;
};
// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(H)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 近日(N)有3种情况，今天、昨天、明天、日期
// 例子： 
// (new Date()).format("yyyy-MM-dd HH:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).format("yyyy-M-d H:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (fmt.indexOf('N') >= 0) {
        var wk = "日一二三四五六";
        var td = new Date();
        var tt = td.getDateTime() / 3600000 / 24, ct = this.getDateTime() / 3600000 / 24;
        if (tt == ct) {
            fmt = fmt.replace('N', "今天");
        } else if (tt - ct == 1) {
            fmt = fmt.replace('N', "昨天");
        } else if (ct - tt == 1) {
            fmt = fmt.replace('N', "明天");
        } else if (tt - ct <= 6 && tt > ct) {
            if (this.getDay() > td.getDay() || this.getDay() == 0) {
                fmt = fmt.replace('N', "上周" + wk[this.getDay()]);
            } else {
                fmt = fmt.replace('N', "周" + wk[this.getDay()]);
            }
        } else if (ct - tt <= 6 && (this.getDay() > td.getDay() || this.getDay() == 0) && ct > tt) {
            fmt = fmt.replace('N', "周" + wk[this.getDay()]);
        } else {
            if (td.getYear() == this.getYear()) {
                fmt = fmt.replace('N', "M月d日");
            } else {
                fmt = fmt.replace('N', "yyyy年M月d日");
            }
        }
    }
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};
//========================基于Validform插件========================
//初始化验证表单
$.fn.initValidform = function (postCallbacks) {
    var checkValidform = function (formObj) {
        $(formObj).Validform({
            tiptype: function (msg, o, cssctl) {

                /*msg：提示信息;
                o:{obj:*,type:*,curform:*}
                obj指向的是当前验证的表单元素（或表单对象）；
                type指示提示的状态，值为1、2、3、4， 1：正在检测/提交数据，2：通过验证，3：验证失败，4：提示ignore状态；
                curform为当前form对象;
                cssctl:内置的提示信息样式控制函数，该函数需传入两个参数：显示提示信息的对象 和 当前提示的状态（既形参o中的type）；*/
                //全部验证通过提交表单时o.obj为该表单对象;
                if (!o.obj.is("form")) {
                    //定位到相应的Tab页面 有面板切换在增加此方法切换 
                    //if (o.obj.is(o.curform.find(".Validform_error:first"))) {
                    //    var tabobj = o.obj.parents(".tab-content"); //显示当前的选项
                    //    var tabindex = $(".tab-content").index(tabobj); //显示当前选项索引
                    //    if (!$(".content-tab ul li").eq(tabindex).children("a").hasClass("selected")) {
                    //        $(".content-tab ul li a").removeClass("selected");
                    //        $(".content-tab ul li").eq(tabindex).children("a").addClass("selected");
                    //        $(".tab-content").hide();
                    //        tabobj.show();
                    //    }
                    //}
                    //页面上不存在提示信息的标签时，自动创建;
                    var _parent = o.obj.parents(".form-group");
                    _parent.addClass("has-error");
                    if (_parent.find(".Validform_checktip").length == 0) {
                        o.obj.parents(".form-group").append("<span  class='Validform_checktip help-block m-b-none' />");
                    }
                    var objtip = _parent.find(".Validform_checktip");
                    cssctl(objtip, o.type);

                    objtip.text(msg);
                }
                if (o.type == 2) {
                    o.obj.parents(".form-group").removeClass("has-error");
                }
            },
            showAllError: true,
            beforeSubmit: function (curform) {
                postCallbacks && postCallbacks();
                return false;
            },
            //callback: function (data) {
            //    if (data.status == "y" || data.status == 200) {
            //        postCallbacks && postCallbacks();
            //    }
            //    return false;
            //}
            datatype: {//传入自定义datatype类型              
                "c": /^(\d{3,4}-)?\d{7,8}$/,//传真或电话 
            }
        });
    };
    return $(this).each(function () {
        checkValidform($(this));
    });
}
//文本框只能输入数字，并屏蔽输入法和粘贴  
$.fn.numeral = function () {
    $(this).css("ime-mode", "disabled");
    this.on("keypress", function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);  //兼容火狐 IE      
        if (!$.browser.msie && (e.keyCode == 0x8))  //火狐下不能使用退格键     
        {
            return;
        }
        return code >= 48 && code <= 57;
    });
    this.on("blur", function () {
        if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
            this.value = this.value.substr(0, this.value.length - 1);
        } else if (isNaN(this.value)) {
            this.value = $(this).attr("data-value");
        }
    });
    this.on("paste", function () {
        var s = clipboardData.getData('text');
        if (!/\D/.test(s));
        value = s.replace(/^0*/, '');
        return false;
    });
    this.on("dragenter", function () {
        return false;
    });
    this.on("keyup", function () {
        if (/(^0+)/.test(this.value)) {
            this.value = this.value.replace(/^0*/, '');
        }
    });
};


//初始化需要用到的方法
$(function () {
    //默认选中select值 2016-05-13
    $("select[data-selval]").each(function () {
        $(this).val($(this).attr("data-selval"));
    });
    //判断权限是否显示
    DFCommon.Role();
})


/*
显示或隐藏html元素
hdg 2016/5/18
*/
//显示或隐藏元素，参数array为数组，数组元素为元素的Id名名称,type=1表示隐藏当前元素，不等于1时表示隐藏当前元素的父级的父级（爷爷）元素
display = {
    show: function (type, array) {
        for (var i = 0; i < array.length; i++) {
            if (type == 1) {
                $('#' + array[i]).removeClass("hidden");
            }
            else {
                $('#' + array[i]).parent().parent().removeClass("hidden");
            }

        }
    },
    hidden: function (type, array) {
        for (var i = 0; i < array.length; i++) {
            if (type == 1) {
                $('#' + array[i]).addClass("hidden");
            }
            else {
                $('#' + array[i]).parent().parent().addClass("hidden");
            }

        }
    }
}
/*
弹出框封装
hdg 2016/5/19
*/
//========================基于artdialog插件========================
//弹出一个Dialog窗口,title为窗口标题，content为窗口显示的内容,callback对话框关闭时执行的回调函数
//url为窗口的地址,width为对话框宽度,height为对话框高度,okValue为确定按钮的文字，ok绑定确定按钮的事件
Dialog = {
    //显示链接地址页面的内容
    Url: function (title, content, url, callback, width, height) {
        var d =top.dialog({
            width: width || 300,
            height: height || 480,
            title: title,
            url: url,
            content: content,
            data: window,
            //okValue: '确定',
            //ok: function () { },
            //cancelValue: '取消',
            //cancel: function () { },
            onclose: function () {
                if (callback) {
                    callback();
                }
            }
        }).showModal();
    },
    //直接显示内容
    Content: function (title, content, okTitle, okFun, width, height, callback) {
        var d = top.dialog({
            width: width || 300,
            height: height || 480,
            title: title,
            content: content,
            okValue: okTitle,
            data: window,
            ok: okFun,
            cancelValue: '取消',
            cancel: function () { },
            onclose: function () {
                if (callback) {
                    callback();
                }
            }
        }).showModal();
    }
}



