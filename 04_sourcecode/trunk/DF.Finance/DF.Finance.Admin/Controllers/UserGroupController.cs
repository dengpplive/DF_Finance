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
    [RoleNavName("UserGroup")]
    public class UserGroupController : BaseAdminController
    {
        //
        // GET: /UserGroup/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search()
        {
            var dic = Utils.GetRequest();
            var UserGroups = new UserGroupBLL().GetPage(dic);
            var total = UserGroups.TotalItems;
            var rows = UserGroups.Items;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(UserGroup UserGroup)
        {
            UserGroupBLL UserGroupBll = new UserGroupBLL();
            if (UserGroup.Id > 0)
            {
                if (!UserGroupBll.Update(UserGroup)) throw new Exception("保存失败");
            }
            else
            {
                UserGroupBll.Add(UserGroup);
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            UserGroupBLL UserGroupBll = new UserGroupBLL();
            var model = new UserGroup();
            if (Id > 0)
            {
                model = UserGroupBll.GetModel(Id);
            }
            return View(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            UserGroupBLL UserGroupBll = new UserGroupBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                UserGroupBll.Delete(ids);
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
    }
}