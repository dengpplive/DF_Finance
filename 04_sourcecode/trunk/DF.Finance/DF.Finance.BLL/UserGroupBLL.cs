using DF.Finance.Common;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class UserGroupBLL : BaseBLL<UserGroup>
    {
        public Page<UserGroup> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool Delete(string ids)
        {
            return dal.Delete(string.Format(" where Id in({0}) and IsSys<>1 ", ids)) > 0;
        }
    }
}
