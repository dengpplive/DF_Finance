using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    public class OrderRecordController : BaseAdminController
    {
        // GET: OrderRecord
        public ActionResult Index(int orderId=0)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]
        public JsonResult Search() 
        {
            var dic = Utils.GetRequest();
            OrderRecordBLL OrderRecordBll = new OrderRecordBLL();
            var OrderRecords = OrderRecordBll.GetPage(dic);
            var total = OrderRecords.TotalItems;
            var rows = OrderRecords.Items;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
    }
}