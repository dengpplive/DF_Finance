using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.Common;
using DF.Finance.DAL;
namespace DF.Finance.BLL
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    public class SysManageBLL : BaseBLL<SysManage>
    {
        SysManageDAL _dal = new SysManageDAL();
        /// <summary>
        /// 后台管理员登录
        /// </summary>
        /// <param name="username">登录账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public RetObj UserLogin(string username, string pwd, bool pwdRequire = true)
        {
            var list = _dal.GetSysManageList(string.Format(" UserName='{0}'", username));
            var user = (list != null && list.Count > 0) ? list.FirstOrDefault() : null;
            if (user == null)
            {

                return new RetObj().Error("用户名不存在");
            }
            if (user.IsLock == 1)
            {
                return new RetObj().Error("用户已锁定");
            }
            if (pwdRequire)
            {
                string oldPwd = Encrypt.Encode(pwd, user.Salt);
                if (oldPwd != user.PassWord)
                {

                    return new RetObj().Error("用户密码错误");
                }
            }
            if (user.Role != null && user.Role.Id > 0 && user.RoleType > 1)
            {
                //获取该用户的权限集合
                SysManagerRoleValueBLL sysManagerRoleValueBLL = new SysManagerRoleValueBLL();
                user.Role.RoleValueList = sysManagerRoleValueBLL.GetRoleValueByRoleId(user.Role.Id);
            }
            return new RetObj(user);
        }
        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<SysManage> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(dic["username"]))
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["username"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["realname"]))
            {
                sbWhere.AppendFormat(" and RealName like '%{0}%' ", dic["realname"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["telphone"]))
            {
                sbWhere.AppendFormat(" and TelePhone like '%{0}%' ", dic["telphone"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["email"]))
            {
                sbWhere.AppendFormat(" and Email like '%{0}%' ", dic["email"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["state"]))
            {
                sbWhere.AppendFormat(" and IsLock={0} ", dic["state"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["starttime"]))
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }

            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
        /// <summary>
        /// 是否存在该用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public bool ExistUser(string userName)
        {
            return _dal.GetList(string.Format(" UserName ='{0}'", userName), null).SingleOrDefault() != null ? true : false;
        }

    }
}
