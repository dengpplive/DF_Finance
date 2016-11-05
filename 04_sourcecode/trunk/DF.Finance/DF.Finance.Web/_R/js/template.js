+function ($) {
    template = {
        PageId: null,
        /*DataPath：json 文件路径
          InitPageId：手工设置初始化那个页面
        */
        Init: function (DataPath, InitPageId, Callback) {
            DFCommon.ajax({
                type: 'get',
                url: DataPath,
                isShowLoading: true,
                async: false,
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                success: function (data) {
                    var gc = data.GlobalConfiguration;
                    var pageId = window.location.hash.replace("#", "");

                    if (!!InitPageId) {
                        gc.HomeId = InitPageId;
                    } else {
                        if (!!pageId) {
                            gc.HomeId = pageId;
                        }
                    }
                    SetPageList(gc, data.ListPage);
                    LoadingData(gc.HomeId, Callback);
                }
            });

            $(document).on("pageAnimationStart", function (e, pageId, $page) {
                LoadingData(pageId);

            });
            /*设置page*/
            function SetPageList(gc, listPage) {
                var html = "";
                $.each(listPage, function (i, item) {
                    var current = "";
                    if (gc.HomeId == item.Id) {
                        current = "page-current";
                    }
                    html += String.format('<div class="page {0} " id="{1}" data-title="{2}{6}" data-pagelinks="{3}" data-arereacquire="{4}" data-preloading="{5}"></div>', current, item.Id, item.Title, item.PageLinks, item.AreReacquire, item.Preloading, gc.Title);
                });
                $("#pageGroup").append(html);

            }
            /*设置数据*/
            function LoadingData(pageId, Callback) {
                LoadingAjax(pageId, true, Callback);
                //设置标题
                $("#title").html($("#" + pageId).attr("data-title"));
                document.title = $("#" + pageId).attr("data-title");
                var $body = $('#body');
                var $iframe = $('<iframe src="/favicon.ico"></iframe>');
                $iframe.on('load', function () {
                    setTimeout(function () {
                        $iframe.off('load').remove();

                    }, 0);
                }).appendTo($body);
            }
            /*请求数据 pageId：页面编号，isComplete：是否执行预加载 */
            function LoadingAjax(pageId, isComplete, Callback) {
                var $obj = $("#" + pageId);
                if (!$obj.find(".content").length || ($obj.attr("data-arereacquire") == "true" && isComplete)) {
                    //处理兑吧 链接
                    if (pageId == "UserSaleIndex") {
                        DFCommon.ajax({
                            type: 'get',
                            url: "/api/Duiba/GetIndexUrl",
                            isShowLoading: true,
                            async: true,
                            success: function (data) {
                                $obj.html(String.format('<iframe src="{0}" id="iframepage" frameborder="0" scrolling="auto"  marginheight="0" marginwidth="0" ></iframe>', data));
                            
                            }
                        });
                    } else {
                        $.ajax({
                            type: 'get',
                            url: $obj.attr("data-pagelinks"),
                            success: function (data) {
                                //必须要到线程中赋值
                                template.PageId = pageId;
                                $obj.html(data);
                                Callback && Callback();
                            }, complete: function () {
                                if (isComplete) {
                                    Preloading($obj);
                                }
                            }
                        });
                    }
                } else {
                    if (isComplete) {
                        //开始执行预加载
                        Preloading($obj);
                    }
                }
            }
            /*预加载方法*/
            function Preloading($obj) {
                var pd = $obj.attr("data-preloading");
                if (pd) {
                    //先取得预加载集合
                    var preloadingArr = pd.split(",");
                    //开始设置预加载的页面
                    for (var i = 0; i < preloadingArr.length; i++) {
                        LoadingAjax(preloadingArr[i], false);
                    }
                }
            }
        },
        /*每个页面拿对象必须要在ready事件中获取 获取当前page对象*/
        PageObj: function () {
            return $("#" + template.PageId);
        }
    }
}(Zepto);