using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Enums
{
    /// <summary>
    /// 提醒类型枚举
    /// 2016-05-26 jim
    /// </summary>
    public enum RemindTypeEnums
    {
        审批通过未放款提醒 = 1,
        放款后未收回登记证 = 21,
        放款后未邮寄登记证 = 22,
        保险快到期提醒 = 3,
        放款为0的实际交易方 =4,
        放款成功前端提醒  = 51,
        放款成功邮件提醒 = 55
        
    }

   
}
