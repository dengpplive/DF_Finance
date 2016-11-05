using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DF.Finance.WeiXin.Extra
{
    public static class UrlExtender
    {
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncode(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return HttpUtility.UrlEncode(input);
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlDecode(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return HttpUtility.UrlDecode(input);
        }
    }
}
