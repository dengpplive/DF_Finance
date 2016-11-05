using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    //金融业代
    public class SalesController : BaseWebController
    {
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        //登录
        public ActionResult Login()
        {
            return View();
        }

        //前端测试用,异步提交暂时提交到这个 --liurongchang
        public string T()
        {
            var random = new Random(1);
            var randomNum = random.Next(3000);
            System.Threading.Thread.Sleep(randomNum);
            //返回json格式字符串，具体格式，后台人员确定
            var successStr = "{\"Success\":true,\"Message\":\"mmm\"}";
            var errorStr = "{\"Success\":false,\"Message\":\"某错误\"}";
            if (randomNum % 2 == 0)
            {
                return successStr;
            }
            return errorStr;
        }

    }
}