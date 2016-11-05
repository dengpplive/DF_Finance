using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.WeiXin.Model
{
    /// <summary>
    /// 微信登录成功后该用户的信息
    /// </summary>
    public class Union
    {

        /// <summary>
        /// 用户的微信 openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>

        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>

        public string sex { get; set; }
        /// <summary>
        /// 使用的语言
        /// </summary>

        public string language { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>

        public string headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息
        /// </summary>
        public JArray privilege { get; set; }
        /// <summary>
        /// 用户统一标识
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public string subscribe { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string subscribe_time { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public string groupid { get; set; }
    }

    public class AllUnion
    {
        public Union[] user_info_list { get; set; }
    }

    public class wxRQUser
    {
        public wxParam[] user_list { get; set; }
    }

    public class wxParam
    {
        public string openid { get; set; }
        private string _lang = "zh-CN";
        public string lang { get { return _lang; } set { _lang = value; } }
    }
}
