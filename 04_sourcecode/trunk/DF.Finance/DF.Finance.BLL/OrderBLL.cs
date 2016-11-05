using DF.Finance.Common;
using DF.Finance.Common.Assert;
using DF.Finance.Model;
using DF.Finance.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderBLL : BaseBLL<Order>
    {
        /// <summary>
        /// 根据查询条件获取订单列表
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Page<Order> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" DeleteFlag=0 ");
            if (!string.IsNullOrEmpty(dic["AddName"]))//金融业代
            {
                sbWhere.AppendFormat(" and AddName like '%{0}%' ", dic["AddName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["ApplicantName"]))//申请人姓名
            {
                sbWhere.AppendFormat(" and ApplicantName like '%{0}%' ", dic["ApplicantName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["ApplicantPhone"]))//申请人电话
            {
                sbWhere.AppendFormat(" and ApplicantPhone like '%{0}%' ", dic["ApplicantPhone"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["LoanProducts"]))//贷款产品
            {
                sbWhere.AppendFormat(" and LoanProducts like '%{0}%' ", dic["LoanProducts"].Replace("'", ""));
            }
            if (int.Parse(dic["DealersId"]) != 0)//实际交易方
            {
                sbWhere.AppendFormat(" and DealersId ='{0}' ", dic["DealersId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["CarBrand"]))//汽车品牌
            {
                sbWhere.AppendFormat(" and CarBrand like '%{0}%' ", dic["CarBrand"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["Models"]))//汽车车型
            {
                sbWhere.AppendFormat(" and Models like '%{0}%' ", dic["Models"].Replace("'", ""));
            }
            if (int.Parse(dic["status2"]) != 0)//节点状态
            {
                sbWhere.AppendFormat(" and Status ='{0}' ", dic["status2"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["SelRegion"])) //区域
            {
                sbWhere.AppendFormat(" and DepartmentName like '%{0}%' ", dic["SelRegion"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["SelProvince"])) //省份
            {
                sbWhere.AppendFormat(" and DepartmentName like '%{0}%' ", dic["SelRegion"].Replace("'", "") + "," + dic["SelProvince"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["SelCity"])) //城市
            {
                sbWhere.AppendFormat(" and DepartmentName like '%{0}%' ", dic["SelRegion"].Replace("'", "") + "," + dic["SelProvince"].Replace("'", "") + dic["SelCity"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["SelQu"])) //分组
            {
                sbWhere.AppendFormat(" and DepartmentName like '%{0}%' ", dic["SelRegion"].Replace("'", "") + "," + dic["SelProvince"].Replace("'", "") + dic["SelQu"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["starttime"]))//收单时间 区间1
            {
                sbWhere.AppendFormat(" and  AcquirerTime >='{0}' ", dic["starttime"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["endtime"]))//收单时间 区间2
            {
                sbWhere.AppendFormat(" and  AcquirerTime <='{0} 23:59:59' ", dic["endtime"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }

        /// <summary>
        /// 更新订单备注和状态
        /// </summary>
        /// <param name="OrderId">订单Id</param>
        /// <param name="Status">订单状态值</param>
        /// <param name="Remark">订单备注</param>
        /// <returns></returns>
        public int UpdateRemark(int OrderId, int Status, string Remark)
        {
            return dal.Update<Order>(string.Format(" set Remark=@0,Status=@1 where Id=@2"), Remark, Status, OrderId);
        }


        /// <summary>
        /// 根据Id获取订单的所有信息
        /// </summary>
        /// <returns></returns>
        public Order GetOrder(int orderId)
        {
            var model = dal.GetModel(orderId);
            model.OrderExtAttribute = new OrderExtAttributeBLL().GetByOrderId(orderId);//扩展属性
            model.Insurance = new OrderInsuranceBLL().GetByOrderId(orderId);//保险信息
            //model.OrderRecords = new OrderRecordBLL().GetByOrderId(orderId);//操作日志
            model.OrderRemindRemarks = new OrderRemindRemarkBLL().GetByOrderId(orderId);//订单提醒备注
            return model;
        }

        /// <summary>
        /// 检测订单是否存在
        /// </summary>
        /// <param name="orderId"></param>
        public void Exist(int orderId)
        {
            if (orderId == 0)
            {
                throw new AdminMessageException("参数错误，非法操作！");
            }
        }

        /// <summary>
        /// 验证提交的订单信息是否完整
        /// </summary>
        /// <param name="order"></param>
        /// <param name="targetStatus">订单要修改的目标状态</param>
        /// <returns></returns>
        public string ValidateData(Order order, OrderStatusEnum targetStatus)
        {
            switch (targetStatus)
            {
                case OrderStatusEnum.资料审核不通过:
                case OrderStatusEnum.总部审核不通过:
                    if (string.IsNullOrEmpty(order.Remark))
                    {
                        return "未填写备注原因";
                    }
                    break;
                case OrderStatusEnum.待确定合同:
                    if (order.OrderExtAttribute.ApprovalAmount == 0 || order.OrderExtAttribute.ApprovalAmount == null)
                    {
                        return "未填写审批金额";
                    }
                    if (order.OrderExtAttribute.ApprovalTime.ToString() == "1900/1/1 0:00:00")
                    {
                        return "未填写审批通过日期";
                    }
                    break;
                case OrderStatusEnum.待放款:
                    if (string.IsNullOrEmpty(order.OrderExtAttribute.ContractNumber))
                    {
                        return "未填写贷款合同号码";
                    }
                    if (order.OrderExtAttribute.ContractTime.ToString() == "1900/1/1 0:00:00")
                    {
                        return "未填写合同签订时间";
                    }
                    break;
                case OrderStatusEnum.待登记:
                    if (string.IsNullOrEmpty(order.OrderExtAttribute.BillingUnit))
                    {
                        return "未填写开票单位";
                    }
                    if (order.OrderExtAttribute.LoanTime.ToString() == "1900/1/1 0:00:00")
                    {
                        return "未填写放款日期";
                    }
                    break;
                case OrderStatusEnum.待资料回邮:
                    if (order.OrderExtAttribute.CheckInStatus == 0)
                    {
                        return "未选择登记证状态";
                    }
                    if (order.OrderExtAttribute.CheckInTime.ToString() == "1900/1/1 0:00:00")
                    {
                        return "未填写从车行回收登记证日期";
                    }
                    break;
                case OrderStatusEnum.已完成:
                    if (order.OrderExtAttribute.MailingTime.ToString() == "1900/1/1 0:00:00")
                    {
                        return "未填写资料邮寄时间";
                    }
                    break;
            }
            return "success";
        }

        /// <summary>
        /// 根据提醒规则 拿符合条件的 订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public List<Order> GetOrderAndExtAttributeList(RemindRule model)
        {
            return new DAL.OrderDAL().GetOrderAndExtAttributeList(model);
        }

        /// <summary>
        /// 根据提醒规则 拿符合条件的 订单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public List<Order> GetOrderAndInsuranceList(RemindRule model)
        {
            return new DAL.OrderDAL().GetOrderAndInsuranceList(model);
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public bool LogicalDelete(string Ids)
        {
            return dal.Update<Order>(string.Format(" set  DeleteFlag=1 where Id in({0})", Ids)) > 0;
        }

        #region 前端
        /// <summary>
        /// 录入资料
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="userId">录入资料的用户Id</param>
        /// <returns></returns>
        public RetObj InputData(Order order, int userId, string ip)
        {

            //查验当前的用户是否登录
            var userBLL = new UserBLL();
            var u = userBLL.GetModel(userId);
            order.UserAgentId = userId;
            AssertUtil.IsNotNull<User>(u, "当前用户未登录,请登录后操作");

            //验证数据
            AssertUtil.IsNotNull<Order>(order, "录入资料信息不能为空！");
            //订单号
            order.OrderNo = AssistantHelper.GetOrderId(4);
            order.AddTime = System.DateTime.Now;
            order.AddName = u.UserName;

            //添加数据库
            var id = this.Add(order);
            //记录日志 首先保证订单能插入成功，所有不使用事务记录日志
            OrderRecord or = new OrderRecord();
            or.UserId = u.Id;
            or.UserName = u.UserName;
            or.OriginalState = 1;
            or.TargetState = 1;
            or.Ip = ip;
            or.RealName = u.UserName;
            or.Remark = "新增订单";
            or.OrderId = int.Parse(id.ToString());
            new BLL.OrderRecordBLL().Add(or);

            return new RetObj(order);
        }
        #endregion
    }
}
