using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF.Finance.Web.Filter
{
    /// <summary>
    /// 登录特性 用在需要登录的action和控制器上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LoginAttribute : Attribute
    {
        public LoginAttribute() { }
    }
}