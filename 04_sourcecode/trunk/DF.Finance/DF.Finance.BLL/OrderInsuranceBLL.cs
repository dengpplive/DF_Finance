using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderInsuranceBLL : BaseBLL<OrderInsurance>
    {
        /// <summary>
        /// 根据订单Id获取订单的保险信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderInsurance GetByOrderId(int orderId)
        {
            var model = dal.GetList(string.Format("OrderId={0}", orderId), "").FirstOrDefault();
            if (model != null) 
            {
                model.OrderInsuranceImgs = new OrderInsuranceImgBLL().GetList(string.Format(" InsuranceId={0}", model.Id));
            }
            return model != null ? model : new OrderInsurance();

        }
    }
}
