using DF.Finance.Common;
using DF.Finance.DAL;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 活动奖品管理
    /// </summary>
    public class ActPrizeBLL : BaseBLL<ActPrize>
    {
        /// <summary>
        /// 获取奖品列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<ActPrize> GetActPrizeList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" select ActPrize.* from ActPrize ");
            sbWhere.Append(" inner join dbo.Activity on ActPrize.ActId=Activity.Id ");
            sbWhere.Append(" where 1=1 ");
            if (!string.IsNullOrEmpty(dic["ActId"]))
            {
                sbWhere.AppendFormat(" and ActId={0} ", dic["ActId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["PrizeName"]))
            {
                sbWhere.AppendFormat(" and PrizeName like '%{0}%' ", dic["PrizeName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["PrizeClass"]))
            {
                sbWhere.AppendFormat(" and PrizeClass={0} ", dic["PrizeClass"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["adduser"]))
            {
                sbWhere.AppendFormat(" and AddUserName like '%{0}%' ", dic["adduser"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["Quantity"]))
            {
                sbWhere.AppendFormat(" and Quantity={0} ", dic["Quantity"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["WQuantity"]))
            {
                sbWhere.AppendFormat(" and WQuantity ={0}", dic["WQuantity"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["NumberOfAwards"]))
            {
                sbWhere.AppendFormat(" and NumberOfAwards ={0}", dic["NumberOfAwards"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_starttime"]))
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["add_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_endtime"]))
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["add_endtime"].Replace("'", ""));
            }
            return this.GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
        /// <summary>
        /// 是否存在该奖品名称
        /// </summary>
        /// <param name="prizeName"></param>
        /// <returns></returns>
        public bool ExistPrizeName(string prizeName)
        {
            return dal.GetList(string.Format(" PrizeName ='{0}'", prizeName), null).SingleOrDefault() != null ? true : false;
        }

        /// <summary>
        /// 是否存在该奖品名称
        /// </summary>
        /// <param name="prizeName"></param>
        /// <returns></returns>
        public bool Exist(string prizeName, int prizeClass)
        {
            return dal.GetList(string.Format(" PrizeName ='{0}' and PrizeClass={1}", prizeName, prizeClass), null).SingleOrDefault() != null ? true : false;
        }

        /// <summary>
        /// 根据奖项获取活动的奖品信息
        /// </summary>
        /// <returns></returns>
        public List<ActPrize> GetActPrizeByClass(int prizeClass)
        {
            return dal.GetList(string.Format(" PrizeClass={0} ", prizeClass), null);
        }

        /// <summary>
        /// 更新已中奖品数量
        /// </summary>
        /// <returns></returns>
        public bool UpdateWQuantity(ActPrize actPrize)
        {
            return dal.Update(actPrize, new List<string>() { 
            "WQuantity"
            }) > 0;
        }

        /// <summary>
        /// 活动中的已中奖品数量清零
        /// </summary>
        /// <param name="Id">奖品Id</param>
        /// <returns></returns>
        public bool WQuantityClearZeroByActId(int actId)
        {
            return new ActPrizeDAL().WQuantityClearZeroByActId(actId);
        }
        /// <summary>
        /// 获取活动的所有奖项的奖品总数
        /// </summary>
        /// <returns></returns>
        public int GetTotalQuantityByActId(int actId)
        {
            return new ActPrizeDAL().GetTotalQuantityByActId(actId);
        }
    }
}
