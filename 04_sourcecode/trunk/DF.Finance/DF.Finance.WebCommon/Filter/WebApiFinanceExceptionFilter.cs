
using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace DF.Finance.WebCommon
{
    /// <summary>
    /// 异常处理过滤器
    /// 2016-03-24 jim
    /// </summary>
    public class WebApiFinanceExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {


            if (context.Exception is AppMessageException)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new RetObj(RetObj.ERR, context.Exception.Message));
            }
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new RetObj(RetObj.ERR, "服务器错误"));
                //开始记录日志
                LogHelper.WriteLog("\r\n客户机IP:" + HttpContext.Current.Request.UserHostAddress + "\r\n错误地址:" + HttpContext.Current.Request.Url + "\r\n异常信息:" + context.Exception.Message, context.Exception);
            }

        }
    }
}
