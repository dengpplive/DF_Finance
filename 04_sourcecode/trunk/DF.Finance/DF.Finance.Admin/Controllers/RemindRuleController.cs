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
    [RoleNavName("RemindRule")]
    public class RemindRuleController : BaseAdminController
    {
        //
        // GET: /RemindRule/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAjaxList()
        {
            return Json(new { rows = new RemindRuleBLL().GetList() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult AddOrEdit(RemindRule model)
        {
            RemindRuleBLL bll = new RemindRuleBLL();
            bll.Update(model);
            return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
        }


    }
}