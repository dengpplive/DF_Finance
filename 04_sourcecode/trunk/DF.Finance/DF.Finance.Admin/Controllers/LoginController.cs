using DF.Finance.BLL;
using DF.Finance.Common.ValidateCode;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DF.Finance.Common;
using DF.Finance.Model.Enums;
using DF.Finance.WebCommon;
namespace DF.Finance.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult GetValidateCode()
        {
            string code;
            var vCode = new ValidateCode_Style1();
            var bytes = vCode.CreateImage(out code);
            TempData["ValidateCode"] = code;
            return File(bytes, @"image/jpeg");
        }
        [HttpPost]
        public JsonResult UserLogin(string username, string pwd, string ucode)
        {
            var userBll = new SysManageBLL();
            var msg = new RetObj(0, "");
            if (TempData["ValidateCode"] == null)
            {
                msg.Msg = "验证码已过期";
                msg.Code = -100;
            }
            else if (TempData["ValidateCode"].ToString().ToLower() != ucode.ToLower())
            {
                msg.Msg = "验证码输入错误";
                msg.Code = -100;
            }
            else
            {
                msg = userBll.UserLogin(username, pwd);
                if (msg.Code == 1)
                {
                    Session["logininfo"] = msg.Data;
                    //添加日志
                    WebRunTime.OpertionLog(DFEnums.ActionEnum.Login, msg.Data.Id, msg.Data.UserName);
                    //写入Cookie 1天
                    Utils.WriteCookie("curname", Encrypt.Encode(username), 24 * 60);
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



    }
}