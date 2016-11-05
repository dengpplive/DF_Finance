using DF.Finance.BLL;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DF.Finance.Common;
namespace DF.Finance.Web.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 选择用户类型页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 抽奖展示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LuckDraw()
        {
            return View();
        }
        /// <summary>
        /// 页面调用 获取抽奖数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            char replaceChar = '*';
            //活动信息 不同时期活动不一样  一个时间段只有一个活动
            var activity = PrizeManageBLL.GetActivity("抽奖");
            //开始获取抽奖数据
            var data = PrizeManageBLL.GetPrizeResult(activity);
            data = PrizeManageBLL.HandleString(data, replaceChar);
            //活动奖项  特等奖 一等奖之类的         
            var prizes = new SysDictionaryBLL().GetDicByTypeCode("award_grade");
            prizes.ForEach(p =>
            {
                p.PrizeCount = data.winners.Where(m => m.type == p.Id).Count();
            });
            return Json(new { data = data, prizes = prizes, activity = activity }, JsonRequestBehavior.AllowGet);
        }
    }
}
