using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class ActPrizeDAL : BaseDAL<ActPrize>
    {
        /// <summary>
        /// 活动中的中奖数量清零
        /// </summary>
        /// <param name="ActId">活动Id</param>
        /// <returns></returns>
        public bool WQuantityClearZeroByActId(int actId)
        {
            return db.Update<ActPrize>(" set WQuantity=0 where ActId=@0", actId) > 0;
        }
        /// <summary>
        /// 获取该活动设置的奖品总数
        /// </summary>
        /// <param name="ActId">活动Id</param>
        /// <returns></returns>
        public int GetTotalQuantityByActId(int actId)
        {
            return db.ExecuteScalar<int>("select SUM(Quantity) Quantity from ActPrize where ActId=@0", actId);
        }
    }
}
