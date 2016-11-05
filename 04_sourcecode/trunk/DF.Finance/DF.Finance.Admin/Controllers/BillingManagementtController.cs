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
    /// <summary>
    /// 开票方管理
    /// </summary>
    [RoleNavName("BillingManagementt")]
    public class BillingManagementtController : BaseAdminController
    {
        // GET: BillingManagementt
        public ActionResult Index()
        {
            var model = new BillingManagementt();
            return View(model);
        }
        [HttpPost]
        public JsonResult GetBillingManagementtList()
        {
            var dic = Utils.GetRequest();
            BillingManagementtBLL billingManagementtBLL = new BillingManagementtBLL();
            var list = billingManagementtBLL.GetPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(BillingManagementt billingManagementt)
        {
            BillingManagementtBLL billingManagementtBLL = new BillingManagementtBLL();
            if (billingManagementt.Id > 0)
            {
                billingManagementt.UpdateName = CurrentUser.UserName;
                billingManagementt.UpdateUserId = CurrentUser.Id;
                billingManagementt.UpdateTime = System.DateTime.Now;
                if (!billingManagementtBLL.Update(billingManagementt)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "开票方管理");
            }
            else
            {
                billingManagementt.AddName = CurrentUser.UserName;
                billingManagementt.AddUserId = CurrentUser.Id;
                billingManagementt.AddTime = System.DateTime.Now;
                billingManagementtBLL.Add(billingManagementt);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "开票方管理");
            }
            return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            BillingManagementtBLL billingManagementtBLL = new BillingManagementtBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                billingManagementtBLL.UpdateDeleteFlag(ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "开票方管理ids:[" + ids + "]");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 检测开票方公司名称是否已存在
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public JsonResult ExistCompany(int Id)
        {
            var dic = Utils.GetRequestPost();
            BillingManagementtBLL billingManagementtBLL = new BillingManagementtBLL();
            var b = Id <= 0 ? billingManagementtBLL.ExistCompany(dic["param"]) : false;
            return Json(new { info = b ? "开票方公司名称已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
    }
}