using DF.Finance.Common;
using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class UserSales
    {
        public User User { get; set; }

        /// <summary>
        ///  销售订单列表
        /// </summary>
        [ResultColumn]
        public Page<Order> OrderList { get; set; }
    }
}
