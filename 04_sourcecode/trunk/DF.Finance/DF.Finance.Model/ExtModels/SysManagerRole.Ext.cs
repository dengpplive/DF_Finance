using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class SysManagerRole
    {
        /// <summary>
        /// 权限集合
        /// </summary>
        public List<SysManagerRoleValue> RoleValueList { get; set; }
        
        /// <summary>
        /// 权限类型集合
        /// </summary>
        public List<string> ActionTypeList { get; set; }
    }
}
