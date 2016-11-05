using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Enums;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("Remind")]
    public class RemindController : BaseAdminController
    {
        //
        // GET: /Remind/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAjaxList()
        {
            var dic = Utils.GetRequest();
            var list = new RemindBLL().GetPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加订单备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult UpdateOrderRemark(OrderRemindRemark model)
        {
            var bll = new OrderRemindRemarkBLL();
            if (bll.Exists(string.Format(" OrderId={0} and RemindType={1} ", model.OrderId, model.RemindType)))
            {
                return Json(new RetObj().Error("已处理过，无需重复处理"), JsonRequestBehavior.AllowGet);
            }
            var order = new OrderBLL().GetModel(model.OrderId);
            bll.Add(model);
            //订单操作记录
            new OrderRecordBLL().AddRecord(CurrentUser, model.OrderId, order.Status, order.Status, string.Format("添加一条{0}备注记录", ((RemindTypeEnums)model.RemindType).ToString()));
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改保险到期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult UpdateInsuranceTime(OrderInsurance model)
        {
            var bll = new OrderInsuranceBLL();
            var list = bll.GetList(string.Format(" OrderId={0} ", model.OrderId));
            if (list == null)
            {
                return Json(new RetObj().Error("保险单据不存在"), JsonRequestBehavior.AllowGet);
            }
            var omodel = list.FirstOrDefault();
            omodel.Remark += "<br>" + model.Remark;
            omodel.MaturityTime = model.MaturityTime;
            var result = bll.Update(omodel);
            if (result)
            {
                base.OpertionLog(DFEnums.ActionEnum.Edit, string.Format("订单号:{0},延长保险时间至：{1},备注内容：{2}", omodel.OrderId, omodel.MaturityTime, model.Remark));
            }
            else
            {
                Json(new RetObj().Error("修改保险信息失败"), JsonRequestBehavior.AllowGet);
            }

            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除 
        ///  AdminRole(ActionType = "Edit")
        ///  此处其实是删除 
        ///  因与修改挂钩 所以直接判断修改权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult RemindDelete(Remind model)
        {
            new RemindBLL().LogicalDelete(model);
            base.OpertionLog(DFEnums.ActionEnum.Delete, string.Format("提醒：{0}，所属编号：{1},提醒类型编号:{2}", ((RemindTypeEnums)model.RemindType).ToString(), model.ViewId, model.RemindType));
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
         [HttpPost, AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            RemindBLL bll = new RemindBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.LogicalDelete(ids);
            }
            return Json(new RetObj("删除成功"), JsonRequestBehavior.AllowGet);
        }
    }
}