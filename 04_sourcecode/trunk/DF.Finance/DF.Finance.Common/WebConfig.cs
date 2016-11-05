using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class WebConfig
    {
        /// <summary>
        /// 缓存开关
        /// </summary>
        public static readonly bool CachingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["CachingEnabled"]);
        /// <summary>
        /// 其它信息的缓存时间(分钟)，被CachingEnabled控制。
        /// </summary>
        public static readonly int CachingExpires = Convert.ToInt32(ConfigurationManager.AppSettings["CachingExpires"]);
        /// <summary>
        /// 用户信息的缓存时间(分钟)，被CachingEnabled控制。
        /// </summary>
        public static readonly int CachingExpiresForUser = Convert.ToInt32(ConfigurationManager.AppSettings["CachingExpiresForUser"]);
        /// <summary>
        /// 驾校列表的缓存时间(分钟)，不受CachingEnabled控制，0则关闭缓存。
        /// </summary>
        public static readonly int CachingExpiresForJxlist = Convert.ToInt32(ConfigurationManager.AppSettings["CachingExpiresForJxlist"]);
        /// <summary>
        /// 驾校口碑点评列表的缓存时间(分钟)，不受CachingEnabled控制，0则关闭缓存。
        /// </summary>
        public static readonly int CachingExpiresForJxkbdp = Convert.ToInt32(ConfigurationManager.AppSettings["CachingExpiresForJxkbdp"]);
        /// <summary>
        /// 推荐驾校的缓存时间(分钟)，不受CachingEnabled控制，0则关闭缓存。
        /// </summary>
        public static readonly int CachingExpiresForTjjx = Convert.ToInt32(ConfigurationManager.AppSettings["CachingExpiresForTjjx"]);
        /// <summary>
        /// 学车通图片服务器地址。
        /// </summary>
        public static readonly string CoachImgHost = ConfigurationManager.AppSettings["CoachImgHost"];
        /// <summary>
        /// 学车易400电话，用于驾校400电话组成。
        /// </summary>
        public static readonly string Tel400 = ConfigurationManager.AppSettings["Tel400"];
        ///
        /// 400电话服务登录信息的缓存时间(分钟)，不受CachingEnabled控制，0则关闭缓存。
        /// </summary>
        public static readonly int Tel400ServiceLoginInfoCachingExpires = Convert.ToInt32(ConfigurationManager.AppSettings["Tel400ServiceLoginInfoCachingExpires"]);
    }
}
