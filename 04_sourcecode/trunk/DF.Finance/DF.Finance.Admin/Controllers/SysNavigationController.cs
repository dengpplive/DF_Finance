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
    public class SysNavigationController : BaseAdminController
    {
        //
        // GET: /SysNavigation/
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
            List<SysNavigation> list = new BLL.SysNavigationBLL().GetList(0);
            return Json(new { rows = list }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 选择图标样式
        /// </summary>
        /// <returns></returns>
        public ActionResult DialogIconClass()
        {
            return View();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0, string procType = "edit")
        {

            var bll = new SysNavigationBLL();
            SysNavigation model = new SysNavigation();
            //说明是修改
            if (procType == "edit")
            {
                model = bll.GetModel(id);
            }
            model.ProcType = procType;
            model.Id = id;
            ViewBag.NavDic = SelectParent(0);
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="SysNavigation"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrEdit(SysNavigation model)
        {

            BLL.SysNavigationBLL bll = new SysNavigationBLL();
            bool result = true;
            if (model.ProcType == "edit")
            {
                //如果选择的父ID不是自己,则更改
                if (model.ParentId == model.Id)
                {
                    model.ParentId = model.OldParentId;
                }
                result = bll.Update(model);
            }
            else
            {
                result = bll.Add(model) != null;
            }
            if (!result)
            {
                throw new AdminMessageException("操作失败");
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult Delete(string ids)
        {
            SysNavigationBLL bll = new SysNavigationBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.Delete(string.Format(" where Id in({0})", ids));
            }
            return Json(new RetObj("删除成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 判断导航名称是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public JsonResult ExistName(string param, string oldName)
        {

            SysNavigationBLL bll = new SysNavigationBLL();
            if (param == oldName)
            {
                return Json(new { info = "该导航别名可使用", status = "y" }, JsonRequestBehavior.AllowGet);
            }
            var n = bll.InquireNumber(string.Format(" Name='{0}' ", param)) > 0;
            return Json(new { info = n ? "该导航别名已被占用，请更换！" : "该导航别名可使用", status = n ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改排序字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateSortId(SysNavigation model)
        {
            var result = new SysNavigationBLL().Update(model, new List<string> { "SortId" });
            if (result)
            {
                return Json(new RetObj("修改成功"), JsonRequestBehavior.AllowGet);
            }
            return Json(new RetObj().Error("修改失败"), JsonRequestBehavior.AllowGet);
        }
        #region 私有方法
        /// <summary>
        /// 处理下拉列表层级样式
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private Dictionary<int, string> SelectParent(int parentId)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var list = new SysNavigationBLL().GetList(parentId);
            dic.Add(0, "无父级导航");
            foreach (var model in list)
            {
                if (model.ClassLayer == 1)
                {
                    dic.Add(model.Id, model.Title);
                }
                else
                {
                    var Title = "├ " + model.Title;
                    Title = Utils.StringOfChar(model.ClassLayer - 1, "　") + Title;
                    dic.Add(model.Id, Title);
                }
            }
            return dic;
        }
        #endregion

    }
}