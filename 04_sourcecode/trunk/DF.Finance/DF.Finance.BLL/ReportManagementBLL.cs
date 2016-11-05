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
    /// <summary>
    /// 报表管理
    /// </summary>
    public class ReportManagementBLL
    {
        /// <summary>
        /// 获取 3个月实际交易方放款为0的列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public static Page<CustomerManagement> GetPage(Dictionary<string, string> dic, string order = null, bool isPager = true)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select  CustomerManagement.* from  CustomerManagement ");
            sbSQL.Append(" left join Orders on CustomerManagement.Id=Orders.DealersId ");
            sbSQL.Append(" left join OrderExtAttributes on OrderExtAttributes.OrderId=Orders.Id ");
            sbSQL.Append(" where  (LastLoanTime is not null and CONVERT(VARCHAR(10),LastLoanTime,120)<>'1900-01-01') ");
            sbSQL.Append(" and (OrderExtAttributes.ApprovalAmount=0 or  OrderExtAttributes.ApprovalAmount is null  ");
           // sbSQL.Append(" or OrderExtAttributes.ApprovalTime is  null or CONVERT(VARCHAR(10),OrderExtAttributes.ApprovalTime,120)='1900-01-01' ");
            sbSQL.Append(" ) ");
            if (dic.ContainsKey("month") && !string.IsNullOrEmpty(dic["month"]))
            {
                sbSQL.AppendFormat(" And DATEDIFF (month,LastLoanTime,GETDATE())>={0} ", dic["month"].Replace("'", ""));
            }
            else
            {
                sbSQL.AppendFormat(" And DATEDIFF (month,LastLoanTime,GETDATE())>={0} ", 3);
            }
            return new CustomerManagementBLL().GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }

        /// <summary>
        /// 放款单数汇总
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public static Page<CustomerManagement> GetLoanSummaryPage(Dictionary<string, string> dic, string order = null, bool isPager = true)
        {
            StringBuilder sbSQL = new StringBuilder();
            //出单数 已放款日期为有效出单 统计出单总数 
            //贷款总金额为审批通过的金额的总和                            
            sbSQL.Append(" SELECT * FROM CustomerManagement ");
            sbSQL.Append(" inner join (select DealersId,COUNT(*) as IssueOrderCount,SUM(ApprovalAmount) as  TotalLoanAmount from  OrderExtAttributes  ");
            sbSQL.Append(" left join Orders on OrderExtAttributes.OrderId=Orders.Id ");
            sbSQL.Append(" where  ");
            sbSQL.Append(" (LoanTime is not null and CONVERT(VARCHAR(10),LoanTime,120)<>'1900-01-01') ");
            sbSQL.Append(" and (ApprovalTime is not null and CONVERT(VARCHAR(10),ApprovalTime,120)<>'1900-01-01') ");
            //sbSQL.Append(" and LoanTime<=GETDATE() ");
            sbSQL.Append(" and Orders.[Status] in(5,6,99) ");
            sbSQL.Append(" group by Orders.DealersId) as A ");
            sbSQL.Append(" on CustomerManagement.Id=A.DealersId ");

            sbSQL.Append(" where 1=1 ");
            if (dic.ContainsKey("companyname") && !string.IsNullOrEmpty(dic["companyname"]))
            {
                sbSQL.AppendFormat(" And CustomerManagement.CompanyName like '%{0}%' ", dic["companyname"].Replace("'", ""));
            }
            return new CustomerManagementBLL().GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }
        /// <summary>
        /// 实际交易方成交清单
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public static Page<ReportDealDetailsView> GetActualTradingPartyDetailList(Dictionary<string, string> dic, string order = null, bool isPager = true)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select OrderId,LoanTime,UserAgent.UserName AgentUserName,Customer.CompanyName DealersName,ApprovalAmount  ");
            //贷款用户列
            sbSQL.Append(" ,Orders.ApplicantName,Orders.ApplicantPhone,Orders.ApplicantIDCard, ");
            sbSQL.Append(" Orders.LCName,Orders.OldCar,Orders.AcquirerTime,Orders.GuarantorName,Orders.GuarantorPhone,Orders.CoApplicantName,Orders.CoApplicantPhone,Orders.LoanProducts, ");
            sbSQL.Append(" Orders.LoanInterest,Orders.LoanLink,Orders.SalesName,Orders.SalesPhone,Orders.CarBrand,Orders.Models,Orders.CarWorth,Orders.LoanAmount,Orders.LoanTerm ");

            sbSQL.Append("  from OrderExtAttributes  ");
            sbSQL.Append(" inner join Orders on OrderExtAttributes.OrderId=Orders.Id  ");//--订单
            sbSQL.Append(" inner join CustomerManagement as Customer on Orders.DealersId=Customer.Id  ");//--实际交易方
            sbSQL.Append(" inner join Users as UserAgent on Orders.UserAgentId=UserAgent.Id  ");//--金融业代
            //sbSQL.Append(" inner join Users as UserSales on Orders.UserSalesId=UserSales.Id  ");//--销售顾问
            sbSQL.Append(" where  ");
            sbSQL.Append(" (LoanTime is not null and CONVERT(VARCHAR(10),LoanTime,120)<>'1900-01-01') ");
            sbSQL.Append(" and (ApprovalTime is not null and CONVERT(VARCHAR(10),ApprovalTime,120)<>'1900-01-01') ");
            //sbSQL.Append(" and LoanTime<=GETDATE() ");
            sbSQL.Append(" and Orders.[Status] in(5,6,99) ");
            //金融业代姓名
            if (dic.ContainsKey("agantname") && !string.IsNullOrEmpty(dic["agantname"]))
            {
                sbSQL.AppendFormat(" And UserAgent.UserName like '%{0}%' ", dic["agantname"].Replace("'", ""));
            }
            //实际交易方
            if (dic.ContainsKey("companyname") && !string.IsNullOrEmpty(dic["companyname"]))
            {
                sbSQL.AppendFormat(" And Customer.CompanyName like '%{0}%' ", dic["companyname"].Replace("'", ""));
            }
            //贷款用户名
            if (dic.ContainsKey("dkusername") && !string.IsNullOrEmpty(dic["dkusername"]))
            {
                sbSQL.AppendFormat(" And Orders.ApplicantName like '%{0}%' ", dic["dkusername"].Replace("'", ""));
            }
            //放款开始日期
            if (!string.IsNullOrEmpty(dic["loan_starttime"]))
            {
                sbSQL.AppendFormat(" and  LoanTime >='{0}' ", dic["loan_starttime"].Replace("'", ""));
            }
            //放款结束日期
            if (!string.IsNullOrEmpty(dic["loan_endtime"]))
            {
                sbSQL.AppendFormat(" and  LoanTime <='{0} 23:59:59' ", dic["loan_endtime"].Replace("'", ""));
            }
            return new BaseBLL<ReportDealDetailsView>().GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }
        /// <summary>
        /// 获取金融业代关联的实际交易方列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public static Page<CustomerManagement> GetFinancialResourcePage(Dictionary<string, string> dic, string order = null, bool isPager = true)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select CustomerManagement.*,[Users].UserName from [Users] ");
            sbSQL.Append(" left join CustomerManagement on CustomerManagement.UserAgentId=[Users].Id ");
            sbSQL.Append(" where GroupId=1 ");
            if (dic.ContainsKey("username") && !string.IsNullOrEmpty(dic["username"]))
            {
                sbSQL.AppendFormat(" And [Users].UserName like '%{0}%' ", dic["username"].Replace("'", ""));
            }
            if (dic.ContainsKey("companyname") && !string.IsNullOrEmpty(dic["companyname"]))
            {
                sbSQL.AppendFormat(" And CustomerManagement.CompanyName like '%{0}%' ", dic["companyname"].Replace("'", ""));
            }
            if (dic.ContainsKey("companyphone") && !string.IsNullOrEmpty(dic["companyphone"]))
            {
                sbSQL.AppendFormat(" And CustomerManagement.CompanyPhone like '%{0}%' ", dic["companyphone"].Replace("'", ""));
            }
            if (dic.ContainsKey("companyaddress") && !string.IsNullOrEmpty(dic["companyaddress"]))
            {
                sbSQL.AppendFormat(" And CustomerManagement.Address like '%{0}%' ", dic["companyaddress"].Replace("'", ""));
            }
            if (dic.ContainsKey("contacts") && !string.IsNullOrEmpty(dic["contacts"]))
            {
                sbSQL.AppendFormat(" And CustomerManagement.Contacts like '%{0}%' ", dic["contacts"].Replace("'", ""));
            }
            return new CustomerManagementBLL().GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }
    }
}
