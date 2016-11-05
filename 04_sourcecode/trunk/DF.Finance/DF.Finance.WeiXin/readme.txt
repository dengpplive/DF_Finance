使用说明1.在App.config或者web.config文件中添加以下节点数据，在配置Config下WeiXinSetting.xml的参数即可
<configSections>
<!--自定义配置节点-->
<section name="WeiXinSetting" type="DF.Finance.WeiXin.Model.WeiXinSetting,DF.Finance.WeiXin"/>  
</configSections>
<!--第三方平台配置-->
<WeiXinSetting configSource="Config\WeiXinSetting.xml"></WeiXinSetting>
2.在项目中添加对该项目的引用，在接收页面调用WeiXinManage.StartAcceptRequest()方法处理WxMsgHandler类中的事件即可