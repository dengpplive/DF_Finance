using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.WeiXin.Model
{
    public class WeiXinSetting : ConfigurationSection
    {
        [ConfigurationProperty("wxlogin")]
        public WxLogin WXLogin
        {
            get { return (WxLogin)this["wxlogin"]; }
        }
    }
    /// <summary>
    /// 登录信息设置
    /// </summary>
    public class WxLogin : ConfigurationElement
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("appid", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["appid"]; }
            set { this["appid"] = value; }
        }
        [ConfigurationProperty("appsecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["appsecret"]; }
            set { this["appsecret"] = value; }
        }
        [ConfigurationProperty("callbackurl")]
        public string CallBackUrl
        {
            get { return (string)this["callbackurl"]; }
            set { this["callbackurl"] = value; }
        }
        [ConfigurationProperty("token")]
        public string Token
        {
            get { return (string)this["token"]; }
            set { this["token"] = value; }
        }
    }
}
