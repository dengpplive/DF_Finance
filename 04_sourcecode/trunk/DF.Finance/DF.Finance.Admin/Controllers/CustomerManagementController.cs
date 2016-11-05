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
    /// 客户资源管理
    /// </summary>
    [RoleNavName("CustomerManagement")]
    public class CustomerManagementController : BaseAdminController
    {
        // GET: CustomerManagement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCustomerList()
        {
            var dic = Utils.GetRequest();
            CustomerManagementBLL customerManagementBLL = new CustomerManagementBLL();
            var list = customerManagementBLL.GetPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(CustomerManagement customerManagement)
        {
            CustomerManagementBLL customerManagementBLL = new CustomerManagementBLL();
            if (customerManagement.Id > 0)
            {
                if (customerManagement.UserAgentId > 0) customerManagement.DistributionTime = System.DateTime.Now;
                else customerManagement.DistributionTime = Convert.ToDateTime("1900-1-1");
                if (!customerManagementBLL.Update(customerManagement)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "客户资源管理");
            }
            else
            {
                customerManagement.AddTime = System.DateTime.Now;
                if (customerManagement.UserAgentId > 0) customerManagement.DistributionTime = System.DateTime.Now;
                customerManagementBLL.Add(customerManagement);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "客户资源管理");
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
            CustomerManagementBLL customerManagementBLL = new CustomerManagementBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                customerManagementBLL.UpdateDeleteFlag(ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "客户资源管理Ids:[" + ids + "]");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 检测公司名称是否已存在
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public JsonResult Exist(int Id)
        {
            var dic = Utils.GetRequestPost();
            CustomerManagementBLL customerManagementBLL = new CustomerManagementBLL();
            var b = Id <= 0 ? customerManagementBLL.ExistCompany(dic["param"]) : false;
            return Json(new { info = b ? "公司名称已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取金融业代详情
        /// </summary>
        /// <param name="Id">金融业代用户Id</param>
        /// <returns></returns>
        public JsonResult GetUser(int Id)
        {
            var list = new List<User>();
            var bll = new UserBLL();
            if (Id > 0) list.Add(bll.GetModel(Id));
            return Json(new { total = 1, rows = list }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            CustomerManagementBLL customerManagementBLL = new CustomerManagementBLL();
            var model = new CustomerManagement();
            if (Id > 0)
            {
                model = customerManagementBLL.GetModel(Id);
                if (model == null) throw new AdminMessageException("该管实际交易方公司信息不存在");
            }
            ViewBag.title = Id > 0 ? "编辑实际交易方公司信息" : "添加实际交易方公司信息";
            //金融业代用户
            ViewBag.users = new UserBLL().GetUserListByGroupId(1);
            return View(model);
        }
    }
}