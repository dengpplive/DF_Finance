using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class SysManagerRoleDAL : BaseDAL<SysManagerRole>
    {
        /// <summary>
        /// 根据条件获取角色列表 带有权限 1对多
        /// </summary>
        /// <param name="where">不带where的条件</param>
        /// <returns></returns>
        public List<SysManagerRole> GetRoleList(string where)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select SysManagerRole.*,SysManagerRoleValue.* from SysManagerRole ");
            sbSQL.Append(" left join SysManagerRoleValue on SysManagerRoleValue.RoleId=SysManagerRole.Id ");
            if (!string.IsNullOrEmpty(where))
                sbSQL.AppendFormat(" where {0}", where);
            return db.Fetch<SysManagerRole, SysManagerRoleValue>(sbSQL.ToString());
        }
        

        /// <summary>
        /// 添加权限数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteAdd(SysManagerRole model)
        {
            try
            {
                using (var scope = db.GetTransaction())
                {
                    object id = base.Add(model);//1
                    if (id != null)
                    {
                        SysManagerRoleValueDAL rvDLL = new SysManagerRoleValueDAL();
                        foreach (var item in model.RoleValueList)
                        {
                            item.RoleId = int.Parse(id.ToString());
                            rvDLL.Add(item);//2
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }
        /// <summary>
        /// 添加权限数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteUpdate(SysManagerRole model)
        {
            try
            {
                using (var scope = db.GetTransaction())
                {
                    base.Update(model);
                    SysManagerRoleValueDAL rvDLL = new SysManagerRoleValueDAL();
                    rvDLL.Delete(string.Format(" where RoleId={0} ", model.Id));
                    foreach (var item in model.RoleValueList)
                    {
                        item.RoleId = model.Id;
                        rvDLL.Add(item);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }
    }
}
