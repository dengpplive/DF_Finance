using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class UserLoginLogBLL : BaseBLL<UserLoginLog>
    {
        public Page<UserLoginLog> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(dic["UserName"]))
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["S_Time"]))
            {
                sbWhere.AppendFormat(" and  LoginTime >='{0}' ", dic["S_Time"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["E_Time"]))
            {
                sbWhere.AppendFormat(" and  LoginTime <='{0} 23:59:59' ", dic["E_Time"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }
    }
}
