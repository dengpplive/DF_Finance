using DF.Finance.BLL;
using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DF.Finance.Admin
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string a = "('‘’')";
            //Response.Write(Utils.GetDefaultSetting("a", "string", a));
            //Response.Write(new RemindBLL().RemindTasks());
            ////var md = method();
            ////Response.Write(md);
            ////Response.Write("<br>" + md.value);
            ////Response.Write("<br>" + md.id);
            //var str = "1,2,3,4,5,6";
            //var a = str.Split(',').Select(p => int.Parse(p)).ToArray();
            CacheHelper.Insert("aaa", "bbb");
           

        }
        private dynamic method()
        {
            return new { value = "BB", id = 1 };
        }
    }
}