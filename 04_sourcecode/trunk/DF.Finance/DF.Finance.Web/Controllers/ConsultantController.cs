using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    /// <summary>
    /// 销售顾问
    /// </summary>
    public class ConsultantController : BaseWebController
    {
        // GET: Consultant
        public ActionResult Index()
        {
            return View();
        }
    }
}