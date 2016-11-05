using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class SysManagerRoleValueDAL : BaseDAL<SysManagerRoleValue>
    {
        /// <summary>
        /// 根据条件获取所有权限 大有所属角色 多对一 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<SysManagerRoleValue> GetSysManagerRoleValueList(string where)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select SysManagerRoleValue.*,SysManagerRole.* from SysManagerRoleValue ");
            sbSQL.Append(" left join SysManagerRole on SysManagerRoleValue.RoleId=SysManagerRole.Id  ");
            if (!string.IsNullOrEmpty(where))
                sbSQL.AppendFormat(" where {0}", where);
            return db.Fetch<SysManagerRoleValue, SysManagerRole>(sbSQL.ToString());
        }
        /// <summary>
        /// 角色Id获取权限结合
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public List<SysManagerRoleValue> GetRoleValueByRoleId(int roleId)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select SysManagerRoleValue.* from SysManagerRoleValue ");
            sbSQL.AppendFormat(" where RoleId= {0}", roleId);
            return db.Fetch<SysManagerRoleValue>(sbSQL.ToString());
        }

    }
}
