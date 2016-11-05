using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 金融业代扩展
    /// </summary>
    public class UserAgentBLL : BaseBLL<UserAgent>
    {
        /// <summary>
        /// 我的业绩和审核查询列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="userAgentId">登录的金融业代Id</param>
        /// <param name="IsAduilt">是否查询审核订单</param>
        /// <returns></returns>
        public MyAchievementView GetAchievement(int pageIndex, int pageSize, int userAgentId, bool IsAduilt = true)
        {
            var result = new MyAchievementView();
            string sqlFormat = @"select {0} from Orders where DeleteFlag=0 and  UserAgentId={1} ";
            string sql = string.Empty;
            sql = string.Format("select  UserName from Users where Id={0}", userAgentId);
            string userName = this.ExecuteScalar<string>(sql);
            if (IsAduilt)
            {
                sql = string.Format(sqlFormat + " and [Status] in({2}) ", "*", userAgentId, "5,6,99");
                var orderlist = new BaseBLL<Order>().GetPager(pageIndex, pageSize, sql);
                sql = string.Format(sqlFormat, " isnull(sum(LoanAmount),0) as TotalLoanAmount ", userAgentId, "5,6,99");
                decimal totalLoanAmount = this.ExecuteScalar<decimal>(sql);
                result.LoanTotalAmount = totalLoanAmount;
                result.LoanTotalOrderCount = (int)orderlist.TotalItems;
                result.OrderList = orderlist;
            }
            else
            {
                sql = string.Format(sqlFormat, "*", userAgentId);
                var orderlist = new BaseBLL<Order>().GetPager(pageIndex, pageSize, sql);
                result.OrderList = orderlist;
            }
            result.UserAgentName = userName;
            return result;
        }       
    }
}
