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
    /// 报表管理
    /// </summary>
    public class ReportManagementController : BaseAdminController
    {
        // GET: ReportManagement
        public ActionResult Index()
        {
            return View();
        }

        #region 页面
        /// <summary>
        /// 公共资源库
        /// </summary>
        /// <returns></returns>
        public ActionResult CommSourceIndex()
        {
            return View();
        }
        /// <summary>
        /// 放款单数汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanSummary()
        {
            return View();
        }
        /// <summary>
        /// 实际交易方成交清单 
        /// </summary>
        /// <returns></returns>
        public ActionResult ActualTradingPartyDealDetailed()
        {
            return View();
        }
        /// <summary>
        /// 金融业代资源信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FinancialResourceInfo()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// 获取公共资源列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCommSourceList()
        {
            var dic = Utils.GetRequest();
            var reportBll = new ReportManagementBLL();
            var list = ReportManagementBLL.GetPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 放款单数汇总
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLoanSummaryList()
        {
            var dic = Utils.GetRequest();
            var reportBll = new ReportManagementBLL();
            var list = ReportManagementBLL.GetLoanSummaryPage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 实际交易方成交清单 数据列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActualTradingPartyDetailList()
        {
            var dic = Utils.GetRequest();
            var reportBll = new ReportManagementBLL();
            var list = ReportManagementBLL.GetActualTradingPartyDetailList(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取录入的订单信息
        /// </summary>
        /// <param name="Id">订单id</param>
        /// <returns></returns>
        public JsonResult GetOrder(int Id)
        {
            var list = new List<Order>();
            var bll = new UserBLL();
            if (Id > 0) list.Add(new OrderBLL().GetModel(Id));
            return Json(new { total = 1, rows = list }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 金融业代资源信息列表数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFinancialResourceList()
        {
            var dic = Utils.GetRequest();
            var reportBll = new ReportManagementBLL();
            var list = ReportManagementBLL.GetFinancialResourcePage(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }


    }
}