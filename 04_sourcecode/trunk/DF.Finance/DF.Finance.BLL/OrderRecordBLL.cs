using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderRecordBLL:BaseBLL<OrderRecord>
    {
        /// <summary>
        /// 添加订单操作记录
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="orderId"></param>
        /// <param name="originalState"></param>
        /// <param name="targetState"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public object AddRecord(SysManage admin,int orderId, int originalState, int targetState, string remark) 
        {
            OrderRecord OrderRecord = new OrderRecord();
            OrderRecord.OrderId = orderId;
            OrderRecord.OriginalState = short.Parse(originalState.ToString());
            OrderRecord.TargetState = short.Parse(targetState.ToString());
            OrderRecord.UserId = admin.Id;
            OrderRecord.UserName = admin.UserName;
            OrderRecord.RealName = admin.RealName;
            OrderRecord.Ip = IPHelper.GetIP();
            OrderRecord.AddTime = DateTime.Now;
            OrderRecord.Remark = remark;
            return dal.Add(OrderRecord);
        }


        /// <summary>
        /// 根据订单Id获取订单的操作记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderRecord> GetByOrderId(int orderId) 
        {
            return dal.GetList(string.Format(" OrderId={0}", orderId), "Id asc");
        }

        /// <summary>
        /// 根据查询条件获取订单操作日志列表
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Page<OrderRecord> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if(dic["OrderId"].ToString()!="")
            {
                if (int.Parse(dic["OrderId"]) != 0)//订单
                {
                    sbWhere.AppendFormat(" and OrderId={0} ", dic["OrderId"].Replace("'", ""));
                }
            }
            if (!string.IsNullOrEmpty(dic["UserName"]))//姓名
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["RealName"]))//昵称
            {
                sbWhere.AppendFormat(" and RealName like '%{0}%' ", dic["RealName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["S_Time"]))//收单时间 区间1
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["S_Time"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["E_Time"]))//收单时间 区间2
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["E_Time"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }
    }
}
