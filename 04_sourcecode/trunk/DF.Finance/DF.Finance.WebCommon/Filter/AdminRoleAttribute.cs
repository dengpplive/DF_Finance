
using DF.Finance.Common;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Mvc;

namespace DF.Finance.WebCommon
{

    public class AdminRoleAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 构建方法
        /// </summary>
        public AdminRoleAttribute()
        {
        }
        /// <summary>
        /// 用户权限
        /// </summary>
        public string ActionType { get; set; }
        /// <summary>
        /// 在controller action执行之前调用 
        /// </summary>
        /// <param name="actionContext">controller action内容</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var obj = actionContext.Controller.GetType().GetCustomAttributes(typeof(RoleNavNameAttribute), true);
            var currUser = WebRunTime.GetCurrentUser();
            var _actionType = ActionType;
            //说明没有定义导航权限名称
            if (obj == null || currUser.RoleType == 1 || ActionType == null)
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                var strTypeArr = _actionType.Split('|');
                //如果有俩个权限 则获取id信息
                if (strTypeArr.Length > 1)
                {
                    var id = DTRequest.GetString("Id");
                    if (id == "0")
                    {
                        _actionType = "Add";
                    }
                    else
                    {
                        _actionType = "Edit";
                    }
                }
                var navName = (obj[0] as RoleNavNameAttribute).Value;
                var isRole = currUser.Role.RoleValueList.Find(p => p.NavName == navName && p.ActionType == _actionType);
                if (isRole == null)
                {
                    actionContext.HttpContext.Response.Write(JsonConvert.SerializeObject(new RetObj().NoAccess("没有操作权限")));
                    actionContext.HttpContext.Response.End();
                    actionContext.Result = new ContentResult();
                }
            }
        }

    }
    [AttributeUsage(AttributeTargets.Class)]
    public class RoleNavNameAttribute : Attribute
    {
        public string Value { get; private set; }
        public RoleNavNameAttribute(string navName)
        {
            Value = navName;
        }
    }


}
