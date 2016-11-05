/*
 * Url链接鼠标悬停图片预览
 * powered by jQuery (http://www.jquery.com)
 * 
 * written by Alen Grakalic (http://cssglobe.com)
 * 
 * for more info visit http://cssglobe.com/post/1695/easiest-tooltip-and-image-preview-using-jquery

<a href="#" class="screenshot" rel="url" title="xxxx" _width="xxx">xxxxx</a>
*/

function screenshotPreview() {   
    xOffset = 10;
    yOffset = 30;    
    $(".screenshot").hover(function (e) {
        var This = $(this);        
        var c = (This.attr("title") != undefined && This.attr("title")!='') ? "<br/>" + This.attr("title") : "";
        This.attr("title","");
        $("body").append('<p id="screenshot" style="clear: both;margin: 0;padding: 1px 1px;position: absolute; border: 1px solid #ccc;display: none;"><img src="' + This.attr("rel") + '" alt="" style="border: none;"/>' + c + '</p>');
        $("#screenshot")
			.css("top", (e.pageY - xOffset) + "px")
			.css("left", (e.pageX + yOffset) + "px")
			.fadeIn("fast");
    },
	function () {	 
	    $("#screenshot").remove();
	});
    $(".screenshot").mousemove(function (e) {
        $("#screenshot")
			.css("top", (e.pageY - xOffset) + "px")
			.css("left", (e.pageX + yOffset) + "px");
    });
};

function screenshotPreview1() {
    var xOffset = 20;
    var yOffset = 20;
    var winHeight = $(window).height();
    var winWidth = $(window).width();
    var obj;
    var objHeight = 0;
    var objWidth = 0;

    $(window).resize(function () {
        winHeight = $(window).height();
        winWidth = $(window).width();
    });

    var setHover = function (e) {
        if (objHeight == 0) objHeight = $(obj).find('iframe').height();
        var top = (e.pageY + yOffset);
        var left = (e.pageX + xOffset);
        if (e.pageY > winHeight / 2) top = (e.pageY - objHeight - yOffset);
        if (e.pageX > winWidth / 2) left = (e.pageX - objWidth - xOffset);
        $(obj).css({ 'top': top, 'left': left });
        if ($(obj).css('visibility') == 'hidden') {
            $(obj).css({ 'visibility': 'visible' }).fadeIn('fast');
        }
    };

    $('.screenshot').hover(function (e) {
        var pic = $(this).attr('rel') || $(this).attr('value') || $(this).attr('href');
        if (!pic) return;
        var width = parseInt($(this).attr('_width'));
        if (!width) width = 200;
        objWidth = width + 2;
        $('body').append('<div id="tooltips" style="position:absolute;top:0;left:0;visibility:hidden;color:#fff;z-index:10001;"><img style="position:absolute;left:0;top:0;z-index:10003;border:1px solid #333;" width="' + width + '" src="' + pic + '" /><iframe width="' + objWidth + '" style="position:absolute;border:none;filter:alpha(opacity=0);-moz-opacity:0;-khtml-opacity: 0;opacity:0;z-index:10002;"></iframe></div>');
        var top = (e.pageY + yOffset);
        var left = (e.pageX + xOffset);
        obj = $('#tooltips');
        $(obj).find('img').load(function () { //如果图片从缓存读取，有的浏览器（如chrome）这里不会执行，所以后面再加一个setHover
            objHeight = $(this).height() + 2;
            $(obj).find('iframe').height(objHeight);
            setHover(e);
        });
        setHover(e);
    },
    function () {
        $(obj).remove();
    });

    $('.screenshot').mousemove(function (e) {
        setHover(e);
    });
};
//$(document).ready(function () {
//    screenshotPreview();
//    screenshotPreview1();
//});

