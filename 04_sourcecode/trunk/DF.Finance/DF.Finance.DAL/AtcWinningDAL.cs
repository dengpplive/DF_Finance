using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class AtcWinningDAL : BaseDAL<AtcWinning>
    {
        /// <summary>
        /// 获取中奖的用户
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public List<Tels> GetWinListByActId(int actId)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select AtcWinning.UserId,ActPrize.PrizeClass from AtcWinning ");
            sbSQL.Append(" left join ActPrize on AtcWinning.PrizeId=ActPrize.Id ");
            sbSQL.Append(" where AtcWinning.ActId=@0 ");
            return db.Fetch<Tels>(sbSQL.ToString(), actId);
        }
        /// <summary>
        /// 参与该活动的用户是否中奖
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <param name="mobile">用户手机号</param>
        /// <returns></returns>
        public bool IsWinningPrize(int actId, string mobile)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select COUNT(*) from dbo.AtcWinning ");
            sbSQL.Append(" left join Users on dbo.AtcWinning.UserId=Users.Id ");
            sbSQL.Append(" where dbo.AtcWinning.ActId=@0 and Users.Mobile=@1 ");
            return db.ExecuteScalar<int>(sbSQL.ToString(), actId, mobile) > 0;

        }


    }
}
