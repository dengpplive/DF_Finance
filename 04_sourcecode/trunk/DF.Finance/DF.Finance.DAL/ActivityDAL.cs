using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class ActivityDAL : BaseDAL<Activity>
    {
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool UpdateDeleteFlag(string ids)
        {
            return Update<Activity>(string.Format(" set  DeleteFlag=1 where Id in({0})", ids)) > 0;
        }
    }
}
