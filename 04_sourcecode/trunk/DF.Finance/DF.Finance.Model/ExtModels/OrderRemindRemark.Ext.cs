using DF.Finance.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class OrderRemindRemark
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string RemindTypeName { get { return ((RemindTypeEnums)RemindType).ToString(); } }
    }
}
