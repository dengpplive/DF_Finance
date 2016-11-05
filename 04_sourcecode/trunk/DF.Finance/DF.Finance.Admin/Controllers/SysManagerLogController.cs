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
    /// <summary>
    /// 系统日志
    /// </summary>
    [RoleNavName("SysManagerLog")]
    public class SysManagerLogController : BaseAdminController
    {
        // GET: SysManagerLog
        public ActionResult Index()
        {
            //操作类型
            var oper_type = EnumHelper.GetItemList<DFEnums.ActionEnum>();
            ViewBag.dicModel = oper_type;
            return View();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSysLogList()
        {
            var dic = Utils.GetRequest();
            SysManagerLogBLL sysManagerLogBLL = new SysManagerLogBLL();
            var sysLogList = sysManagerLogBLL.GetSysLogList(dic);
            return Json(new { total = sysLogList.TotalItems, rows = sysLogList.Items }, JsonRequestBehavior.AllowGet);
        }
    }
}