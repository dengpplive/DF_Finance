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
    /// 字典分类
    /// </summary>
    [RoleNavName("SysDictionaryType")]
    public class SysDictionaryTypeController : BaseAdminController
    {
        // GET: SysDictionaryType
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSysDictionaryTypeList()
        {
            var dic = Utils.GetRequest();
            SysDictionaryTypeBLL sysDictionaryTypeBLL = new SysDictionaryTypeBLL();
            var list = sysDictionaryTypeBLL.GetSysDictionaryTypeList(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage">实体</param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(SysDictionaryType sysDictionaryType)
        {
            SysDictionaryTypeBLL sysDictionaryTypeBLL = new SysDictionaryTypeBLL();
            if (sysDictionaryType.Id > 0)
            {
                if (!sysDictionaryTypeBLL.Update(sysDictionaryType)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "字典管理分类");
            }
            else
            {
                if (sysDictionaryTypeBLL.Exist(sysDictionaryType)) throw new AdminMessageException("该类型已存在");
                sysDictionaryTypeBLL.Add(sysDictionaryType);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "字典管理分类");
            }
            return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            SysDictionaryTypeBLL sysDictionaryTypeBLL = new SysDictionaryTypeBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                sysDictionaryTypeBLL.DeleteByIds(ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "字典管理分类:[" + ids + "]");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }

    }
}