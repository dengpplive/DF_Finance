using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 客户贷款详情
    /// </summary>
    public class LoanDetailView
    {        
        /// <summary>
        /// 订单
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// 订单记录
        /// </summary>
        public OrderRecord OrderRecord { get; set; }

        /// <summary>
        /// 开票单位
        /// </summary>
        public string OpenTicketConpanyName { get; set; }
    }
}
