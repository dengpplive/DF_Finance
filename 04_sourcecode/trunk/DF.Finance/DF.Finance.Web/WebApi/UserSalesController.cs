using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace DF.Finance.Web.WebApi
{
    /// <summary>
    /// 销售顾问
    /// </summary>
    public class UserSalesController : BaseWebApiController
    {
        /// <summary>
        /// 销售顾问-个人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj SalePersonCenter()
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var userBLL = new UserBLL();
            var data = userBLL.GetPersonCenterData(userId, 2);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="oldEmail">愿邮箱</param>
        /// <param name="newEmail">新邮箱</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj UpdateEmail(string oldEmail, string newEmail)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var userBLL = new UserBLL();
            var result = userBLL.UpdateEmail(userId, oldEmail, newEmail);
            return result;
        }
        /// <summary>
        /// 我的金融业代
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyFinanceList(int pageIndex, int pageSize)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var userSaleBLL = new UserSaleBLL();
            var data = userSaleBLL.GetMyFinanceList(pageIndex, pageSize, userId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 我的金融业代
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="userAgentId">金融业代Id</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyFinanceOrderList(int pageIndex, int pageSize, int userAgentId)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var userSaleBLL = new UserSaleBLL();
            var data = userSaleBLL.GetMyFinanceOrderList(pageIndex, pageSize, userId, userAgentId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 实际交易方列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetCustomerManagementList()
        {
            var bll = new CustomerManagementBLL();
            var data = bll.GetCustomerManagementList();
            return new RetObj(data);
        }

        #region 有关积分的一块
        /// <summary>
        /// 我的积分-销售顾问
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>

        [HttpGet]
        public RetObj GetMyPointView(int pageIndex, int pageSize)
        {
            var userPointLogBLL = new UserPointLogBLL();
            string strUserId = Utils.GetCookie("userId");
            int userSalesId = 0;
            int.TryParse(strUserId, out userSalesId);
            var data = userPointLogBLL.GetMyPointView(pageIndex, pageSize, userSalesId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 获取积分信息
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetPointById(int Id)
        {
            var userPointLogBLL = new UserPointLogBLL();
            var data = userPointLogBLL.GetModel(Id);
            return new RetObj(data);
        }

        #endregion
    }
}
