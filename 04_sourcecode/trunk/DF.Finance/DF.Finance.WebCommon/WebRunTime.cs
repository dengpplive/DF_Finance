/*
版权信息：
作    者：jim.li
完成日期：2016-05-19
内容说明：本系统Web运行时相关信息封装

 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using DF.Finance.Model;
using DF.Finance.BLL;
using DF.Finance.Common;

namespace DF.Finance.WebCommon
{
    /// <summary>
    /// 本系统Web运行时相关信息封装
    /// </summary>
    public class WebRunTime
    {
        /// <summary>
        /// 被许可跨域请求数据的客户端url
        /// </summary>
        public const string WEB_CLIENT_URL = "";//"*";//

        //当前用户的权限代码、地域代码对照
        //const string CURRENT_PRI_INFO = "CURRENT_PRI_INFO";
        /// <summary>
        /// 是否调试
        /// </summary>
        public static int IsDebug
        {
            get
            {
                int IsDebug = 0;
#if DEBUG
                IsDebug = 1;
#endif
                return IsDebug;
            }
        }


        #region GetCurrentUser 获取当前登录的用户
        /// <summary>
        /// 在Session封装当前登录的用户
        /// 用户在某系统登录时加载
        /// </summary>
        static public SysManage GetCurrentUser()
        {
            var CurrentUser = HttpContext.Current.Session["logininfo"] as SysManage;
#if DEBUG
            if (CurrentUser == null)
            {
                CurrentUser = new SysManageBLL().GetModel(31);
                //if (CurrentUser.RoleType != 1)
                //{
                    CurrentUser.Role = new SysManagerRole();
                    CurrentUser.Role.RoleValueList = new SysManagerRoleValueBLL().GetRoleValueByRoleId(CurrentUser.RoleId);
                //}
            }
#endif

            if (CurrentUser == null)
            {
                //查看存储的Cookie信息
                string curname = Utils.GetCookie("curname");
                //如果不为空
                if (!string.IsNullOrEmpty(curname))
                    CurrentUser = ReLogin(Encrypt.Decode(curname));
            }
            return CurrentUser;

        }
        /// <summary>
        /// 重登登陆
        /// </summary>
        /// <param name="username">cookie中获取到的用户名</param>
        /// <param name="filterContext"></param>
        private static SysManage ReLogin(string username)
        {
            var userBll = new SysManageBLL();
            var msg = userBll.UserLogin(username, "", false);
            if (msg.Code == 1)
            {
                HttpContext.Current.Session["logininfo"] = msg.Data;
                return msg.Data;
            }
            return null;
        }
        #endregion


        #region IsLogon 检查用户是否已经登录系统
        /// <summary>
        /// 检查用户是否已经等录系统
        /// </summary>
        /// <returns>是否已等录</returns>
        static public bool IsLogon()
        {
            var u = GetCurrentUser();
            return u != null;
        }

        #endregion

        // 

        // 权限验证
        #region IsAccess 检测当前用户是否资源访问权限
        /// <summary>
        /// 检测当前用户是否资源访问权限
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <param name="navName">导航名称</param>
        /// <param name="actionType">检测资源</param>
        /// <returns>如果有权限返回true，否则返回false</returns>
        static public bool IsRole(SysManage model, string navName, string actionType)
        {

            //超级管理员，拥有所有权限
            if (model.RoleType == 1) //1为超级管理员
            {
                return true;
            }
            return new SysManagerRoleValueBLL().GetList(string.Format(" RoleId={0} and NavName='{1}' and  ActionType='{2}'", model.RoleId, navName, actionType)) != null;
        }
        /// <summary>
        /// 检测当前用户是否资源访问权限
        /// </summary>
        /// <param name="sid">用户sid</param>
        /// <param name="resCode">资源代码</param>
        /// <returns>如果有权限返回true,否则返回false</returns>
        static public bool IsRole(string navName, string actionType)
        {
            return IsRole(GetCurrentUser(), navName, actionType);
        }
        /// <summary>
        /// 页面获得权限
        /// </summary>
        /// <param name="navName"></param>
        /// <returns></returns>
        static public SysManagerRole GetRole(string navName, string actionType, out bool isRole)
        {
            var user = GetCurrentUser();
            if (user.RoleType == 1)
            {
                isRole = true;
                return null;
            }
            //TODO：此处为了开发方便，暂不放入缓存，后期权限改动小，从缓存中拿 jim
            SysManagerRole model = new SysManagerRole()
            {
                RoleType = user.RoleType,
                Id = user.RoleId
            };
            var roleList = new SysManagerRoleValueBLL().GetList(string.Format(" RoleId={0} and NavName='{1}' ", user.RoleId, navName));

            if (roleList == null)
            {
                isRole = false;
            }
            else
            {
                isRole = roleList.Find(p => p.ActionType == actionType) != null;
                model.ActionTypeList = roleList.Select(p => p.ActionType).ToList();
            }

            return model;
        }
        #endregion

        /// <summary>
        /// 记录操作用户操作日志
        /// </summary>
        /// <param name="actionEnum">操作类型的枚举值</param>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public static bool OpertionLog(DFEnums.ActionEnum actionEnum, int userId, string userName, string remark = "")
        {
            var obj = new SysManagerLogBLL().Add(new SysManagerLog()
            {
                ActionType = (byte)actionEnum,
                AddTime = System.DateTime.Now,
                UserId = userId,
                UserIP = IPHelper.GetIP(),
                UserName = userName,
                Remark = "["+actionEnum.ToDesc()+"]:" + remark
            });
            int id = 0;
            return int.TryParse(obj.ObjectToString(), out id) && id > 0;
        }

    }
}

