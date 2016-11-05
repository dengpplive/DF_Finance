using DF.Finance.Common;
using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 销售顾问-我的金融业代
    /// </summary>
    public class MyFinanceView
    {
        /// <summary>
        /// 金融业代用户Id
        /// </summary>
        public int UserAgentId { get; set; }
        /// <summary>
        /// 金融业代用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 金融业代手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 金融业代贷款订单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 我的订单列表
        /// </summary>
        [Ignore]
        public Page<Order> OrderList { get; set; }
    }
}
