using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DF.Finance.Web.App_Start
{
    public class WebApiDoc
    {
        public static void Create()
        {
            try
            {
                var xml1 = XDocument.Load(HttpContext.Current.Server.MapPath("/App_Data/ModelDocument.xml"));
                var xml2 = XDocument.Load(HttpContext.Current.Server.MapPath("/App_Data/XmlDocument.xml"));
                xml1.Root.Add(xml2.Root.DescendantNodes());
                xml1.Save(HttpContext.Current.Server.MapPath("~/App_Data/AllDocument.xml"));
            }
            catch (Exception)
            {
            }
        }
    }
}