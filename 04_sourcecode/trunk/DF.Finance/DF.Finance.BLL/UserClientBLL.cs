using DF.Finance.DAL;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class UserClientBLL : BaseBLL<UserClient>
    {
        UserClientDAL _dal = new UserClientDAL();
        /// <summary>
        /// 获取审批通过的用户列表 待抽奖的用户
        /// </summary>
        /// <returns></returns>
        public List<UserClient> GetInvolvedUserList()
        {
            return _dal.GetInvolvedUserList();
        }
        /// <summary>
        /// 获取和查询抽奖信息
        /// </summary>        
        /// <param name="mobile">查询的手机号</param>
        /// <returns></returns>
        public  PrizeView GetPrizeResult(string mobile = "")
        {
            var activity = PrizeManageBLL.GetActivity("抽奖");
            return PrizeManageBLL.GetPrizeResult(activity, mobile);
        }

    }
}
