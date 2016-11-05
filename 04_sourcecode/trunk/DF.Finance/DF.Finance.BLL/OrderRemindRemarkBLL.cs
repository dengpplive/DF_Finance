using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderRemindRemarkBLL : BaseBLL<OrderRemindRemark>
    {
        /// <summary>
        /// 根据订单Id获取订单的提醒备注信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderRemindRemark> GetByOrderId(int orderId)
        {
            var list = dal.GetList(string.Format(" OrderId={0}", orderId), "Id asc");
            return list.Count != 0 ? list : new List<OrderRemindRemark>();
        }
    }
}
