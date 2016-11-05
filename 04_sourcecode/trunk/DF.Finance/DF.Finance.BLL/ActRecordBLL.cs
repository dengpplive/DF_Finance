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
    /// 参与人活动记录
    /// </summary>
    public class ActRecordBLL : BaseBLL<ActRecord>
    {
        ActRecordDAL _dal = new ActRecordDAL();
        /// <summary>
        /// 获取参与活动纪录列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<ActRecord> GetActRecordList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" select ActRecord.*,Users.UserName from ActRecord  ");
            sbSql.Append("inner join Users on dbo.ActRecord.UserId=Users.Id ");
            sbSql.Append(" where  1=1 ");

            if (!string.IsNullOrEmpty(dic["ActId"]))
            {
                sbSql.AppendFormat(" and ActId ={0} ", dic["ActId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["UserName"]))
            {
                sbSql.AppendFormat(" and Users.UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["UserIP"]))
            {
                sbSql.AppendFormat(" and UserIP like '%{0}%' ", dic["UserIP"].Replace("'", ""));
            }

            if (!string.IsNullOrEmpty(dic["add_starttime"]))
            {
                sbSql.AppendFormat(" and  AddTime >='{0}' ", dic["add_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_endtime"]))
            {
                sbSql.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["add_endtime"].Replace("'", ""));
            }
            return this.GetPageBySQL(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSql.ToString(), order);
        }

        /// <summary>
        /// 批量添加中奖纪录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool PatchAdd(List<ActRecord> list)
        {
            List<string> sqlList = new List<string>();
            list.ForEach(p =>
            {
                sqlList.Add(string.Format("INSERT INTO [ActRecord]([UserId],[ActName],[ActId],[AddTime],[UserIP]) VALUES ({0} ,'{1}',{2} ,'{3}','{4}')",
                    p.UserId,
                    p.ActName,
                    p.ActId,
                    p.AddTime,
                    p.UserIP
                    ));
            });
            int count = _dal.DoTrans(sqlList);
            return list.Count == count;
        }
        /// <summary>
        /// 清空活动记录
        /// </summary>
        /// <param name="ActId">活动编号</param>
        /// <returns></returns>
        public bool DeleteByActType(int actId)
        {
            return _dal.Delete("delete from ActRecord where ActId=@0", actId) > 0;
        }
        /// <summary>
        /// 获取活动的参与人员列表
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public List<Tels> GetListByActId(int actId)
        {
            return _dal.GetListByActId(actId);
        }

    }
}
