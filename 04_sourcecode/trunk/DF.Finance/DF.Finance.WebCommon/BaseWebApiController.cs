using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DF.Finance.WebCommon
{
    public class BaseWebApiController : ApiController
    {
        /// <summary>
        /// 检测访问页面是否登录
        /// </summary>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj CheckLogin()
        {
            return new RetObj();
        }
        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="groupId">登录类型 1.金融业代 2.销售顾问 3.贷款客户</param>
        /// <returns></returns>
        [HttpGet]
        public RetObj UserLogin(string mobile, string pwd, int groupId)
        {
            UserBLL bll = new UserBLL();
            var msg = new RetObj(0, "");
            msg = bll.Login(mobile, pwd, groupId);
            if (msg.Code == 1)
            {
                //记录用户登录日志
                UserLoginLogBLL UserLoginLogBLL = new UserLoginLogBLL();
                UserLoginLogBLL.Add(new UserLoginLog()
                {
                    UserId = msg.Data.Id,
                    UserName = msg.Data.UserName,
                    LoginTime = System.DateTime.Now,
                    LoginIP = IPHelper.GetIP(),

                    Remark = "会员登录"
                });
                //写入Cookie 30天
                int time = 24 * 60 * 30;
                Utils.WriteCookie("userId", msg.Data.Id.ToString(), time);
            }
            return msg;
        }
        /// <summary>
        /// 会员注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj UserLoginOut()
        {
            Utils.WriteCookie("userId", "", -1);
            return new RetObj();
        }
        /// <summary>
        /// 获取当前的登陆用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetCurrentUserInfo()
        {
            string userId = Utils.GetCookie("userId");
            var userBLL = new UserBLL();
            int _userId = 0;
            int.TryParse(userId, out _userId);
            var data = userBLL.GetUserInfo(_userId);
            //var result = (data == null) ? new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆") : new RetObj(data);
            return new RetObj(data);
        }
        public int GetUserId()
        {
            string userId = Utils.GetCookie("userId");
            int _userId = 0;
            int.TryParse(userId, out _userId);
            return _userId;
        }
        /// <summary>
        /// 绑定新手机号
        /// </summary>
        /// <param name="verCode">新手机号验证码</param>
        /// <param name="mobile">新手机号</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj UpdateMobile(string verCode, string mobile)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);

            var userBLL = new UserBLL();
            return userBLL.UpdateMobile(userId, verCode, mobile);
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="oldPwd">原密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj UpdatePwd(string oldPwd, string newPwd)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);

            var userBLL = new UserBLL();
            return userBLL.UpdatePwd(userId, oldPwd, newPwd);
        }
        /// <summary>
        /// 获取部门管理下面的区域
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetArea()
        {
            var list = new SysDictionaryBLL().GetDicByTypeCode("area_city");
            return new RetObj(list);
        }
        /// <summary>
        /// 获取指定节点下面的数据
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetNodeList(int id)
        {
            var list = new SysDictionaryBLL().GetListByParentId(id);
            return new RetObj(list);
        }
        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="sourceType">1.注册 2.更换新手机号</param>
        /// <returns></returns>
        [HttpPost]
        public RetObj SendSmsCode([FromBody] SendSmsCode sendSmsCode)
        {
            //生成verCode
            var verCode = AssistantHelper.GetRandomNumber(6);
            int _minute = 10;//默认10分钟
#if DEBUG
            verCode = "888888";
#else
                        //获取短信的参数设置
                        SysDictionaryBLL dicBLL = new SysDictionaryBLL();
                        var dicData = dicBLL.GetListByTypeCode("sms_setting");
                        var sms_url = dicData.Where(p => p.Name.ToLower() == "sms_url").FirstOrDefault();
                        var valid_time = dicData.Where(p => p.Name.ToLower() == "valid_time").FirstOrDefault();
                        var verCode_key = dicData.Where(p => p.Name.ToLower() == "vercode_key").FirstOrDefault();
                        string url = "http://112.74.115.8:899/api/send/SendOneByTemp";          
                        string key = "dfusercode";
                        if (sms_url != null && !string.IsNullOrEmpty(sms_url.Val)) url = sms_url.Val;
                        if (valid_time != null && !string.IsNullOrEmpty(valid_time.Val))
                        {
                            if (!int.TryParse(valid_time.Val, out _minute))
                            {
                                _minute = 10;
                            }
                        } if (verCode_key != null && !string.IsNullOrEmpty(verCode_key.Val)) key = verCode_key.Val;
                        //发送短信   
                        var smsBLL = new SmsBLL();
                        string strResult = smsBLL.SendSmsCode(url, sendSmsCode.mobile, verCode, key, _minute);
#endif


            var smsCodeBLL = new SmsCodeBLL();
            var iSuccess = smsCodeBLL.AddOrUpdateSmsCode(sendSmsCode.mobile, verCode, _minute, sendSmsCode.sourceType);
            var result = new RetObj(iSuccess);
            return !iSuccess ? result.Error("发送失败，请重试") : result;
        }
        /// <summary>
        /// 获取设置的短信的失效时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj GetSmsValidTime()
        {
            int _minute = 5;
            SysDictionaryBLL dicBLL = new SysDictionaryBLL();
            var dicData = dicBLL.GetListByTypeCode("sms_setting");
            var valid_time = dicData.Where(p => p.Name.ToLower() == "valid_time").FirstOrDefault();
            if (valid_time != null && !string.IsNullOrEmpty(valid_time.Val))
            {
                if (!int.TryParse(valid_time.Val, out _minute))
                {
                    _minute = 10;
                }
            }
            return new RetObj(_minute);
        }
        /// <summary>
        /// 验证手机短信验证码是否有效
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="verCode">验证码</param>
        /// <param name="sourceType">类型: 1.注册 2.更换新手机号</param>
        /// <returns></returns>
        [HttpGet]
        public RetObj SmsVerCodeIsValid(string mobile, string verCode, int sourceType)
        {
            var smsCodeBLL = new SmsCodeBLL();
            var bRet = smsCodeBLL.IsValid(mobile, verCode, sourceType);
            return new RetObj(bRet);

        }
        /// <summary>
        /// 提醒消息列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>        
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetRemindList(int pageIndex, int pageSize)
        {
            string strUserId = Utils.GetCookie("userId");
            int userId = 0;
            int.TryParse(strUserId, out userId);
            if (userId == 0) return new RetObj().Error("该用户未登录或者登陆信息已失效,请登陆");
            var data = new RemindBLL().GetRemindList(pageIndex, pageSize, userId);
            if (data == null)
            {
                return new RetObj().Error("没有提醒的订单数据");
            }
            var ihlist = data.Items.Where(p => p.IHaveRead == 0);
            if (ihlist.Count() > 0)
            {
                new BLL.RemindBLL().UpdateIHaveRead(string.Join(",", ihlist.Select(p => p.Id).ToArray()));
            }
            return new RetObj(new { HaveCount = ihlist.Count(), List = data });
        }
    }
}
