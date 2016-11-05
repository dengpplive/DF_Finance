using DF.Finance.WebCommon;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DF.Finance.BLL;
using DF.Finance.Common;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("Activity")]
    public class ActivityController : BaseAdminController
    {
        // GET: Activity
        public ActionResult Index()
        {
            //活动类型   
            ViewBag.dicModel = new SysDictionaryBLL().GetDicByTypeCode("act_type");
            return View();
        }

        public ActionResult Edit(int Id)
        {
            ActivityBLL bll = new ActivityBLL();
            var model = new Activity();
            model.StartTime = System.DateTime.Now;
            model.EndTime = System.DateTime.Now.AddMonths(1);
            if (Id > 0)
            {
                model = bll.GetModel(Id);
                if (model == null) throw new AdminMessageException("该活动已下线");
            }
            //活动类型   
            SysDictionaryBLL dicBll = new SysDictionaryBLL();
            ViewBag.dicModel = dicBll.GetDicByTypeCode("act_type");
            ViewBag.title = Id > 0 ? "编辑活动信息" : "添加活动信息";
            return View(model);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="sysManage"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Add|Edit"), ValidateInput(false)]
        public JsonResult AddOrEdit(Activity activity)
        {
            if (CurrentUser == null) throw new AdminMessageException("请登录后在操作");
            ActivityBLL bll = new ActivityBLL();
            if (activity.Id > 0)
            {
                activity.UpdateTime = System.DateTime.Now;
                activity.UpdateUserName = CurrentUser.UserName;
                activity.EndTime = Convert.ToDateTime(activity.EndTime.ToString("yyyy-MM-dd") + " 23:59:59");
                if (!bll.Update(activity)) throw new AdminMessageException("保存失败");
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Edit, "抽奖活动信息");
            }
            else
            {
                activity.AddTime = System.DateTime.Now;
                activity.AddUserName = CurrentUser.UserName;
                activity.DeleteFlag = 0;
                activity.EndTime = Convert.ToDateTime(activity.EndTime.ToString("yyyy-MM-dd") + " 23:59:59");
                bll.Add(activity);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Add, "抽奖活动信息");
            }
            return Json(new RetObj(1, "保存成功"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 是否存在相同的活动名称
        /// </summary>
        /// <param name="Id">活动Id</param>
        /// <returns></returns>
        public JsonResult Exist(int Id)
        {
            var dic = Utils.GetRequestPost();
            ActivityBLL sysManageBLL = new ActivityBLL();
            var b = Id <= 0 ? sysManageBLL.ExistActivityName(dic["param"]) : false;
            return Json(new { info = b ? "该活动名称已存在" : "", status = b ? "n" : "y" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除活动列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            ActivityBLL bll = new ActivityBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.UpdateDeleteFlag(ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "抽奖活动信息ids:[" + ids + "]");
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询活动列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActivityList()
        {
            var dic = Utils.GetRequest();
            ActivityBLL bll = new ActivityBLL();
            var list = bll.GetActivityList(dic, " SortId asc ");
            return Json(new { total = list.TotalItems, rows = list.Items }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 手动生成抽奖数据
        /// </summary>
        /// <param name="Id">活动</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public JsonResult HandCreatePrizeData(Activity activity)
        {
            PrizeManageBLL.StartPrize(activity);
            //添加日志
            this.OpertionLog(DFEnums.ActionEnum.Build, "手动生成抽奖数据");
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
    }
}