using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 短信验证码
    /// </summary>
    public class SmsCodeBLL : BaseBLL<SmsCode>
    {
        /// <summary>
        /// 该手机号的验证码是否有效
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="smsCode">验证码</param>
        /// <param name="sourceType">1.注册 2.更换新手机号</param>
        /// <returns></returns>
        public bool IsValid(string mobile, string verCode,int sourceType=1)
        {
            return this.Exists(string.Format(" SourceType={0} and  Mobile='{1}' and VerCode='{2}' and FailTime>getdate()",sourceType, mobile, verCode));
        }
        /// <summary>
        /// 添加或者刷新手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="verCode">验证码</param>
        /// <param name="minutes">有效时长（分钟）</param>
        /// <param name="sourceType">1.注册 2.更换新手机号</param>
        /// <returns></returns>
        public bool AddOrUpdateSmsCode(string mobile, string verCode, int minutes,int sourceType=1)
        {
            bool IsSuccess = false;
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(verCode))
            {
                SmsCode smsCode = new SmsCode();
                var list = this.GetList(string.Format(" SourceType={0} and  Mobile='{1}' ", sourceType,mobile));
                bool isExist = (list != null && list.FirstOrDefault() != null);
                if (isExist) smsCode = list.FirstOrDefault();

                smsCode.FailTime = System.DateTime.Now.AddMinutes(minutes);
                smsCode.Mobile = mobile;
                smsCode.SourceType = sourceType;
                smsCode.VerCode = verCode;
                if (isExist) this.Update(smsCode); else this.Add(smsCode);
                IsSuccess = true;
            }
            return IsSuccess;
        }
    }
}
