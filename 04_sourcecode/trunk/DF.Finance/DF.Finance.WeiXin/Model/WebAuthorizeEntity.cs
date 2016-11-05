using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    /// <summary>
    /// 授权成功后 有关access_token的信息
    /// </summary>
    public class WebAuthorizeEntity
    {
        /// <summary>
        ///  获取资源需要的access_token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期时间(秒) 默认72小时
        /// </summary>
        public string expires_in { get; set; }
        /// <summary>
        /// 刷新token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 登录公众号的用户标识 openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 用户unionid 
        /// </summary>
        public string unionid { get; set; }
    }
}
