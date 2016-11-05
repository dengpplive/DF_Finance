using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("SysManagerRole")]

    public class RoleController : BaseAdminController
    {
        // GET: Role
        public ActionResult Index()
        {
            //管理员类型                     
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("admin_type");
            return View();
        }
        /// <summary>
        /// 查询分页方法
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRole()
        {
            var dic = Utils.GetRequest();
            SysManagerRoleBLL sysManagerRoleBLL = new SysManagerRoleBLL();
            var roleList = sysManagerRoleBLL.GetRoleList(dic);
            return Json(new { total = roleList.TotalItems, rows = roleList.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 快速编辑角色信息
        /// </summary>
        /// <param name="strPostData"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public object AddOrEdit(string model)
        {

            SysManagerRole role = Newtonsoft.Json.JsonConvert.DeserializeObject<SysManagerRole>(model);
            var roleBll = new SysManagerRoleBLL();
            bool result = true;
            if (role.Id > 0)
            {
                result = roleBll.CompleteUpdate(role);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "角色管理");
            }
            else
            {
                result = roleBll.CompleteAdd(role);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "角色管理");
            }
            if (result)
            {
                return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new AdminMessageException("操作失败");
            }

        }
        [HttpGet]
        public ActionResult EditRole(int Id)
        {
            SysManagerRoleBLL sysManagerRoleBLL = new SysManagerRoleBLL();
            var model = new SysManagerRole();
            if (Id > 0)
            {
                model = sysManagerRoleBLL.GetModel(Id);
                SysManagerRoleValueBLL roleValuebll = new SysManagerRoleValueBLL();
                model.RoleValueList = roleValuebll.GetList(string.Format(" RoleId={0}", model.Id));
            }
            //管理员类型                     
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("admin_type");
            ViewBag.title = Id > 0 ? "编辑角色信息" : "添加角色信息";
            ViewBag.DicActionType = Utils.ActionType();

            return View(model);
        }




        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            SysManagerRoleBLL bll = new SysManagerRoleBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.Delete(string.Format(" where Id in({0})  and IsSys<>1", ids));
            }
            return Json(new RetObj("删除成功"), JsonRequestBehavior.AllowGet);
        }
    }
}