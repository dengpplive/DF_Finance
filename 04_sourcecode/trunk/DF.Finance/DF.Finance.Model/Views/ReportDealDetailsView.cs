using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 实际交易方成交清单 报表详情
    /// </summary>
    public class ReportDealDetailsView
    {
        private DateTime _LoanTime = Convert.ToDateTime("1900-1-1");
        /// <summary>
        /// 放款日期
        /// </summary>
        public DateTime LoanTime
        {
            get
            {
                return _LoanTime;
            }
            set
            {
                _LoanTime = value;
            }
        }
        /// <summary>
        /// 金融业代姓名
        /// </summary>
        public string AgentUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 实际交易方名称
        /// </summary>
        public string DealersName
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款金额 审批通过的金额
        /// </summary>
        public decimal ApprovalAmount { get; set; }

        #region 录入的资料信息
        /// <summary>
        /// 放款金融公司
        /// </summary>
        public string LCName { get; set; }
        /// <summary>
        /// 新车/二手车
        /// </summary>
        public string OldCar { get; set; }
        /// <summary>
        /// 收单日期
        /// </summary>
        public string AcquirerTime { get; set; }
        /// <summary>
        /// 贷款人姓名
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        /// 贷款人电话
        /// </summary>
        public string ApplicantPhone { get; set; }

        /// <summary>
        /// 贷款人身份证号码
        /// </summary>
        public string ApplicantIDCard { get; set; }
        /// <summary>
        /// 担保人姓名
        /// </summary>
        public string GuarantorName { get; set; }
        /// <summary>
        /// 担保人电话
        /// </summary>
        public string GuarantorPhone { get; set; }
        /// <summary>
        /// 共申人姓名
        /// </summary>
        public string CoApplicantName { get; set; }
        /// <summary>
        /// 共申人电话
        /// </summary>
        public string CoApplicantPhone { get; set; }
        /// <summary>
        /// 贷款产品
        /// </summary>
        public string LoanProducts { get; set; }
        /// <summary>
        /// 贷款利息类型
        /// </summary>
        public string LoanInterest { get; set; }
        /// <summary>
        /// 放款环节
        /// </summary>
        public string LoanLink { get; set; }
        /// <summary>
        /// 销售顾问姓名
        /// </summary>
        public string SalesName { get; set; }
        /// <summary>
        /// 销售顾问电话
        /// </summary>
        public string SalesPhone { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public string CarBrand { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        public string Models { get; set; }
        /// <summary>
        /// 车身价
        /// </summary>
        public decimal CarWorth { get; set; }
        /// <summary>
        /// 贷款金额 未审批的金额
        /// </summary>
        public decimal LoanAmount { get; set; }
        /// <summary>
        /// 贷款期限
        /// </summary>
        public string LoanTerm { get; set; }
        #endregion

        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
    }
}
