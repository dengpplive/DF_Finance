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
    /// 金融业代
    /// </summary>
    public class FinancialBusinessController : BaseWebApiController
    {

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="mobile">注册用户手机号</param>
        /// <param name="verCode">手机验证码</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="area">区域</param>
        /// <returns></returns>
        [HttpPost]
        public RetObj UserRegister([FromBody] UserRegister userRegister)
        {
            var userBLL = new UserBLL();
            return userBLL.UserRegister(new User()
            {
                Mobile = userRegister.mobile,
                UserName = userRegister.mobile,
                Area = userRegister.area,
                PassWord = userRegister.pwd
            }, userRegister.verCode, userRegister.groupId, userRegister.dealersId);
        }

        /// <summary>
        /// 金融业代-个人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj PersonCenter()
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var userBLL = new UserBLL();
            var data = userBLL.GetPersonCenterData(userId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }



        /// <summary>
        /// 我的业绩数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetAchievement(int pageIndex, int pageSize)
        {
            var userAgentBLL = new UserAgentBLL();
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var data = userAgentBLL.GetAchievement(pageIndex, pageSize, userId, true);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 审核查询列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyAuditList(int pageIndex, int pageSize)
        {
            var userAgentBLL = new UserAgentBLL();
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            var data = userAgentBLL.GetAchievement(pageIndex, pageSize, userId, false);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 金融业代的客户管理(实际交易方)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>      
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyCustomUserList(int pageIndex, int pageSize)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            if (userId == 0) return new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆");

            //查询数据
            var customerManagementBLL = new CustomerManagementBLL();
            var pageData = customerManagementBLL.GetMyCustomUserList(pageIndex, pageSize, userId);
            return new RetObj(pageData);
        }
        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cusId">实际交易方公司id</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyCustomUserDetail(int pageIndex, int pageSize, int cusId)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            if (userId == 0) return new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆");

            //查询数据
            var customerManagementBLL = new CustomerManagementBLL();
            var data = customerManagementBLL.GetMyCustomUserDetail(pageIndex, pageSize, userId, cusId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 销售顾问详情
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="saleUserId">销售顾问的用户id</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetMyCustomUserOrderList(int pageIndex, int pageSize, int saleUserId)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            if (userId == 0) return new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆");
            //查询数据
            var customerManagementBLL = new CustomerManagementBLL();
            var data = customerManagementBLL.GetMyCustomUserOrderList(pageIndex, pageSize, saleUserId, userId);
            var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return result;
        }
        /// <summary>
        /// 贷款详情
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetLoanDetail(int orderId)
        {
            var customerManagementBLL = new CustomerManagementBLL();
            var data = customerManagementBLL.GetLoanDetail(orderId);
            if (data == null)
            {
                return new RetObj().Error("该订单不存在");
            }
            return new RetObj(data);
        }
        /// <summary>
        ///  根据车行编号获取所属销售代表
        /// </summary>
        /// <param name="dealersId"></param>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetUserSalesByDealersId(int dealersId)
        {
            return new RetObj(new BLL.UserBLL().GetUserSalesByDealersId(dealersId).Select(p => new { p.Id, p.UserName, p.Mobile }));
        }

    }
}
