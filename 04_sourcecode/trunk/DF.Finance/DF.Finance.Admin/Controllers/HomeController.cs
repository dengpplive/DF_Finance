using DF.Finance.BLL;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
namespace DF.Finance.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public ActionResult Index()
        {
            if (CurrentUser == null) 
            {
                return RedirectToAction("Index", "Login");
            }
            return View(CurrentUser);
        }
        #region 导航相关
        [HttpGet]
        public JsonResult GetNavigation()
        {
            //获得角色信息 先默认角色
            //TODO：jim  此处后期权限改动小，放入缓存中 今后放在登陆方法中 角色和权限缓存起来
            //取得导航集合
            var list = new SysNavigationBLL().GetList(0);
            var sBuilder = new StringBuilder();
            var newList = GetNewNavigationList(sBuilder, list, 0, CurrentUser);
            foreach (var item in newList)
            {
                GetNavigationHtml(sBuilder, item);
            }
            return Json(new RetObj(sBuilder.ToString()), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获得导航菜单
        /// </summary>
        /// <param name="list">导航集合</param>
        /// <param name="parentId">当前节点</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="roleModel">角色信息</param>
        /// <returns></returns>
        private List<SysNavigation> GetNewNavigationList(StringBuilder sBuilder, List<SysNavigation> list, int parentId, SysManage sysManage)
        {

            //取得导航
            var navigationList = list.Where(p => p.IsLock == 0 && p.ParentId == parentId).ToList();
            //新导航集合
            var newList = new List<SysNavigation>();
            //如果不是超级管理员 则检查权限

            foreach (var model in navigationList)
            {

                //如果不是超级管理员则检测权限
                if (sysManage.RoleType > 1)
                {
                    var actionTypeArr = model.ActionType.Split(',');
                    foreach (string actionTypeStr in actionTypeArr)
                    {
                        //如果有显示权限
                        if (actionTypeStr == "Show")
                        {
                            if (sysManage.Role.RoleValueList != null)
                            {
                                var modelt = sysManage.Role.RoleValueList.Find(p => p.NavName == model.Name && p.ActionType == "Show");
                                if (modelt != null)
                                {
                                    model.NavigationList = GetNewNavigationList(sBuilder, list, model.Id, sysManage);
                                    newList.Add(model);
                                    break;
                                }

                            }
                        }
                    }
                }
                else
                {
                    model.NavigationList = GetNewNavigationList(sBuilder, list, model.Id, sysManage);
                    newList.Add(model);
                }

            }
            return newList;
        }
        /// <summary>
        /// 从新递归导航菜单 取的html
        /// </summary>
        /// <param name="sBuilder"></param>
        /// <param name="model"></param>
        private void GetNavigationHtml(StringBuilder sBuilder, SysNavigation model)
        {
            if (model.ParentId == 0)
            {
                sBuilder.AppendFormat("<li> <a href=\"{0}\" {3}><i class=\"{4}\"></i> <span class=\"nav-label\">{1}</span>{2}</a>", string.IsNullOrEmpty(model.LinkUrl) ? "#" : model.LinkUrl, model.Title, model.NavigationList.Count > 0 ? "<span class=\"fa arrow\"></span>" : "", string.IsNullOrEmpty(model.LinkUrl) ? "" : "class=\"J_menuItem\"",model.IconClass);
                if (model.NavigationList.Count > 0)
                {
                    sBuilder.AppendFormat("<ul class=\"nav nav-second-level collapse\">");
                    foreach (var item in model.NavigationList)
                    {
                        GetNavigationHtml(sBuilder, item);
                    }
                    sBuilder.Append("</ul>");
                }
                sBuilder.Append("</li>");
            }
            else
            {
                sBuilder.AppendFormat("<li><a href=\"{0}\" {3} >{1} {2}</a>", string.IsNullOrEmpty(model.LinkUrl) ? "#" : model.LinkUrl, model.Title, model.NavigationList.Count > 0 ? "<span class=\"fa arrow\"></span>" : "", string.IsNullOrEmpty(model.LinkUrl) ? "" : "class=\"J_menuItem\"");
                if (model.NavigationList.Count > 0)
                {
                    sBuilder.AppendFormat("<ul class=\"nav nav-third-level collapse\">");
                    foreach (var item in model.NavigationList)
                    {
                        GetNavigationHtml(sBuilder, item);
                    }
                    sBuilder.Append("</ul>");
                }
                sBuilder.Append("</li>");
            }

        }
        #endregion

        #region 提醒相关
        [HttpGet]
        public JsonResult GetAjaxRemindListByRemindType(int RemindType)
        {
            if (!base.IsRole("Remind","View"))
            {
                return Json(new RetObj(), JsonRequestBehavior.AllowGet);
            }
            return Json(new RetObj(new RemindBLL().GetPageList(1, 15, string.Format(" RemindType={0} and DeleteFlag=0 ", RemindType.ToString())," Id desc ").Items), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Center()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfile()
        {
            return View(CurrentUser);
        }

        public ActionResult SignOut()
        {
            Session.Remove("logininfo");
            this.CurrentUser = null;
            Utils.WriteCookie("curname", "", -1);
            return Redirect("~/Login");
        }

    }
}