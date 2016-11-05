using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("ActRecord")]
    public class ActRecordController : BaseAdminController
    {
        // GET: ActRecord
        public ActionResult Index()
        {
            ViewBag.ActivityList = new ActivityBLL().GetList();
            return View();
        }
        /// <summary>
        /// 查询活动记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActRecordList()
        {
            var dic = Utils.GetRequest();
            ActRecordBLL bll = new ActRecordBLL();
            var list = bll.GetActRecordList(dic, " AddTime desc ");
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
    }
}