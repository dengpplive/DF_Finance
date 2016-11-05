using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Web.Controllers
{
    public class TController : Controller
    {
        //
        // GET: /T/
        public ActionResult Index()
        {
            ViewBag.data = "";
            return View();
        }
	}
}