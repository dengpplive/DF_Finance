using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// app端销售顾问列表
    /// </summary>
    public class UserSaleView
    {
        /// <summary>
        /// 销售顾问Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 销售顾问姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 销售顾问手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 销售顾问单数
        /// </summary>
        public int OrderCount { get; set; }
    }
}
