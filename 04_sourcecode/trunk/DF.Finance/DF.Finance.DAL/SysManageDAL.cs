using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class SysManageDAL : BaseDAL<SysManage>
    {
        /// <summary>
        /// 根据条件获取管理员 带有角色 
        /// </summary>
        /// <param name="where">不带where的条件</param>
        /// <returns></returns>
        public List<SysManage> GetSysManageList(string where)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select SysManage.*,SysManagerRole.* from SysManage ");
            sbSQL.Append(" left join SysManagerRole on SysManage.RoleId=SysManagerRole.Id  ");
            if (!string.IsNullOrEmpty(where))
                sbSQL.AppendFormat(" where {0}", where);
            return db.Fetch<SysManage, SysManagerRole>(sbSQL.ToString());
        }
    }
}
