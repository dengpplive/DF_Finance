using DF.Finance.Common;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    /// <summary>
    /// 贷款客户
    /// </summary>
    public class CustomersController : BaseWebController
    {
        //首页视图
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
    }
}