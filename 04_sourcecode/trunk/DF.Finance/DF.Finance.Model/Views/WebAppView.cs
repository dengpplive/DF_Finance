using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 发送短信验证码信息
    /// </summary>
    public class SendSmsCode
    {
        [Display(Name = "注册用户手机号")]
        public string mobile { get; set; }
        [Display(Name = "类型： 1.注册 2.更换新手机号")]
        public int sourceType { get; set; }
    }
    /// <summary>
    /// 注册用户信息
    /// </summary>
    public class UserRegister
    {
        [Display(Name = "注册用户手机号")]
        public string mobile { get; set; }
        [Display(Name = "注册用户手机验证码")]
        public string verCode { get; set; }
        [Display(Name = "注册用户登陆密码")]
        public string pwd { get; set; }
        [Display(Name = "注册用户区域地址")]
        public string area { get; set; }

        [Display(Name = "销售顾问的注册邮箱")]
        public string email { get; set; }
        [Display(Name = "销售顾问所属的车行编号")]
        public int dealersId { get; set; }
        [Display(Name = "类型 1.金融业代 2.销售顾问 3.贷款客户")]
        public int groupId { get; set; }
    }
    /// <summary>
    /// 金融业代-个人中心
    /// </summary>
    public class PersonCenterView
    {
        private string _UserName = string.Empty;
        private string _HeaderSrc = string.Empty;
        private string _Address = string.Empty;
        private int _RemindCount = 0;
        private string _Mobile = string.Empty;
        /// <summary>
        /// 金融业代姓名
        /// </summary>
        [Display(Name = "金融业代姓名")]
        public string UserName { get { return _UserName; } set { _UserName = value; } }
        /// <summary>
        /// 金融业代头像路径
        /// </summary>
        [Display(Name = "金融业代头像路径")]
        public string HeaderSrc { get { return _HeaderSrc; } set { _HeaderSrc = value; } }

        /// <summary>
        /// 金融业代区域地址
        /// </summary>
        [Display(Name = "金融业代区域地址")]
        public string Address { get { return _Address; } set { _Address = value; } }
        /// <summary>
        /// 提醒的消息数量
        /// </summary>
        [Display(Name = "提醒的消息数量")]
        public int RemindCount { get { return _RemindCount; } set { _RemindCount = value; } }
        /// <summary>
        /// 绑定手机号
        /// </summary>
        [Display(Name = "绑定的手机号")]
        public string Mobile { get { return _Mobile; } set { _Mobile = value; } }
    }
    /// <summary>
    /// 实际交易方
    /// </summary>
    public class CustomerManagementView
    {
        [Display(Name = "编号")]
        public int Id { get; set; }
        [Display(Name = "实际交易方公司名称")]
        public string CompanyName { get; set; }
    }

    /// <summary>
    /// 我的积分
    /// </summary>
    public class MyPoint
    {
        public int TotalPoint { get; set; }
        /// <summary>
        /// 积分列表
        /// </summary>
        public Page<UserPointLog> PointList { get; set; }
    }

}
