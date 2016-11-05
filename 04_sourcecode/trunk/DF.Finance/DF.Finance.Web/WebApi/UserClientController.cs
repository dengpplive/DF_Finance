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
    /// 贷款客户
    /// </summary>
    public class UserClientController : BaseWebApiController
    {
        /// <summary>
        /// 获取抽奖活动信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetPrizeInfo()
        {
            var activity = PrizeManageBLL.GetActivity("抽奖");
            return new RetObj(activity);
        }
        /// <summary>
        /// 手机号查询中奖信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        [HttpGet]
        public  RetObj GetPrizeResult(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                string strUserId = Utils.GetCookie("userId");
                int userId = 0;
                int.TryParse(strUserId, out userId);
                var userModel = new UserBLL().GetModel(userId);
                if (userModel != null) mobile = userModel.Mobile;
            }
            var userClientBLL = new UserClientBLL();
            var data = userClientBLL.GetPrizeResult(mobile);
            data = PrizeManageBLL.HandleString(data, '*');
            return new RetObj(data);
        }

    }
}
