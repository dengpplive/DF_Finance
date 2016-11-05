using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Enums;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 实际交易方管理
    /// </summary>
    public class CustomerManagementBLL : BaseBLL<CustomerManagement>
    {
        #region 后台
        /// <summary>
        /// 获取实际交易方列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<CustomerManagement> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbSQL = new StringBuilder();
            // sbSQL.Append("select CustomerManagement.*,Users.* from CustomerManagement ");
            //sbSQL.Append(" left join Users on UserAgentId=Users.Id ");
            sbSQL.Append("  1=1 ");
            if (!string.IsNullOrEmpty(dic["companyname"]))
            {
                sbSQL.AppendFormat(" and CompanyName like '%{0}%' ", dic["companyname"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["companyaddress"]))
            {
                sbSQL.AppendFormat(" and Address like '%{0}%' ", dic["companyaddress"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["companyphone"]))
            {
                sbSQL.AppendFormat(" and CompanyPhone like '%{0}%' ", dic["companyphone"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["contacts"]))
            {
                sbSQL.AppendFormat(" and Contacts like '%{0}%' ", dic["contacts"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["state"]))
            {
                sbSQL.AppendFormat(" and DeleteFlag={0} ", dic["state"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["dis_starttime"]))
            {
                sbSQL.AppendFormat(" and  DistributionTime >='{0}' ", dic["dis_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["dis_starttime"]))
            {
                sbSQL.AppendFormat(" and  DistributionTime <='{0} 23:59:59' ", dic["dis_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["starttime"]))
            {
                sbSQL.AppendFormat(" and  AddTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))
            {
                sbSQL.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }


        /// <summary>
        /// 是否存在该公司名称
        /// </summary>
        /// <param name="userName">公司名称</param>
        /// <returns></returns>
        public bool ExistCompany(string companyName)
        {
            return dal.GetList(string.Format(" CompanyName ='{0}' and DeleteFlag=0 ", companyName), null).SingleOrDefault() != null ? true : false;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool UpdateDeleteFlag(string ids)
        {
            return dal.Update<CustomerManagement>(string.Format(" set  DeleteFlag=1 where Id in({0})", ids)) > 0;
        }
        /// <summary>
        /// 获取超过一个月未交易的实际交易方
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CustomerManagement> GetCustomerManagementList(RemindRule model)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT * ");
            sbSql.Append(" FROM CustomerManagement ");
            sbSql.AppendFormat(" where DATEDIFF (month,{1},GETDATE())>={0} and DeleteFlag=0 ", model.Start, model.AnalyzingField);
            return base.GetListExecute(sbSql.ToString());
        }

        #endregion

        #region 前端  金融业代-个人中心-客户管理
        //金融业代
        //1.我的客户管理
        //2.客户详情
        //3.销售顾问详情
        //4.贷款详情

        /// <summary>
        /// 金融业代的客户管理(实际交易方)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="userId">登陆的金融业代的Id</param>
        /// <returns></returns>
        public Page<CustomerManagement> GetMyCustomUserList(int pageIndex, int pageSize, int userId)
        {
            return this.GetPageList(pageIndex, pageSize, string.Format(" UserAgentId={0}", userId));
        }
        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="dealersId">登陆的金融业代的Id</param>
        /// <param name="dealersId">实际交易方Id</param>
        /// <returns></returns>
        public CustomerManagement GetMyCustomUserDetail(int pageIndex, int pageSize, int userId, int dealersId)
        {
            var customUserDetail = this.GetModel(dealersId);
            if (customUserDetail != null)
            {
                string sql = string.Format(@"
                            select Users.Id as UserId,Users.UserName,Users.Mobile,
                            (
                            select COUNT(*) from Orders where Orders.DealersId={0} and Orders.UserSalesId =UserSales.UserId and Orders.UserAgentId={1}
                            )as OrderCount from UserSales 
                            left join Users on UserSales.UserId=Users.Id
                            where 
                            UserSales.DealersId=DealersId",
                            dealersId,
                            userId
                            );
                var list = new BaseBLL<UserSaleView>().GetPager(pageIndex, pageSize, sql);
                customUserDetail.UserSalesList = list;
            }
            return customUserDetail;
        }
        /// <summary>
        /// 销售顾问详情
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">大小</param>
        /// <param name="userId">销售顾问用户Id</param>
        /// <param name="userAgentId">登陆的金融业代的Id</param>
        /// <returns></returns>
        public UserSales GetMyCustomUserOrderList(int pageIndex, int pageSize, int userId, int userAgentId)
        {
            var userSales = new UserSales();
            userSales.User = new BaseBLL<User>().GetModel(userId);
            string sql = string.Format("select * from Orders where UserSalesId={0} and UserAgentId={1}", userId, userAgentId);
            var list = new BaseBLL<Order>().GetPager(pageIndex, pageSize, sql);
            userSales.OrderList = list;
            return userSales;
        }

        /// <summary>
        /// 贷款详情
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        public LoanDetailView GetLoanDetail(int orderId)
        {
            string sql = string.Format("select * ,(select UserName from Users where Id=UserAgentId) as UserAgentName from Orders where Id={0}", orderId);
            var order = new BaseBLL<Order>().GetListExecute(sql).FirstOrDefault();
            if (order == null) return new LoanDetailView();

            sql = string.Format(@"select top 1 * from OrderRecord where OrderId={0} and TargetState={1} order by Id desc",
               orderId,
               order.Status
               );
            var orderRecord = new BaseBLL<OrderRecord>().GetListExecute(sql).SingleOrDefault();
            sql = string.Format("select * from OrderExtAttributes where OrderId={0}", orderId);
            var orderExtAttribute = new BaseBLL<OrderExtAttribute>().GetListExecute(sql).SingleOrDefault();
            var detail = new LoanDetailView();
            detail.Order = order == null ? new Order() : order;
            detail.OrderRecord = orderRecord == null ? new OrderRecord() : orderRecord;
            detail.OpenTicketConpanyName = orderExtAttribute != null ? orderExtAttribute.BillingUnit : "";
            return detail;
        }
        /// <summary>
        /// 获取实际交易方信息
        /// </summary>
        /// <returns></returns>
        public List<CustomerManagementView> GetCustomerManagementList()
        {
            var result = new List<CustomerManagementView>();
            this.GetList().ForEach(p =>
            {
                result.Add(new CustomerManagementView()
                {
                    Id = p.Id,
                    CompanyName = p.CompanyName
                });
            });
            return result;
        }


        #endregion
    }
}
