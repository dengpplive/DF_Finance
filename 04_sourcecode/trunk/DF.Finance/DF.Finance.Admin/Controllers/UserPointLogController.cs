using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("UserPointLog")]
    public class UserPointLogController : BaseAdminController
    {
        //
        // GET: /UserPointLog/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search()
        {
            var dic = Utils.GetRequest();
            var PointLogs = new UserPointLogBLL().GetPage(dic);
            var total = PointLogs.TotalItems;
            var rows = PointLogs.Items;
            var strIds = string.Join(",", rows.Select(p => p.UserId).ToArray());
            var Users = new UserBLL().GetList(string.Format(" Id in({0})", strIds));
            foreach (var m in PointLogs.Items)
            {
                m.User = Users.Where(p => p.Id == m.UserId).FirstOrDefault<User>();
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
	}
}