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
    [RoleNavName("ActPrize")]
    public class ActPrizeController : BaseAdminController
    {
        // GET: ActPrize
        public ActionResult Index()
        {
            ViewBag.ActivityList = new ActivityBLL().GetList();
            //活动奖项                     
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("award_grade");
            return View();
        }
        public ActionResult Edit(int Id, int ActId = 0)
        {
            ActPrizeBLL bll = new ActPrizeBLL();
            var model = new ActPrize();
            if (ActId != 0) model.ActId = ActId;
            if (Id > 0)
            {
                model = bll.GetModel(Id);
                if (model == null) throw new AdminMessageException("该活动奖品不存在");
            }
            //活动奖项                     
            SysDictionaryBLL dicBll = new SysDictionaryBLL();
            ViewBag.dicModel = dicBll.GetDicByTypeCode("award_grade");
            ViewBag.ActivityList = new ActivityBLL().GetList();
            ViewBag.title = Id > 0 ? "编辑奖品信息" : "添加奖品信息";
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(ActPrize actPrize)
        {
            if (CurrentUser == null) throw new AdminMessageException("请登录后在操作");
            ActPrizeBLL bll = new ActPrizeBLL();
            if (actPrize.Id > 0)
            {
                if (!bll.Update(actPrize)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "抽奖活动奖品信息");
            }
            else
            {
                if (bll.Exist(actPrize.PrizeName, actPrize.PrizeClass))
                    throw new AdminMessageException("该奖项的奖品已存在，请直接进行修改");
                actPrize.AddTime = System.DateTime.Now;
                actPrize.AddUserName = CurrentUser.UserName;
                bll.Add(actPrize);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "抽奖活动奖品信息");
            }
            return Json(new RetObj(1, "保存成功"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Exist(int Id)
        {
            var dic = Utils.GetRequestPost();
            ActPrizeBLL sysManageBLL = new ActPrizeBLL();
            var b = Id <= 0 ? sysManageBLL.ExistPrizeName(dic["param"]) : false;
            return Json(new { info = b ? "奖品名称已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除奖品
        /// </summary>
        /// <param name="Id">Ids</param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            ActPrizeBLL bll = new ActPrizeBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.Delete(string.Format(" where Id in({0})", ids));
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "抽奖活动奖品信息ids:[" + ids + "]");
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询奖品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActPrizeList()
        {
            var dic = Utils.GetRequest();
            ActPrizeBLL bll = new ActPrizeBLL();
            var list = bll.GetActPrizeList(dic);
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
    }
}