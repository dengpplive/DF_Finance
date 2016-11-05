using DF.Finance.Common;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DF.Finance.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //应用程序启动时，自动加载配置log4Net
            LogHelper.SetConfig(new System.IO.FileInfo(Utils.GetMapPath("~/XmlConfig/log4net.config")));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //处理ajax 提交 "" 变成null问题
            ModelMetadataProviders.Current = new EmptyStringDataAnnotationsModelMetadataProvider();
            ////修改WebAPI的返回的时间格式
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
            //    new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 获取系统未处理错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 作    者：jim
        /// 完成时间：2013-09-04
        /// 修 改 人：jim
        /// 修改时间：2016-05-11
        /// </remarks>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
            {
                HttpException exception = (HttpException)ex;
                string sheader = Request.Headers["X-Requested-With"];
                bool isAjaxRequest = (sheader != null && (sheader == "XMLHttpRequest" || sheader == "Delta-true")) ? true : false;
                if (exception.GetHttpCode() == 404)
                {
                    Response.StatusCode = 404;
                    Server.ClearError();
                    Response.ClearContent();
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    if (isAjaxRequest || Request.Url.AbsolutePath.ToLower().StartsWith("/api/"))
                    {
                        LogHelper.WriteLog("api ajax 404", ex);
                        Response.Write(@"{""Code"":0,""Msg"":""服务器找不到请求地址！"",""Data"":{""Status Code"":404}}");
                        Response.ContentType = "application/json";
                    }
                    else
                    {
                        LogHelper.WriteLog("web404", ex);
                        Response.WriteFile(Server.MapPath("~/_R/error/404.html"));
                        Response.ContentType = "text/html";
                    }
                    return;
                }
                else if (exception.GetHttpCode() != 404 && (isAjaxRequest || Request.Url.AbsolutePath.ToLower().StartsWith("/api/")))
                {
                    Response.StatusCode = 500;
                    Server.ClearError();
                    Response.ClearContent();
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    LogHelper.WriteLog("api ajax 500", ex);
                    Response.Write(@"{""Code"":0,""Msg"":""服务器异常！"",""Data"":{""Status Code"":500}}");
                    Response.ContentType = "application/json";
                    return;
                }
            }
            if (ex is AdminMessageException)
            {
                Response.StatusCode = 200;
                Server.ClearError();
                Response.ClearContent();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new RetObj(RetObj.ERR, ex.Message)));
                Response.ContentType = "application/json";
                return;
            }
            LogHelper.WriteLog("500", ex);
            Response.StatusCode = 500;
            Response.WriteFile(Server.MapPath("~/_R/error/500.html"));
        }
    }
}
