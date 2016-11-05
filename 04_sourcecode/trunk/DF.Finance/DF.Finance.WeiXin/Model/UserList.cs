using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.WeiXin.Model
{
    /// <summary>
    /// 关注该公众号的所有微信用户
    /// </summary>
    public class UserList
    {
        public int total { get; set; }
        public int count { get; set; }

        public OpenIdList data { get; set; }

        public string next_openid { get; set; }
    }

    public class OpenIdList
    {
        public string[] openid { get; set; }
    }
    /// <summary>
    /// 微信服务器IP地址
    /// </summary>
    public class WxIpList
    {
        public string[] ip_list { get; set; }
    }
}
