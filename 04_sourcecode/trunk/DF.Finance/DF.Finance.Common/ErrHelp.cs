using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DF.Finance.Common
{
    public static class ErrHelp
    {
        public static string ServerError = "服务器繁忙，请稍候再试！";
        public static HttpError NotFound(string PTitle)
        {
            if (string.IsNullOrEmpty(PTitle)) PTitle = "信息";
            return new HttpError(string.Format("此{0}不存在", PTitle));
        }
    }
}