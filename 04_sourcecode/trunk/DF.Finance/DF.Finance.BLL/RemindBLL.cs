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
    public class RemindBLL : BaseBLL<Remind>
    {
        #region  任务处理
        /// <summary>
        /// 处理提醒任务
        /// </summary>
        public string RemindTasks()
        {
            //获取任务规则
            var ruleList = new RemindRuleBLL().GetList();
            OrderBLL bll = new OrderBLL();
            var addList = new List<Remind>();
            try
            {
                foreach (var ruleModel in ruleList)
                {
                    if (ruleModel.UnitTyep == 1)
                    {
                        #region 订单相关提醒
                        var list = new List<Order>();
                        //4：保险提醒 特殊处理
                        if (ruleModel.Id != 4)
                        {
                            list = bll.GetOrderAndExtAttributeList(ruleModel);
                        }
                        else
                        {
                            list = bll.GetOrderAndInsuranceList(ruleModel);
                        }
                        if (list.Count > 0)
                        {
                            string orderid = string.Join(",", list.Select(p => p.Id).ToArray());
                            var remindList = GetRemindList(ruleModel, orderid);
                            foreach (var orderModel in list)
                            {
                                var meets = true;
                                //判断是否存在提醒记录
                                var rd = remindList.Where(p => p.ViewId == orderModel.Id).ToList();
                                //进行时间对比
                                if (rd.Count > 0)
                                {
                                    var rdModel = rd.FirstOrDefault();
                                    if ((rdModel.AddTime - DateTime.Now).TotalDays < ruleModel.FirstDay)
                                    {
                                        meets = false;
                                    }
                                }
                                if (meets)
                                {
                                    var day = GetDay(orderModel, ruleModel.RemindType);
                                    addList.Add(new Remind()
                                    {
                                        ViewId = orderModel.Id,
                                        RemindType = ruleModel.RemindType,
                                        UserId = orderModel.UserAgentId,
                                        SourceType = 1,
                                        Reminder = ruleModel.ReplaceText.Replace("{DealersName}", orderModel.DealersName).Replace("{ApplicantName}", orderModel.ApplicantName).Replace("{Day}", day.ToString())
                                    });
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 实际交易方超出期限提醒
                        var list = new BLL.CustomerManagementBLL().GetCustomerManagementList(ruleModel);
                        if (list.Count > 0)
                        {
                            string orderid = string.Join(",", list.Select(p => p.Id).ToArray());
                            var remindList = GetRemindList(ruleModel, orderid);
                            foreach (var cmModel in list)
                            {
                                var meets = true;
                                //判断是否存在提醒记录
                                var rd = remindList.Where(p => p.ViewId == cmModel.Id).ToList();
                                //进行时间对比
                                if (rd.Count > 0)
                                {
                                    var rdModel = rd.FirstOrDefault();
                                    if ((rdModel.AddTime - DateTime.Now).TotalDays < ruleModel.FirstDay)
                                    {
                                        meets = false;
                                    }
                                }
                                if (meets)
                                {
                                    var day = GetDay(null, ruleModel.RemindType, cmModel);
                                    addList.Add(new Remind()
                                    {
                                        ViewId = cmModel.Id,
                                        SourceType = 2,
                                        RemindType = ruleModel.RemindType,
                                        UserId = cmModel.UserAgentId,
                                        Reminder = ruleModel.ReplaceText.Replace("{DealersName}", cmModel.CompanyName).Replace("{Day}", day.ToString())
                                    });
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("执行提醒任务错误", ex);
            }
            //开始执行插入操作
            if (addList != null)
            {
                foreach (var item in addList)
                {
                    base.Add(item);
                }
            }
            return addList.ToJSON();

        }

        /// <summary>
        /// 根据时间返回天数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int GetDay(DateTime time)
        {
            return Convert.ToInt32((DateTime.Now - time).TotalDays);
        }
        /// <summary>
        /// 获取对应的时间
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RemindType"></param>
        /// <returns></returns>
        private int GetDay(Order model, int RemindType, CustomerManagement cmModel = null)
        {
            DateTime time;
            switch (RemindType)
            {
                case 1:
                    time = model.OrderExtAttribute.ApprovalTime;
                    break;
                case 21:
                    time = model.OrderExtAttribute.LoanTime;
                    break;
                case 22:
                    time = model.OrderExtAttribute.RecoveryTime;
                    break;
                case 3:
                    time = model.Insurance.MaturityTime;
                    break;
                case 4:
                    time = cmModel.LastLoanTime;
                    break;
                default:
                    time = DateTime.Now;
                    break;
            }
            var day = GetDay(time);
            //如果是保险需要把负数转正
            if (RemindType == 3)
            {
                day = 0 - day;
            }
            return day;
        }

        public List<Remind> GetRemindList(RemindRule model, string viewId)
        {
            return new RemindDAL().GetRemindList(model, viewId);
        }
        #endregion

        /// <summary>
        /// 获取提醒信息
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<Remind> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("  DeleteFlag=0 ");
            if (!string.IsNullOrEmpty(dic["RemindType"]))
            {
                sbSQL.AppendFormat(" and RemindType={0} ", dic["RemindType"]);
            }

            if (!string.IsNullOrEmpty(dic["starttime"]))
            {
                sbSQL.AppendFormat(" and  AddTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))
            {
                sbSQL.AppendFormat(" and  AddTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbSQL.ToString(), " Id desc ");
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool LogicalDelete(string Ids)
        {
            return dal.Update<Remind>(string.Format(" set  DeleteFlag=1 where Id in({0})", Ids)) > 0;
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="Remind"></param>
        /// <returns></returns>
        public bool LogicalDelete(Remind model)
        {
            return dal.Update<Remind>(string.Format(" SET [DeleteFlag] =1 WHERE ViewId={0} and RemindType={1} ", model.ViewId, model.RemindType)) > 0;
        }
        public void UpdateIHaveRead(string id)
        {
             new DAL.RemindDAL().UpdateIHaveRead(id);
        }
        #region  前端金融业代-提醒消息列表

        /// <summary>
        /// 获取提醒数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="loginUserId">登录用户Id</param>
        /// <returns></returns>
        public Page<Remind> GetRemindList(int pageIndex, int pageSize, int loginUserId)
        {
            string sql = @"select * from dbo.Remind where IsLock=0 and  UserId=" + loginUserId;
            return this.GetPager(pageIndex, pageSize, sql);
        }


        #endregion

    }
}
