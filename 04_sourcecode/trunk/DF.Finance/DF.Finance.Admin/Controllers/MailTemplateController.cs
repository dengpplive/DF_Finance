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
    [RoleNavName("MailTemplate")]
    public class MailTemplateController : BaseAdminController
    {
        // GET: MailTemplate
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public ActionResult Edit(int Id)
        {
            MailTemplateBLL bll = new MailTemplateBLL();
            var model = new MailTemplate();
            if (Id > 0)
            {
                model = bll.GetModel(Id);
                if (model == null) throw new AdminMessageException("该邮件模板不存在");
            }
            ViewBag.title = Id > 0 ? "编辑邮件模板信息" : "添加邮件模板信息";
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="MailTemplate"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit"), ValidateInput(false)]
        public JsonResult AddOrEdit(MailTemplate mailTemplate)
        {
            if (CurrentUser == null) throw new AdminMessageException("请登录后在操作");
            MailTemplateBLL bll = new MailTemplateBLL();
            if (mailTemplate.Id > 0)
            {
                if (!bll.Update(mailTemplate)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "邮件模板管理");
            }
            else
            {
                bll.Add(mailTemplate);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "邮件模板管理");
            }
            return Json(new RetObj(1, "保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 是否存在相同的邮件标题
        /// </summary>
        /// <param name="Id">邮件模板Id</param>
        /// <returns></returns>
        public JsonResult ExistMailTitle(int Id)
        {
            var dic = Utils.GetRequestPost();
            MailTemplateBLL bll = new MailTemplateBLL();
            var b = Id <= 0 ? bll.ExistMailTitle(dic["param"]) : false;
            return Json(new { info = b ? "该邮件标题已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 是否存在相同的邮件模板标题
        /// </summary>
        /// <param name="Id">邮件模板Id</param>
        /// <returns></returns>
        public JsonResult ExistTitle(int Id)
        {
            var dic = Utils.GetRequestPost();
            MailTemplateBLL bll = new MailTemplateBLL();
            var b = Id <= 0 ? bll.ExistTitle(dic["param"]) : false;
            return Json(new { info = b ? "该模板标题已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否存在相同的调用别名
        /// </summary>
        /// <param name="Id">邮件模板Id</param>
        /// <returns></returns>
        public JsonResult ExistCode(int Id)
        {
            var dic = Utils.GetRequestPost();
            MailTemplateBLL bll = new MailTemplateBLL();
            var b = Id <= 0 ? bll.ExistCode(dic["param"]) : false;
            return Json(new { info = b ? "该邮件的调用别名已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除邮件模板列表
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            MailTemplateBLL bll = new MailTemplateBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.DeleteByIds(ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "邮件模板管理ids:["+ids+"]");
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询邮件模板列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMailTemplateList()
        {
            var dic = Utils.GetRequest();
            MailTemplateBLL bll = new MailTemplateBLL();
            var list = bll.GetMailTemplateList(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
    }
}