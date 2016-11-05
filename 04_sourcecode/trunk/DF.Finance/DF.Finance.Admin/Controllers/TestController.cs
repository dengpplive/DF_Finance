using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("aa")]
    public class TestController : Controller
    {
        //
        // GET: /Test/
        [AdminRole]
        public ActionResult Index()
        {
            //var model = new BLL.NotesBLL().GetModel("1");
            //var listPage = new BLL.NotesBLL().GetPageList(1, 10);
            //var list = new BLL.NotesBLL().GetList();
           
            return View();
        }
    }
}