using DF.Finance.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXin.Public.Common
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public class WenXinGlobal
    {
        /// <summary>
        /// 获取微信的参数设置信息
        /// </summary>
        /// <returns></returns>
        public static WeiXinSetting GetWeiXinSetting
        {
            get
            {
                return ConfigurationManager.GetSection(Constant.WeiXinSetting_Key) as WeiXinSetting;
            }
        }       
    }
}
