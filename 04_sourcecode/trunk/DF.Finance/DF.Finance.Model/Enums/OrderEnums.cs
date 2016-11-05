using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Enums
{
    /// <summary>
    /// 订单状态枚举
    /// 2016-05-23 jim
    /// </summary>
    public enum OrderStatusEnum
    {
        已录入 = 1,
        资料审核不通过 = 91,
        待总部审核 = 2,
        总部审核不通过 = 92,
        待确定合同 = 3,
        待放款 = 4,
        待登记 = 5,
        待资料回邮 = 6,
        已完成 = 99
    }
    /// <summary>
    /// 订单状态操作
    /// </summary>
    public enum OrderActionEnum
    {
        /// <summary>
        /// 资料不完整
        /// </summary>
        资料不完整,
        /// <summary>
        /// 资料完整
        /// </summary>
        资料完整,
        /// <summary>
        /// 审批不通过
        /// </summary>
        审批不通过,
        /// <summary>
        /// 审批通过
        /// </summary>
        审批通过,
        /// <summary>
        /// 录入合同资料
        /// </summary>
        录入合同资料,
        /// <summary>
        /// 放款资料录入
        /// </summary>
        放款资料录入,
        /// <summary>
        /// 登记证操作
        /// </summary>
        登记证操作,
        /// <summary>
        /// 资料回邮
        /// </summary>
        资料回邮,
        /// <summary>
        /// 保险信息录入
        /// </summary>
        保险信息录入,
        /// <summary>
        /// 订单提醒备注
        /// </summary>
        订单提醒备注
    }
    /// <summary>
    /// 订单枚举状态扩展方法
    /// </summary>
    public static class OrderStatusEnumExtension
    {
        /// <summary>
        /// 审核状态 3种 
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static string ToClientAuditString(this OrderStatusEnum status)
        {
            // TODO: 客户端贷款详情显示状态 djy
            switch (status)
            {
                case OrderStatusEnum.已录入:
                case OrderStatusEnum.待总部审核:
                case OrderStatusEnum.待确定合同:
                case OrderStatusEnum.待放款:
                    return "等待审核";
                case OrderStatusEnum.待登记:
                case OrderStatusEnum.待资料回邮:
                case OrderStatusEnum.已完成:
                    return "审核通过";
                case OrderStatusEnum.资料审核不通过:
                case OrderStatusEnum.总部审核不通过:
                    return "审核不通过";
            }
            return "";
        }


        /// <summary>
        /// 转换成客户端显示订单状态名称
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static string ToClientString(this OrderStatusEnum status)
        {
            // TODO: 客户端显示订单状态名称 jim 2016-05-23
            switch (status)
            {
                case OrderStatusEnum.待确定合同:
                    return "总部审核通过 待确定合同";
                case OrderStatusEnum.待放款:
                    return "合同确定 待放款";
                case OrderStatusEnum.待登记:
                    return "放款成功 待登记";
                case OrderStatusEnum.待资料回邮:
                    return "登记成功 待回邮资料";
                case OrderStatusEnum.已完成:
                case OrderStatusEnum.已录入:
                case OrderStatusEnum.资料审核不通过:
                case OrderStatusEnum.待总部审核:
                case OrderStatusEnum.总部审核不通过:
                    return status.ToString();
            }
            return "";
        }
        /// <summary>
        /// 转换成后台管理端显示订单状态名称
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static string ToAdminString(this OrderStatusEnum status)
        {
            // TODO: 后台管理端显示订单状态名称 jim 2016-05-23
            switch (status)
            {
                case OrderStatusEnum.已录入:
                case OrderStatusEnum.资料审核不通过:
                case OrderStatusEnum.待总部审核:
                case OrderStatusEnum.总部审核不通过:
                case OrderStatusEnum.待确定合同:
                case OrderStatusEnum.待放款:
                case OrderStatusEnum.待登记:
                case OrderStatusEnum.待资料回邮:
                case OrderStatusEnum.已完成:
                    return status.ToString();
            }
            return status.ToString();
        }

        public static string ToNextAdminString(this OrderStatusEnum status)
        {
            // TODO: 后台管理端显示订单状态名称 jim 2016-05-23
            switch (status)
            {
                case OrderStatusEnum.已录入:
                    return OrderActionEnum.资料完整.ToString();
                case OrderStatusEnum.待总部审核:
                    return OrderActionEnum.审批通过.ToString();
                case OrderStatusEnum.待确定合同:
                    return OrderActionEnum.录入合同资料.ToString();
                case OrderStatusEnum.待放款:
                    return OrderActionEnum.放款资料录入.ToString();
                case OrderStatusEnum.待登记:
                    return OrderActionEnum.登记证操作.ToString();
                case OrderStatusEnum.待资料回邮:
                    return OrderActionEnum.资料回邮.ToString();

            }
            return status.ToString();
        }

        /// <summary>
        /// 往下一步
        /// </summary>
        private static Dictionary<OrderStatusEnum, Dictionary<OrderActionEnum, OrderStatusEnum>> allowStatus = new Dictionary<OrderStatusEnum, Dictionary<OrderActionEnum, OrderStatusEnum>>
        {
            { OrderStatusEnum.已录入, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.资料不完整, OrderStatusEnum.资料审核不通过 },
                { OrderActionEnum.资料完整, OrderStatusEnum.待总部审核 }
            }
            },
            { OrderStatusEnum.待总部审核, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.审批不通过, OrderStatusEnum.总部审核不通过 },
                { OrderActionEnum.审批通过, OrderStatusEnum.待确定合同 }
            }
            },
            { OrderStatusEnum.待确定合同, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.录入合同资料, OrderStatusEnum.待放款 }
            }
            },
            { OrderStatusEnum.待放款, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.放款资料录入, OrderStatusEnum.待登记 }
            }
            },
            { OrderStatusEnum.待登记, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.登记证操作, OrderStatusEnum.待资料回邮 },
                { OrderActionEnum.保险信息录入, OrderStatusEnum.待资料回邮 }
            }
            },
            { OrderStatusEnum.待资料回邮, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.保险信息录入, OrderStatusEnum.待资料回邮 },
                { OrderActionEnum.资料回邮, OrderStatusEnum.已完成 }
            }
            },
            { OrderStatusEnum.已完成, new Dictionary<OrderActionEnum, OrderStatusEnum> {
                { OrderActionEnum.订单提醒备注, OrderStatusEnum.已完成 }
            }
            }
        };

        /// <summary>
        /// 检测源状态是否允许转换到目标状态（当前状态到下一个状态）
        /// </summary>
        /// <param name="sourceStatus">源状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns></returns>
        public static bool CheckToStatus(this OrderStatusEnum sourceStatus, OrderStatusEnum targetStatus)
        {
            //Enum<OrderStatusEnum>.AsEnumerable().ToDictionary(r => r.ToAdminString(), y=>(int)y);
            try
            {
                return allowStatus[sourceStatus].ContainsValue(targetStatus);
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 获取下一个扭转的状态值
        /// </summary>
        /// <param name="sourceStatus"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static OrderStatusEnum GetNextStatus(this OrderStatusEnum sourceStatus, OrderActionEnum action)
        {
            try
            {
                return allowStatus[sourceStatus][action];
            }
            catch
            {
                throw new NotSupportedException("订单的" + sourceStatus.ToClientString() + "状态不允许执行" + action.ToString() + "操作");
            }
        }

        /// <summary>
        /// 获取可扭转到上一步的订单状态的枚举数组
        /// </summary>
        /// <param name="status">当前订单状态枚举</param>
        /// <returns></returns>
        public static OrderStatusEnum[] ToPreviousStatusEnum(this OrderStatusEnum status)
        {
            List<OrderStatusEnum> list = new List<OrderStatusEnum>();
            switch (status)
            {
                case OrderStatusEnum.已录入:
                    list.Add(OrderStatusEnum.资料审核不通过);
                    break;
                case OrderStatusEnum.资料审核不通过:
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.总部审核不通过:
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.待总部审核:
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.待确定合同:
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.待放款:
                    list.Add(OrderStatusEnum.待确定合同);
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.待登记:
                    list.Add(OrderStatusEnum.待放款);
                    list.Add(OrderStatusEnum.待确定合同);
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.待资料回邮:
                    list.Add(OrderStatusEnum.待登记);
                    list.Add(OrderStatusEnum.待放款);
                    list.Add(OrderStatusEnum.待确定合同);
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;
                case OrderStatusEnum.已完成:
                    list.Add(OrderStatusEnum.待资料回邮);
                    list.Add(OrderStatusEnum.待登记);
                    list.Add(OrderStatusEnum.待放款);
                    list.Add(OrderStatusEnum.待确定合同);
                    list.Add(OrderStatusEnum.待总部审核);
                    list.Add(OrderStatusEnum.总部审核不通过);
                    list.Add(OrderStatusEnum.资料审核不通过);
                    list.Add(OrderStatusEnum.已录入);
                    break;

            }
            return list.ToArray();
        }

        /// <summary>
        /// 判断是否可以往上回滚订单状态
        /// </summary>
        /// <param name="Current">当前订单状态枚举值</param>
        /// <param name="Previous">要扭转的订单状态枚举值</param>
        /// <returns></returns>
        public static bool CheckToPrevious(this OrderStatusEnum Current, OrderStatusEnum Previous)
        {
            OrderStatusEnum[] array = ToPreviousStatusEnum(Current);
            if (array.Contains(Previous))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
