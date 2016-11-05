using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class SysManagerLogBLL : BaseBLL<SysManagerLog>
    {
        public Page<SysManagerLog> GetSysLogList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(dic["UserName"]))
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["UserIP"]))
            {
                sbWhere.AppendFormat(" and UserIP like '%{0}%' ", dic["UserIP"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["UserId"]))
            {
                sbWhere.AppendFormat(" and UserId={0} ", dic["UserId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["ActionType"]))
            {
                sbWhere.AppendFormat(" and ActionType={0} ", dic["ActionType"]);
            }
            if (!string.IsNullOrEmpty(dic["starttime"]))
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }
    }
}
