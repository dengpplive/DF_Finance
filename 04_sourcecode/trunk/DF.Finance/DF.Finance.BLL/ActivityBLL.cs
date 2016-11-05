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
    /// 活动管理
    /// </summary>
    public class ActivityBLL : BaseBLL<Activity>
    {
        ActivityDAL _dal = new ActivityDAL();
        #region 后台
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool UpdateDeleteFlag(string ids)
        {
            return _dal.UpdateDeleteFlag(ids);
        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetList()
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 and DeleteFlag=0 ");
            return _dal.GetList(sbWhere.ToString(), "SortId asc,Id desc");
        }
        /// <summary>
        /// 查询活动列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public Page<Activity> GetActivityList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 and DeleteFlag=0 ");
            if (!string.IsNullOrEmpty(dic["atytitle"]))
            {
                sbWhere.AppendFormat(" and Title like '%{0}%' ", dic["atytitle"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["adduser"]))
            {
                sbWhere.AppendFormat(" and AddUserName like '%{0}%' ", dic["adduser"].Replace("'", ""));
            }
            //if (!string.IsNullOrEmpty(dic["atytype"]))
            //{
            //    sbWhere.AppendFormat(" and ActType ={0} ", dic["atytype"].Replace("'", ""));
            //}
            if (!string.IsNullOrEmpty(dic["IsLock"]))
            {
                sbWhere.AppendFormat(" and IsLock ={0} ", dic["IsLock"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["aty_starttime"]))
            {
                sbWhere.AppendFormat(" and StartTime>='{0}' ", dic["aty_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["aty_endtime"]))
            {
                sbWhere.AppendFormat(" and StartTime<='{0} 23:59:59' ", dic["aty_endtime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_starttime"]))
            {
                sbWhere.AppendFormat(" and  AddTime >='{0}' ", dic["add_starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["add_endtime"]))
            {
                sbWhere.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["add_endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
        /// <summary>
        /// 是否存在该活动名称
        /// </summary>
        /// <param name="activityName">活动名称</param>
        /// <returns></returns>
        public bool ExistActivityName(string activityName)
        {
            return _dal.GetList(string.Format(" Title ='{0}' and DeleteFlag=0", activityName), null).SingleOrDefault() != null ? true : false;
        }
        #endregion

        #region 前台

        /// <summary>
        /// 获取正在进行的活动
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetActivityingList(bool isExpires=true)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" DeleteFlag=0 and IsLock=0  {0}", (isExpires) ? " and (getdate() > StartTime and getdate()< EndTime)" : "");
            return _dal.GetList(sbWhere.ToString(), "SortId asc,Id desc");
        }
        /// <summary>
        /// 根据活动类型获取指定的活动
        /// </summary>
        /// <param name="actType">活动类型Id</param>
        /// <returns></returns>
        public Activity GetActivityByActType(int actType)
        {
            return _dal.GetList("ActType=" + actType, null).SingleOrDefault();
        }

        #endregion


    }
}
