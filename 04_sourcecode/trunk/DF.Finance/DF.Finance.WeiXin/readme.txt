ʹ��˵��1.��App.config����web.config�ļ���������½ڵ����ݣ�������Config��WeiXinSetting.xml�Ĳ�������
<configSections>
<!--�Զ������ýڵ�-->
<section name="WeiXinSetting" type="DF.Finance.WeiXin.Model.WeiXinSetting,DF.Finance.WeiXin"/>  
</configSections>
<!--������ƽ̨����-->
<WeiXinSetting configSource="Config\WeiXinSetting.xml"></WeiXinSetting>
2.����Ŀ����ӶԸ���Ŀ�����ã��ڽ���ҳ�����WeiXinManage.StartAcceptRequest()��������WxMsgHandler���е��¼�����