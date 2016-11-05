using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Filter
{
    /// <summary>
    /// 检测是否登录
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeActionFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //是否添加有需要登录的特性
            bool loginFlag = filterContext.ActionDescriptor.IsDefined(typeof(LoginAttribute), false) ||
                 filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(LoginAttribute), false);
            if (loginFlag)
            {
                //验证是否登录 没有登录跳转到登录页面
                var user = filterContext.HttpContext.Session["user"] as User;
                if (user == null)
                {
                    //查看存储的Cookie信息
                    string mobile = Utils.GetCookie("user");
                    //如果不为空
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        UserBLL bll = new UserBLL();
                        var msg = new RetObj(0, "");
                        msg = bll.Login(mobile, "", false);
                        if (msg.Code == 1) user = msg.Data;
                    }
                }
                if (user == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest()) 
                        filterContext.Result = new JsonResult { Data = new RetObj().Error("您还没有登录") };
                    else filterContext.Result = new RedirectResult("~/FinancialBusiness/Login");                   
                }
            }
        }
    }
}