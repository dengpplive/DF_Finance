using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class UserSaleBLL : BaseBLL<UserSale>
    {
        //销售顾问-个人中心-我的金融业代
        /// <summary>
        /// 我的金融业代
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>      
        /// <param name="userSalesId">登陆的销售顾问的Id</param>
        /// <returns></returns>
        public Page<MyFinanceView> GetMyFinanceList(int pageIndex, int pageSize, int userSalesId)
        {
            var result = new Page<MyFinanceView>()
            {
                CurrentPage = 1,
                Items = new List<MyFinanceView>(),
                ItemsPerPage = 1,
                TotalItems = 1,
                TotalPages=1
            };
            var isExistt = new BaseBLL<Order>().Exists(string.Format(" UserSalesId={0} ", userSalesId));
            if (isExistt)
            {
                string sql = string.Format(@"select Users.Id as UserAgentId,Users.UserName,Users.Mobile,COUNT(1) OrderCount from Orders 
                    LEFT join Users on Orders.UserAgentId=Users.Id and Orders.UserAgentId is not null
                    where Orders.DeleteFlag=0 and Orders.UserSalesId={0}
                    group by Users.Mobile,Users.UserName,Users.Id", userSalesId);
                result = new BaseBLL<MyFinanceView>().GetPager(pageIndex, pageSize, sql);
            }
            return result;
        }
        /// <summary>
        /// 销售顾问-个人中心-我的金融业代详情
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>      
        /// <param name="userId">登陆的销售顾问的Id</param>
        /// <param name="userAgentId">金融业代的用户Id</param>
        /// <returns></returns>
        public MyFinanceView GetMyFinanceOrderList(int pageIndex, int pageSize, int userId, int userAgentId)
        {
            var result = new MyFinanceView();
            var userAgent = new BaseBLL<User>().GetModel(userAgentId);
            string sql = string.Format(@"select * from Orders 
                         where Orders.DeleteFlag=0 and Orders.UserSalesId={0} and Orders.UserAgentId={1}",
                userId,
                userAgentId
                );
            var orderlist = new BaseBLL<Order>().GetPager(pageIndex, pageSize, sql);
            result.OrderList = orderlist;
            result.UserAgentId = userAgentId;
            result.UserName = userAgent.UserName;
            result.Mobile = userAgent.Mobile;
            result.OrderCount = (int)orderlist.TotalItems;
            return result;
        }
        /// <summary>
        /// 是否存在车行编号
        /// </summary>
        /// <param name="dealersId">车行编号</param>
        /// <returns></returns>
        public bool IsExistDealersId(int dealersId)
        {
            return this.Exists(string.Format(" DealersId={0}", dealersId));
        }
        /// <summary>       
        /// 销售顾问-我的积分
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="userAgentId">登录的销售顾问的Id</param>       
        /// <returns></returns>
        //public MyPointView GetMyPointView(int pageIndex, int pageSize, int userSalesId)
        //{
        //    var result = new MyPointView();
        //    string sqlFormat = @"select {0} from Orders where DeleteFlag=0 and  UserSalesId={1} ";
        //    string sql = string.Empty;
        //    sql = string.Format("select  UserName from Users where Id={0}", userSalesId);
        //    string userName = this.ExecuteScalar<string>(sql);

        //    sql = string.Format(sqlFormat + " and [Status] in({2}) ", " *,(select UserName from Users where Id=UserAgentId) as UserAgentName", userSalesId, "5,6,99");
        //    var orderlist = new BaseBLL<Order>().GetPager(pageIndex, pageSize, sql);
        //    sql = string.Format(sqlFormat, " isnull(sum(LoanAmount),0) as TotalLoanAmount ", userSalesId, "5,6,99");
        //    decimal totalLoanAmount = this.ExecuteScalar<decimal>(sql);
        //    result.TotalPoint = (int)totalLoanAmount; //   取整数部分       
        //    result.OrderList = orderlist;
        //    result.SaleName = userName;
        //    return result;
        //}


    }
}
