using DF.Finance.Common;
using DF.Finance.DAL;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    ///角色权限
    /// </summary>
    public class SysManagerRoleValueBLL : BaseBLL<SysManagerRoleValue>
    {
        SysManagerRoleValueDAL _dal = new SysManagerRoleValueDAL();
        /// <summary>
        /// 根据条件获取所有权限 大有所属角色 多对一 
        /// </summary>
        /// <param name="where">不带where条件</param>
        /// <returns></returns>
        public List<SysManagerRoleValue> GetSysManagerRoleValueList(string where)
        {
            return _dal.GetSysManagerRoleValueList(where);
        }

        /// <summary>
        /// 角色Id获取权限结合
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public List<SysManagerRoleValue> GetRoleValueByRoleId(int roleId)
        {
            return _dal.GetRoleValueByRoleId(roleId);
        }


    }
}
