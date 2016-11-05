using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 金融业代 -我的业绩数据
    /// </summary>
    public class MyAchievementView
    {
        /// <summary>
        /// 贷款总金额
        /// </summary>
        [Display(Name = "贷款总金额")]
        public decimal LoanTotalAmount { get; set; }
        /// <summary>
        /// 贷款总单数
        /// </summary>
        [Display(Name = "贷款总单数")]
        public int LoanTotalOrderCount { get; set; }

        /// <summary>
        /// 金融业代姓名
        /// </summary>
        [Display(Name = "金融业代姓名")]
        public string UserAgentName { get; set; }

        /// <summary>
        /// 我的订单列表
        /// </summary>
        [Display(Name = "我的订单列表")]
        public Page<Order> OrderList { get; set; }
    }

    /// <summary>
    /// 销售顾问-我的积分
    /// </summary>
    public class MyPointView
    {
        /// <summary>
        /// 总积分
        /// </summary>
        [Display(Name = "总积分")]
        public int TotalPoint { get; set; }
        /// <summary>
        /// 销售顾问姓名
        /// </summary>
        [Display(Name = "销售顾问姓名")]
        public string SaleName { get; set; }

        /// <summary>
        /// 我的订单列表
        /// </summary>
        [Display(Name = "我的订单列表")]
        public Page<Order> OrderList { get; set; }
    }
}
