using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    //会员相关，
    public class UserController : BaseWebController
    {
        [HttpPost]
        public JsonResult Login(string mobile, string pwd)
        {
            mobile = "15297914278";
            pwd = "123456";
            UserBLL bll = new UserBLL();
            var msg = new RetObj(0, "");
            msg = bll.Login(mobile, pwd);
            if (msg.Code == 1) 
            {
                //记录用户登录日志
                UserLoginLogBLL UserLoginLogBLL = new UserLoginLogBLL();
                UserLoginLogBLL.Add(new UserLoginLog()
                {
                    UserId=msg.Data.Id,
                    UserName = msg.Data.UserName,
                    LoginTime = System.DateTime.Now,
                    LoginIP = IPHelper.GetIP(),
                    Remark="会员登录"

                });
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}