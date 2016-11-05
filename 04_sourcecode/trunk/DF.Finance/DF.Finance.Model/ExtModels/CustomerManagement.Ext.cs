using DF.Finance.Common;
using DF.Finance.DBUtility.PetaPoco;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class CustomerManagement
    {
        /// <summary>
        /// 金融业代用户
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 出单数
        /// </summary>
        [ResultColumn]
        public int IssueOrderCount { get; set; }
        /// <summary>
        /// 总的贷款金额
        /// </summary>
        [ResultColumn]
        public decimal TotalLoanAmount { get; set; }

        /// <summary>
        /// 金融业代的姓名
        /// </summary>
        [ResultColumn]
        public string UserName { get; set; }

        /// <summary>
        ///  销售顾问列表
        /// </summary>
        [ResultColumn]
        public Page<UserSaleView> UserSalesList { get; set; }
    }
}
