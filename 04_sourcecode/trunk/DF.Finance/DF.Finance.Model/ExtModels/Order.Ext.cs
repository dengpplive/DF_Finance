using DF.Finance.DBUtility.PetaPoco;
using DF.Finance.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class Order
    {
        /// <summary>
        /// 订单保险信息
        /// </summary>
        public OrderInsurance Insurance { get; set; }

        /// <summary>
        /// 订单提醒备注
        /// </summary>
        public List<OrderRemindRemark> OrderRemindRemarks { get; set; }

        /// <summary>
        /// 订单扩展属性
        /// </summary>
        public OrderExtAttribute OrderExtAttribute { get; set; }

        /// <summary>
        /// 订单操作记录
        /// </summary>
        public List<OrderRecord> OrderRecords { get; set; }

        /// <summary>
        /// 后台订单的状态
        /// </summary>
        public string StatusAdminName { get { return ((OrderStatusEnum)Status).ToAdminString(); } }

        /// <summary>
        /// 客户端 的审核状态显示
        /// </summary>
        public string StatusClientAuditName { get { return ((OrderStatusEnum)Status).ToClientAuditString(); } }

        /// <summary>
        /// 所属金融业代姓名
        /// </summary>
        [ResultColumn]
        public string UserAgentName { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Point
        {
            get
            {
                return (int)LoanAmount;
            }
        }

    }
}
