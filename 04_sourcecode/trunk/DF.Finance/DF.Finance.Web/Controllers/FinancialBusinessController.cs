using DF.Finance.Web.Filter;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    /// <summary>
    /// 金融业代
    /// </summary>   
    public class FinancialBusinessController : BaseWebController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 金融业代登录页面
        /// </summary>
        /// <returns></returns>        
        public ActionResult Login(string phone, string pwd)
        {
            return View();
        }
        /// <summary>
        /// ajax请求登录
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        public JsonResult AjaxLogin(string phone, string pwd)
        {
            return UserLogin(phone, pwd);
        }
        /// <summary>
        /// 金融业代注册页面
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult PersonalCenter()
        {
            return View();
        }
        /// <summary>
        /// 录入资料
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult InputData()
        {
            return View();
        }
        /// <summary>
        /// 客户管理
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult CustomerManagement()
        {
            return View();
        }
        /// <summary>
        /// 贷款详情
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult LoanDetails()
        {
            return View();
        }
        /// <summary>
        /// 审核查询页面
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult AuditQuery()
        {
            return View();
        }

        /// <summary>
        /// 客户详情
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult CustomerDetails()
        {
            return View();
        }
    }
}