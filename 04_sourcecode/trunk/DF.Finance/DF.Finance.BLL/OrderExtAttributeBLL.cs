using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderExtAttributeBLL : BaseBLL<OrderExtAttribute>
    {
        /// <summary>
        /// 根据订单Id获取订单扩展属性信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderExtAttribute GetByOrderId(int orderId)
        {
            var model = dal.GetList(string.Format("OrderId={0}", orderId), "").FirstOrDefault();

            return model != null ? model : new OrderExtAttribute();
        }
    }
}
