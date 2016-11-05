using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.Common;
using DF.Finance.Common.Assert;
namespace DF.Finance.BLL
{
    /// <summary>
    /// 短信方面的操作
    /// </summary>
    public class SmsBLL
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="url">短信接口地址</param>
        /// <param name="mobiles">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="validtime">失效时间 (分钟)</param>
        /// <returns></returns>

        public string SendSmsCode(string url, string mobile, string code, string key,int validtime)
        {
            string variables = string.Format("code,{0};valid,{1}", code, validtime).UrlEncode();
            string token = this.CreateToken(mobile, key, variables);
            string data = "{0}?mobile={1}&code={2}&variables={3}&token={4}";
            string reqUrl = string.Format(data, url, mobile, key, variables, token);
            string strResult = Utils.HttpGet(reqUrl);
            //开始记录日志
            LogHelper.WriteLog("\r\n接收手机号:" + mobile + "\r\n验证码:" + code + "\r\n返回信息:" + strResult);
            return strResult;
        }
        private string CreateToken(params string[] paramas)
        {
            var listTemp = new List<string>();
            foreach (var p in paramas)
            {
                listTemp.Add(p.Substring(0, 1));
            }
            var listArr = listTemp.ToArray();
            Array.Sort(listArr); //字典排序

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(string.Join("", listArr));
            string token = BitConverter.ToString(md5.ComputeHash(data), 4, 8);

            return token.Replace("-", "");
        }
    }
}
