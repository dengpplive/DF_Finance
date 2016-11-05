using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DF.Finance.Common
{
    public class DuibaCommon
    {
        /// <summary>
        /// 获取兑吧URL地址
        /// </summary>
        /// <returns></returns>
        public static string GetIndexUrl(string uid, int credits)
        {
            string url = "http://www.duiba.com.cn/autoLogin/autologin";

            Hashtable hshTable = new Hashtable();
            hshTable.Add("uid", uid);
            hshTable.Add("credits", credits);
            return DuibaHelper.BuildUrlWithSign(url, hshTable);
        }
        /*
        *  积分消耗请求的解析方法,重点在于方法中的三重验证
         *  当用户进行兑换时，兑吧会发起积分扣除请求，开发者收到请求后可以通过此方法进行签名验证与解析，然后返回相应的格式
        */
        public static Hashtable ParseCreditConsume( HttpRequest request)
        {
            if (!request.Params["appKey"].Equals(DuibaHelper.AppKey))
            {
                throw new Exception("appKey not match");
            }
            if (request.Params["timestamp"] == null)
            {
                throw new Exception("timestamp can't be null");
            }
            Hashtable hshTable = DuibaHelper.GetUrlParams(request);

            bool verify = DuibaHelper.SignVerify(hshTable);
            if (!verify)
            {
                throw new Exception("sign verify fail");
            }
            return hshTable;
        }
    }
}
