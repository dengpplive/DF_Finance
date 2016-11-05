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
    [RoleNavName("UserLoginLog")]
    public class UserLoginLogController : BaseAdminController
    {
        //
        // GET: /UserLoginLog/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search()
        {
            var dic = Utils.GetRequest();
            var LoginLogs = new UserLoginLogBLL().GetPage(dic);
            var total = LoginLogs.TotalItems;
            var rows = LoginLogs.Items;
            var strIds = string.Join(",", rows.Select(p => p.UserId).ToArray());
            var Users = new UserBLL().GetList(string.Format(" Id in({0})", strIds));
            foreach (var m in LoginLogs.Items)
            {
                m.User = Users.Where(p => p.Id == m.UserId).FirstOrDefault<User>();
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
	}
}