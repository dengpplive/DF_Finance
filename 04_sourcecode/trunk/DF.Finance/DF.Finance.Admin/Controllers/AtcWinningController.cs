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
    [RoleNavName("AtcWinning")]
    public class AtcWinningController : BaseAdminController
    {
        // GET: AtcWinning
        public ActionResult Index()
        {
            ViewBag.ActivityList = new ActivityBLL().GetList();
            //活动奖项                     
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("award_grade");
            return View();
        }
        /// <summary>
        /// 查询中奖纪录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAtcWinningList()
        {
            var dic = Utils.GetRequest();
            AtcWinningBLL bll = new AtcWinningBLL();
            var list = bll.GetWinningList(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult Edit(AtcWinning atcWinning)
        {
            if (CurrentUser == null) throw new AdminMessageException("请登录后在操作");
            atcWinning.IsAward = 1;
            atcWinning.AwardTime = System.DateTime.Now;
            atcWinning.UpdateTime = System.DateTime.Now;
            atcWinning.UpdateUserName = CurrentUser.UserName;
            AtcWinningBLL bll = new AtcWinningBLL();
            bll.Update(atcWinning, new List<string>(){
            "IsAward",
            "AwardTime",
            "UpdateTime",
            "UpdateUserName"
            });
            //添加日志
            this.OpertionLog(DFEnums.ActionEnum.Edit, "抽奖活动获奖纪录");
            return Json(new RetObj(1, "领奖成功"), JsonRequestBehavior.AllowGet);
        }
    }
}