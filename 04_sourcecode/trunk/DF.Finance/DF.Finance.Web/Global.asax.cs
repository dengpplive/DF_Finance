
using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DF.Finance.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //应用程序启动时，自动加载配置log4Net
            LogHelper.SetConfig(new System.IO.FileInfo(Utils.GetMapPath("~/xmlconfig/log4net.config")));
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // GlobalFilters.Filters.Add(new AuthorizeActionFilter());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // 使api返回为json 
            // GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
            {
                HttpException exception = (HttpException)ex;
                string sheader = Request.Headers["X-Requested-With"];
                bool isAjaxRequest = (sheader != null && (sheader == "XMLHttpRequest" || sheader == "Delta-true")) ? true : false;

                if (exception.GetHttpCode() != 404 && (isAjaxRequest || Request.Url.AbsolutePath.ToLower().StartsWith("/api/")) || ex is AppMessageException)
                {
                    Response.StatusCode = 500;
                    Server.ClearError();
                    Response.ClearContent();
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    LogHelper.WriteLog("api出错啦！");
                    Response.Write(@"{""Code"":0,""Msg"":""服务器异常！"",""Data"":{""Status Code"":500}}");
                    Response.ContentType = "application/json";
                    return;
                }
            }
           


        }
        /// <summary>
        /// 2016-04-13
        /// jim 
        /// 1.增加获取用户信息 把用户信息放到Cache中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //判断是否是api 访问，是API访问把用户信息放到 Cache中
            //            if (Request.Url.AbsolutePath.ToLower().StartsWith("/api/"))
            //            {

            //                string _token = Request.Headers["token"];
            //                var service = new UserServiceSoapClient();
            //                var user = service.GetUserBy(_token);
            //                if (user != null)
            //                {
            //                    //把用户信息放到Cache中
            //                    CacheHelper.Insert(_token, user);
            //                }
            //#if DEBUG
            //                if (user == null)//&& DTRequest.GetHost().ToLower().StartsWith("api.xuecheyi.net")
            //                {
            //                    user = new User
            //                   {
            //                       id = 729,
            //                       nick_name = "陈广敬",
            //                       user_name = "cdcgj"
            //                   };
            //                    //把用户信息放到Cache中
            //                    CacheHelper.Insert("_token", user);
            //                }
            //#endif
            //            }

        }
        /// <summary>
        /// 2016-04-13
        /// jim 
        /// 1.请求结束清理掉Cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //if (Request.Url.AbsolutePath.ToLower().StartsWith("/api/"))
            //{
            //    var _token = Request.Headers["token"];
            //    if (string.IsNullOrEmpty(_token))
            //    {
            //        _token = "_token";
            //    }
            //    //移除掉用户 Cache
            //    CacheHelper.Remove(_token);
            //}
        }
    }
}
