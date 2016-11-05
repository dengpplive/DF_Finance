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
    /// 字典数据
    /// </summary>
    [RoleNavName("SysDictionary")]
    public class SysDictionaryController : BaseAdminController
    {
        // GET: SysDictionary
        public ActionResult Index()
        {
            var bll = new SysDictionaryTypeBLL();
            ViewBag.typeList = bll.GetList().Where(p => p.IsSys == 0).ToList();
            var model = new SysDictionary();
            ViewBag.leftTree = bll.GetDicData();
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysDictionary"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEditDic(SysDictionary sysDictionary)
        {
            SysDictionaryBLL sysDictionaryBLL = new SysDictionaryBLL();
            if (sysDictionary.Id > 0)
            {
                if (!sysDictionaryBLL.Update(sysDictionary)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "字典管理");
            }
            else
            {
                if (sysDictionaryBLL.Exist(sysDictionary)) throw new AdminMessageException("该名称已存在");
                sysDictionaryBLL.Add(sysDictionary);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "字典管理");
            }
            return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>      
        [HttpPost, AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string Ids)
        {
            SysDictionaryBLL sysDictionaryBLL = new SysDictionaryBLL();
            string[] strArr = Ids.Split(',');
            if (strArr.Length > 0)
            {
                if (strArr.Length == 1 && int.Parse(strArr[0]) == 0) throw new AdminMessageException("请选择需要删除的字典数据");
                //逻辑删除
                sysDictionaryBLL.LogicalDelete(Ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "字典管理ids:[" + Ids + "]");
            }
            else
            {
                throw new AdminMessageException("请选择需要删除的字典数据");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DicManageIndex(int TypeId)
        {
            if (TypeId <= 0) throw new AdminMessageException("该字典分类不存在");
            var SysDictionaryType = new SysDictionaryTypeBLL().GetModel(TypeId);
            if (SysDictionaryType == null) throw new AdminMessageException("该字典分类不存在");
            return View(SysDictionaryType);
        }
        /// <summary>
        /// 获取字典管理列表 带分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDicManageList()
        {
            var dic = Utils.GetRequest();
            SysDictionaryBLL bll = new SysDictionaryBLL();
            var type = new SysDictionaryTypeBLL().GetModel(dic["Id"]);
            var dicView = bll.GetDicListByDicType(dic, type, 0);
            return Json(new { total = dicView.Total, rows = dicView.SysDictionaryList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 字典列表编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult DicManageEdit(int Id, int typeId, int OperType)
        {
            SysDictionaryBLL bll = new SysDictionaryBLL();
            var model = new SysDictionary();
            model.DicType = typeId;
            if (Id > 0) model = bll.GetModel(Id);
            if (model == null) throw new AdminMessageException("该字典数据不存在");
            if (OperType == 0)
            {

            }
            else if (OperType == 1)
            {
                model.ParentId = model.Id;
                model.Name = "";
                model.Val = "";
                model.Remark = "";
            }

            model.OperType = OperType;
            var type = new SysDictionaryTypeBLL().GetModel(typeId);
            model.TypeCode = type.TypeCode;
            ViewBag.type = type;
            ViewBag.NavDic = SelectParent(type);
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysDictionary"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEditDicManage(SysDictionary sysDictionary)
        {
            SysDictionaryBLL sysDictionaryBLL = new SysDictionaryBLL();
            if (sysDictionary.OperType == 1)
            {
                if (sysDictionaryBLL.Exist(sysDictionary)) throw new AdminMessageException("该名称已存在");
                sysDictionaryBLL.Add(sysDictionary);
            }
            else
            {
                if (sysDictionary.Id > 0)
                {
                    if (sysDictionary.ParentId == sysDictionary.Id)
                    {
                        sysDictionary.ParentId = sysDictionary.OldParentId;
                    }
                    if (!sysDictionaryBLL.Update(sysDictionary)) throw new AdminMessageException("保存失败");
                }
            }
            return Json(new RetObj("保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 选择父级
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        private Dictionary<int, string> SelectParent(SysDictionaryType type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var dcView = new SysDictionaryBLL().GetDicListByDicType(null, type, 0, false);
            var list = dcView.SysDictionaryList.Where(p => p.Val != type.TypeCode).ToList();
            dic.Add(0, type.TypeName);
            list.ForEach(p =>
            {
                var Title = "├ " + p.Name;
                Title = Utils.StringOfChar(p.Level, "　") + Title;
                dic.Add(p.Id, Title);
            });
            return dic;
        }

        [HttpGet]
        public JsonResult GetDictByParentId(int ParentId = 0)
        {
            var List = new SysDictionaryBLL().GetListByParentId(ParentId);
            var msg = new RetObj();
            msg.Data = List;
            msg.Code = 1;
            msg.Msg = "请求成功";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDicByTypeCode(string TypeCode, int parentId = 0)
        {
            var list = new SysDictionaryBLL().GetDicByTypeCode("area_city", parentId);
            var msg = new RetObj();
            msg.Data = list;
            msg.Code = 1;
            msg.Msg = "请求成功";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}