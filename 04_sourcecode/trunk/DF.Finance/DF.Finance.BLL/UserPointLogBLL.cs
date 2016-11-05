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
    public class UserPointLogBLL : BaseBLL<UserPointLog>
    {
        public Page<UserPointLog> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(dic["UserName"]))
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["S_Time"]))
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["S_Time"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["E_Time"]))
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["E_Time"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }

        /// <summary>
        /// 添加数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteAdd(UserPointLog model)
        {
            return new DAL.UserPointLogDAL().CompleteAdd(model);
        }

        /// <summary>
        /// 销售顾问-我的积分
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="userSalesId">销售顾问Id</param>
        /// <returns></returns>
        public MyPoint GetMyPointView(int pageIndex, int pageSize, int userSalesId)
        {
            var myPoint = new MyPoint();
            string sql = string.Format("select * from  UserPointLog where UserId={0}", userSalesId);
            myPoint.PointList = this.GetPager(pageIndex, pageSize, sql);
            sql = string.Format("select sum(Value) as TotalPoint from  UserPointLog where UserId={0}", userSalesId);
            myPoint.TotalPoint=this.ExecuteScalar<int>(sql);
            return myPoint;
        }

    }
}
