using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common.Json
{
    public class JsonResult
    {
        public string state { get; set; }
        public object data { get; set; }

        private string _msg = "";
        public string msg
        {
            get
            {
                return _msg;
            }
            set
            {
                if (value != null) _msg = value.Replace('"', ' ').Replace('\'', ' ').Replace("\r\n", " ");
            }
        }

        /// <summary>
        /// Json 结果错误码
        /// 0：没有错误, state == "ok"
        /// 1 ：一般性错误, 默认值
        /// 2 :wxId 有误，需要从微信重新登录系统，带入wxId
        /// 3 :查询的信息不存在
        /// </summary>
        public int error_code { get; set; }

        public static string Ok(object data, string message = "")
        {
            JsonResult obj = new JsonResult();
            obj.state = "ok";
            obj.data = data;
            obj.msg = message;
            obj.error_code = 0;
            return JsonHelper.ToJson(obj);
        }

        /// <summary>
        /// 0：没有错误, state == "ok", 1：一般性错误, 默认值,2：wxId 有误，需要从微信重新登录系统，带入wxId ,3：查询的信息不存在
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorCode">0：没有错误, state == "ok", 1 ：一般性错误, 默认值,2 :wxId 有误，需要从微信重新登录系统，带入wxId ,3 :查询的信息不存在</param>
        /// <returns></returns>
        public static string Fail(string msg, int errorCode = 1)
        {
            JsonResult obj = new JsonResult();
            obj.state = "fail";
            obj.msg = msg;
            obj.error_code = errorCode;

            return JsonHelper.ToJson(obj);
        }

        public static string OpenIdFail(string msg)
        {
            return JsonResult.Fail(msg, 2);
        }
    }
}
