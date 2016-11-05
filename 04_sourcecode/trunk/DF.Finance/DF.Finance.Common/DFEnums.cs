using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DF.Finance.Common
{
    public class DFEnums
    {
        /// <summary>
        /// 统一管理操作枚举
        /// </summary>
        public enum ActionEnum : int
        {
            /// <summary>
            /// 所有
            /// </summary> 
            [Description("所有")]
            All = 0,
            /// <summary>
            /// 显示
            /// </summary>
            [Description("显示")]
            Show = 1,
            /// <summary>
            /// 查看
            /// </summary>
            [Description("查看")]
            View = 2,
            /// <summary>
            /// 添加
            /// </summary>
            [Description("添加")]
            Add = 3,
            /// <summary>
            /// 修改
            /// </summary>
            [Description("修改")]
            Edit = 4,
            /// <summary>
            /// 删除
            /// </summary>
            [Description("删除")]
            Delete = 5,
            /// <summary>
            /// 审核
            /// </summary>
            [Description("审核")]
            Audit = 6,
            /// <summary>
            /// 回复
            /// </summary>
            [Description("回复")]
            Reply = 7,
            /// <summary>
            /// 确认
            /// </summary>
            [Description("确认")]
            Confirm = 8,
            /// <summary>
            /// 取消
            /// </summary>
            [Description("取消")]
            Cancel = 9,
            /// <summary>
            /// 作废
            /// </summary>
            [Description("作废")]
            Invalid = 10,
            /// <summary>
            /// 生成
            /// </summary>
            [Description("生成")]
            Build = 11,
            /// <summary>
            /// 安装
            /// </summary>
            [Description("安装")]
            Instal = 12,
            /// <summary>
            /// 卸载
            /// </summary>
            [Description("卸载")]
            UnLoad = 13,
            /// <summary>
            /// 登录
            /// </summary>
            [Description("登录")]
            Login = 14,
            /// <summary>
            /// 备份
            /// </summary>
            [Description("备份")]
            Back = 15,
            /// <summary>
            /// 还原
            /// </summary>
            [Description("还原")]
            Restore = 16,
            /// <summary>
            /// 替换
            /// </summary>
            [Description("替换")]
            Replace = 17,
            /// <summary>
            /// 存款
            /// </summary>
            [Description("存款")]
            Deposit = 18,
            /// <summary>
            /// 取款
            /// </summary>
            [Description("取款")]
            Withdraw = 19,
            /// <summary>
            /// 权限
            /// </summary>
            [Description("权限")]
            Permission = 20           
        }

        /// <summary>
        /// 系统导航菜单类别枚举
        /// </summary>
        public enum NavigationEnum : int
        {
            /// <summary>
            /// 系统后台菜单
            /// </summary>
            [Description("系统后台菜单")]
            System = 0,
            /// <summary>
            /// 新闻中心
            /// </summary>
            [Description("新闻中心")]
            News = 1,
            /// <summary>
            /// 学车通教练
            /// </summary>
            [Description("学车通教练")]
            Coach = 2,
            /// <summary>
            /// 会员中心导航
            /// </summary>
            [Description("会员中心导航")]
            Users = 3,
            /// <summary>
            /// 网站主导航
            /// </summary>
            [Description("网站主导航")]
            WebSite = 4
        }

        /// <summary>
        /// 用户生成码枚举
        /// </summary>
        public enum CodeEnum : int
        {
            /// <summary>
            /// 注册验证
            /// </summary>
            [Description("注册验证")]
            RegVerify = 0,
            /// <summary>
            /// 邀请注册
            /// </summary>
            [Description("邀请注册")]
            Register = 1,
            /// <summary>
            /// 取回密码
            /// </summary>
            [Description("取回密码")]
            Password = 2
        }

        /// <summary>
        /// 金额类型枚举
        /// </summary>
        public enum AmountTypeEnum : int
        {
            /// <summary>
            /// 系统赠送
            /// </summary>
            [Description("系统赠送")]
            SysGive = 0,
            /// <summary>
            /// 在线充值
            /// </summary>
            [Description("在线充值")]
            Recharge = 1,
            /// <summary>
            /// 用户消费
            /// </summary>
            [Description("用户消费")]
            Consumption = 2,
            /// <summary>
            /// 购买商品
            /// </summary>
            [Description("购买商品")]
            BuyGoods = 3,
            /// <summary>
            /// 积分兑换
            /// </summary>
            [Description("积分兑换")]
            Convert = 4
        }
    }
}
