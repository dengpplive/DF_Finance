using DF.Finance.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class OrderRecord
    {
        /// <summary>
        /// 后台订单的状态
        /// </summary>
        public string OriginalStatusAdmin { get { return ((OrderStatusEnum)OriginalState).ToAdminString(); } }


        /// <summary>
        /// 目标状态
        /// </summary>
        public string TargetStatusAdmin { get { return ((OrderStatusEnum)TargetState).ToAdminString(); } }
    }
}
