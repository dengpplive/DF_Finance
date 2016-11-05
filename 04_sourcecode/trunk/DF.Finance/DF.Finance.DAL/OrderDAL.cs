using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class OrderDAL : BaseDAL<Order>
    {
        /// <summary>
        /// 根据提醒规则 拿符合条件的 订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public List<Order> GetOrderAndExtAttributeList(RemindRule model)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" select a.*,b.* ");
            sbSql.Append(" FROM Orders as a");
            sbSql.Append(" left join  OrderExtAttributes as b on a.Id=b.OrderId  ");
            //sbSql.Append(" left join OrderRemindRemark as c on a.Id=c.OrderId ");
            //sbSql.Append(" left join Remind as f on a.Id=f.OrderId ");
            sbSql.AppendFormat(" where DATEDIFF (Day,b.{1} ,GETDATE())>={0} and ", model.Start, model.AnalyzingField);
            sbSql.AppendFormat(" DATEDIFF (Day,b.{0} ,GETDATE())<={1} and a.DeleteFlag=0 and ", model.AnalyzingField, model.End);
            sbSql.AppendFormat(" a.Status in({0}) and ", model.ForOrderState);
            sbSql.AppendFormat(" not exists(select * from dbo.OrderRemindRemark as c where RemindType={0} and a.Id=c.OrderId) ", model.RemindType);
            return db.Fetch<Order,OrderExtAttribute>(sbSql.ToString()); 
        }
        /// <summary>
        /// 根据提醒规则 拿符合条件的 订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public List<Order> GetOrderAndInsuranceList(RemindRule model)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" select a.*,b.* ");
            sbSql.Append(" FROM Orders as a");
            sbSql.Append(" left join  OrderInsurance as b on a.Id=b.OrderId   ");
            sbSql.AppendFormat(" where DATEDIFF (Day,b.{1} ,GETDATE())>={0} and ", model.Start, model.AnalyzingField);
            sbSql.AppendFormat(" DATEDIFF (Day,b.{0} ,GETDATE())<={1} and a.DeleteFlag=0 and ", model.AnalyzingField, model.End);
            sbSql.AppendFormat(" a.Status in({0}) ", model.ForOrderState);
            return db.Fetch<Order,OrderInsurance>(sbSql.ToString()); 
        }
    }
}
