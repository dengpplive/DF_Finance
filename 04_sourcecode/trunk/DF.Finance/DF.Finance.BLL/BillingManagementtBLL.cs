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
    /// 开票方管理
    /// </summary>
    public class BillingManagementtBLL : BaseBLL<BillingManagementt>
    {
        /// <summary>
        /// 获取开票方公司信息列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<BillingManagementt> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("  DeleteFlag=0 ");
            if (!string.IsNullOrEmpty(dic["companyname"]))
            {
                sbSQL.AppendFormat(" and CompanyName like '%{0}%' ", dic["companyname"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["addname"]))
            {
                sbSQL.AppendFormat(" and AddName like '%{0}%' ", dic["addname"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["starttime"]))
            {
                sbSQL.AppendFormat(" and  AddTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))
            {
                sbSQL.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), order);
        }

        /// <summary>
        /// 获取开票方的公司列表
        /// </summary>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public List<BillingManagementt> GetListByCompanyName(string companyName)
        {
            return this.GetList(string.Format(" CompanyName like '%{0}%' ", companyName));
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool UpdateDeleteFlag(string ids)
        {
            return dal.Update<BillingManagementt>(string.Format(" set  DeleteFlag=1 where Id in({0})", ids)) > 0;
        }
        /// <summary>
        /// 是否存在该开票方公司名称
        /// </summary>
        /// <param name="userName">公司名称</param>
        /// <returns></returns>
        public bool ExistCompany(string companyName)
        {
            return dal.GetList(string.Format(" CompanyName ='{0}' and DeleteFlag=0 ", companyName), null).SingleOrDefault() != null ? true : false;
        }
    }
}
