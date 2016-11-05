using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using DF.Finance.Model;
using DF.Finance.BLL;
using DF.Finance.Common;
using System.Web.Http;

namespace DF.Finance.WebCommon
{
    public class BaseAdminController : Controller
    {
        private SysManage _CurrentUser = null;
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public SysManage CurrentUser
        {
            get
            {
                if (_CurrentUser == null)
                {
                    _CurrentUser = WebRunTime.GetCurrentUser();
                }
                return _CurrentUser;
            }
            set
            {
                _CurrentUser = value;
            }
        }
        /// <summary>
        /// 用户的操作日志
        /// </summary>
        /// <param name="actionEnum">操作类型的枚举</param>
        /// <param name="userId">用户的Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public bool OpertionLog(DFEnums.ActionEnum actionEnum, int userId, string userName, string remark = "")
        {
            return WebRunTime.OpertionLog(actionEnum, userId, userName, remark);
        }
        /// <summary>
        /// 当前的登陆用户操作日志
        /// </summary>
        /// <param name="actionEnum">操作类型的枚举</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public bool OpertionLog(DFEnums.ActionEnum actionEnum, string remark = "")
        {
            return WebRunTime.OpertionLog(actionEnum, CurrentUser.Id, CurrentUser.UserName, remark);
        }
        /// <summary>
        /// 检测当前用户是否资源访问权限
        /// </summary>
        /// <param name="navName">导航名称</param>
        /// <param name="actionType">操作类型</param>
        /// <returns>如果有权限返回true,否则返回false</returns>
         protected bool IsRole(string navName, string actionType)
        {
            return WebRunTime.IsRole(navName, actionType);
        }

        #region 处理JSON中的时间格式
        /// <summary>
        /// 处理JSON中的时间格式
        /// </summary>       
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult()
            {
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }
        #endregion

    }
    public class NewtonJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context=null");
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet)
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("无效的操作");
            }
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                context.HttpContext.Response.ContentType = this.ContentType;
            }
            else
            {
                context.HttpContext.Response.ContentType = "text/html";
            }
            if (this.ContentEncoding != null)
            {
                context.HttpContext.Response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                //设置输出的时间格式
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string strData = JsonConvert.SerializeObject(this.Data, Newtonsoft.Json.Formatting.Indented, timeFormat);
                context.HttpContext.Response.Write(strData);
            }
        }
    }
}