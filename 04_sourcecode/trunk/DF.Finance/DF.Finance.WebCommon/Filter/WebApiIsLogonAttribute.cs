using System.Net.Http;
using System.Web.Http.Controllers;
using System.Net;
using DF.Finance.Model;
using DF.Finance.Common;
using DF.Finance.BLL;
namespace System.Web.Http.Filters
{
    public class WebApiIsLogonAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 构建方法
        /// </summary>
        public WebApiIsLogonAttribute()
        {
        }

        /// <summary>
        /// 在controller action执行之前调用 
        /// </summary>
        /// <param name="actionContext">controller action内容</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //是否手机发出的请求 可以做限制
            var IsPhone = IPHelper.IsPhone();
#if DEBUG
            IsPhone = true;
#endif
            //查看存储的Cookie信息
            string userId = Utils.GetCookie("userId");
            int _userId = 0;
            int.TryParse(userId, out _userId);//new UserBLL().GetModel(_userId)
            if (string.IsNullOrEmpty(userId) || !IsPhone)
            {
                LogHelper.WriteLog("登录凭证验证无效！");
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, new RetObj(RetObj.NO_ACCESS, "登录凭证验证无效！"));
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        /// <summary>
        /// 在controller action执行之后调用 
        /// </summary>
        /// <param name="actionExecutedContext">controller action内容</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
