$(function () {
    //通过遍历给菜单项加上data-index属性
    //$(".J_menuItem").each(function (index) {
    //    index = $(".J_menuItem", window.parent.document).length + index;
    //    if (!$(this).attr('data-index')) {
    //        $(this).attr('data-index', index);
    //    }
    //});
    $('.J_menuItem').on('click', menuItem);
})

function menuItem(_dataUrl, _menuName) {
    // 获取标识数据
    var dataUrl = $(this).attr('data-href') || _dataUrl,
        dataIndex = 999,
        menuName = $.trim($(this).attr('data-name')) || _menuName,
        flag = true;
    if (dataUrl == undefined || $.trim(dataUrl).length == 0) return false;

    // 选项卡菜单已存在
    $('.J_menuTab', window.parent.document).each(function () {
        if ($(this).data('id') == dataUrl) {
            if (!$(this).hasClass('active')) {
                $(this).addClass('active').siblings('.J_menuTab').removeClass('active');
                //scrollToTab(this);
                // 显示tab对应的内容区
                $('.J_mainContent .J_iframe', window.parent.document).each(function () {
                    if ($(this).data('id') == dataUrl) {
                        $(this).show().siblings('.J_iframe').hide();
                        return false;
                    }
                });
            }
            flag = false;
            return false;
        }
    });
    // 选项卡菜单不存在
    if (flag) {
        var str = '<a href="javascript:;" class="active J_menuTab" data-id="' + dataUrl + '">' + menuName + ' <i class="fa fa-times-circle"></i></a>';
        $('.J_menuTab', window.parent.document).removeClass('active');

        // 添加选项卡对应的iframe
        var str1 = '<iframe class="J_iframe" name="iframe' + dataIndex + '" width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-id="' + dataUrl + '" seamless></iframe>';
        $('.J_mainContent', window.parent.document).find('iframe.J_iframe').hide().parents('.J_mainContent').append(str1);

        //显示loading提示
        var loading = layer.load();

        $('.J_mainContent iframe:visible', window.parent.document).load(function () {
            //iframe加载完成后隐藏loading提示
            layer.close(loading);
        });
        // 添加选项卡
        $('.J_menuTabs .page-tabs-content', window.parent.document).append(str);
        // scrollToTab($('.J_menuTab.active', window.parent.document));
    }
    return false;
}