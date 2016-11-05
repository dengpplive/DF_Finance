using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class ActRecordDAL : BaseDAL<ActRecord>
    {
        /// <summary>
        /// 获取活动的参与人员列表
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public List<Tels> GetListByActId(int actId)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select Users.Id UserId,Users.UserName,Users.Mobile from ActRecord ");
            sbSQL.Append(" left join Users on  Users.Id= ActRecord.UserId ");
            sbSQL.Append(" where ActRecord.ActId=@0 ");
            return db.Fetch<Tels>(sbSQL.ToString(), actId);
        }
    }
}
