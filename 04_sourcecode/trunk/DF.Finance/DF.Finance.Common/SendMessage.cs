using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DF.Finance.Common
{
    public class SendMessage
    {
        #region 发送短信内容
        /// <summary>
        /// 发送短信内容
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendMess(string phone, string content)
        {
            string result = "";
            try
            {
                string url = "https://sdk999ws.eucp.b2m.cn/sdkproxy/sendsms.action?"
                        + "cdkey=9SDK-EMY-0999-JDSOT&password=123qaz&phone=" + phone + "&"
                        + "message=【和在线】" + content + "&addserial=";
                System.Net.WebClient client = new System.Net.WebClient();
                string ip = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
                //if (ip == "112.124.41.18")
                {
                    using (System.IO.Stream stream = client.OpenRead(url))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                        {
                            string info = reader.ReadToEnd();
                            info = info.Substring(info.IndexOf("<error>") + 7);
                            info = info.Substring(0, info.IndexOf("</error>"));
                            result = info;
                            reader.Close();
                        }
                        stream.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        #endregion
    }
}
