using DF.Finance.Common;
using DF.Finance.DAL;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 获奖记录管理
    /// </summary>
    public class AtcWinningBLL : BaseBLL<AtcWinning>
    {
        AtcWinningDAL _dal = new AtcWinningDAL();
        /// <summary>
        /// 获取中奖纪录列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序列表</param>
        /// <returns></returns>
        public Page<AtcWinning> GetWinningList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("");
            sbSql.Append(" select dbo.AtcWinning.*,Users.UserName,ActPrize.PrizeClass,Activity.Title ");
            sbSql.Append(" from  dbo.AtcWinning  ");
            sbSql.Append(" inner join Users on dbo.AtcWinning.UserId=Users.Id ");
            sbSql.Append(" inner join Activity on dbo.AtcWinning.ActId=Activity.Id ");
            sbSql.Append(" inner join ActPrize on dbo.AtcWinning.PrizeId=ActPrize.Id ");
            sbSql.Append(" where 1=1 ");

            if (!string.IsNullOrEmpty(dic["username"]))
            {
                sbSql.AppendFormat(" and Users.UserName like '%{0}%' ", dic["username"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["ActId"]))
            {
                sbSql.AppendFormat(" and AtcWinning.ActId ={0} ", dic["ActId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["PrizeClass"]))
            {
                sbSql.AppendFormat(" and ActPrize.PrizeClass ={0} ", dic["PrizeClass"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_starttime"]))
            {
                sbSql.AppendFormat(" and AtcWinning.AwardTime>='{0}' ", dic["add_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_endtime"]))
            {
                sbSql.AppendFormat(" and AtcWinning.AwardTime<='{0} 23:59:59' ", dic["add_endtime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["phone"]))
            {
                sbSql.AppendFormat(" and AwardPhone like '%{0}%'  ", dic["phone"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["prizename"]))
            {
                sbSql.AppendFormat(" and ActPrize.PrizeName like '%{0}%'  ", dic["prizename"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["IsAward"]))
            {
                sbSql.AppendFormat(" and AtcWinning.IsAward={0}  ", dic["IsAward"].Replace("'", ""));
            }
            return _dal.GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSql.ToString(), order);
        }



        /// <summary>
        /// 添加中奖纪录
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public bool PatchAdd(List<AtcWinning> list)
        {
            List<string> sqlList = new List<string>();
            list.ForEach(p =>
            {
                sqlList.Add(string.Format("INSERT INTO [AtcWinning]([UserId],[ActId],[PrizeId],[PrizeName],[IsAward],[AwardPhone]) VALUES ({0} ,{1},{2} ,'{3}',{4},'{5}')",
                    p.UserId,
                    p.ActId,
                    p.PrizeId,
                    p.PrizeName,
                    p.IsAward,
                    p.AwardPhone
                    ));
            });
            int count = _dal.DoTrans(sqlList);
            return list.Count == count;
        }


        /// <summary>
        /// 清空中奖活动记录
        /// </summary>
        /// <param name="ActId">活动编号</param>
        /// <returns></returns>
        public bool DeleteByActType(int ActId)
        {
            return _dal.Delete("delete from AtcWinning where ActId=@0", ActId) > 0;
        }
        /// <summary>
        /// 获取中奖的用户
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public List<Tels> GetWinListByActId(int actId)
        {
            return _dal.GetWinListByActId(actId);
        }

        #region 手机端
        /// <summary>
        /// 用户的手机号查询是否中奖 true中奖 false未中奖
        /// </summary>
        /// <param name="mobile">用户手机号</param>
        /// <returns></returns>
        public bool UserIsWinningPrize(string mobile)
        {
            var activity = PrizeManageBLL.GetActivity("抽奖",false);
            return _dal.IsWinningPrize(activity.Id, mobile);
        }
        /// <summary>
        /// 手机号查询中奖信息
        /// </summary>
        /// <param name="mobile">用户手机号</param>
        /// <returns></returns>
        public PrizeView UserWinningPrizeInfo(string mobile)
        {
            var result = new PrizeView();
            result.activity = PrizeManageBLL.GetActivity("抽奖", false);
            var isWinning = _dal.IsWinningPrize(result.activity.Id, mobile);
            if (isWinning)
                result = PrizeManageBLL.GetPrizeResult(result.activity, mobile);
            return result;
        }

        #endregion
    }
}
