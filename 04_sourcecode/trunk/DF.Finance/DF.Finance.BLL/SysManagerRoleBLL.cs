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
    /// 角色管理
    /// </summary>
    public class SysManagerRoleBLL : BaseBLL<SysManagerRole>
    {
        public Page<SysManagerRole> GetRoleList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            //if (!string.IsNullOrEmpty(dic["rolename"]))
            //{
            //    sbWhere.AppendFormat(" and RoleName like '%{0}%' ", dic["rolename"].Replace("'", ""));
            //}
            //if (!string.IsNullOrEmpty(dic["roletype"]))
            //{
            //    sbWhere.AppendFormat(" and RoleType={0} ", dic["roletype"].Replace("'", ""));
            //}
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
        public bool ExistRoleName(string roleName)
        {
            return dal.GetList(string.Format(" RoleName ='{0}'", roleName), null).SingleOrDefault() != null ? true : false;
        }
        /// <summary>
        /// 添加权限数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteAdd(SysManagerRole model)
        {

            return new SysManagerRoleDAL().CompleteAdd(model);
        }
        /// <summary>
        /// 添加权限数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteUpdate(SysManagerRole model)
        {
            return new SysManagerRoleDAL().CompleteUpdate(model);
        }

        /// <summary>
        /// 获得权限
        /// </summary>
        //public SysManagerRole GetSysManagerRole(int roleId)
        //{
        //    SysManagerRole model = new SysManagerRoleDAL().GetModel(roleId);
        //    model.RoleValueList = new SysManagerRoleValueBLL().GetList(string.Format(" RoleId={0}", model.Id));
        //    //if (model != null )
        //    //{
        //    //    if (model.RoleType == 1)
        //    //    {
        //    //        return true;
        //    //    }
        //    //    if (model.RoleValueList == null)
        //    //    {
        //    //        return false;
        //    //    }
        //    //    SysManagerRoleValue modelt = model.RoleValueList.Find(p => p.NavName == navName && p.ActionType == actionType);
        //    //    if (modelt != null)
        //    //    {
        //    //        return true;
        //    //    }
        //    //}
        //    //return false;
        //    return model;
        //}
    }
}
