using DF.Finance.BLL;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DF.Finance.Common;
using System.IO;
namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("SysManagerList")]
    public class SysAdminController : BaseAdminController
    {
        // GET: SysAdmin
        public ActionResult Index()
        {
            ViewBag.roleList = new SysManagerRoleBLL().GetList();
            return View();
        }
        [HttpPost]
        public JsonResult GetUserList()
        {
            var dic = Utils.GetRequest();
            SysManageBLL sysManageBLL = new SysManageBLL();
            var list = sysManageBLL.GetPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(SysManage sysManage)
        {
            SysManageBLL sysManageBLL = new SysManageBLL();
            if (sysManage.Id > 0)
            {
                sysManage.PassWord = Encrypt.Encode(sysManage.PassWord, sysManage.Salt);
                if (!sysManageBLL.Update(sysManage)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "管理员管理");
            }
            else
            {
                sysManage.Salt = Encrypt.UniquePWDKey();
                sysManage.PassWord = Encrypt.Encode(sysManage.PassWord, sysManage.Salt);
                sysManage.AddTime = System.DateTime.Now;
                sysManageBLL.Add(sysManage);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "管理员管理");
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
            SysManageBLL sysManageBLL = new SysManageBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                sysManageBLL.Delete(string.Format(" where Id in({0}) and RoleType <>1 ", ids));
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "管理员管理ids:[" + ids + "]");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Exist(int Id)
        {
            var dic = Utils.GetRequestPost();
            SysManageBLL sysManageBLL = new SysManageBLL();
            var b = Id <= 0 ? sysManageBLL.ExistUser(dic["param"]) : false;
            return Json(new { info = b ? "用户已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            SysManageBLL sysManageBLL = new SysManageBLL();
            var model = new SysManage();
            if (Id > 0)
            {
                model = sysManageBLL.GetModel(Id);
                if (model == null) throw new AdminMessageException("该管理员信息不存在");
                model.PassWord = Encrypt.Decode(model.PassWord, model.Salt);
            }                  
            ViewBag.title = Id > 0 ? "编辑管理员信息" : "添加管理员信息";
            ViewBag.roleList = new SysManagerRoleBLL().GetList();
            //管理员类型                     
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("admin_type");
            return View(model);
        }
    }
}