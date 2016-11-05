using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Test/
        [HttpPost]
        public JsonResult Upload()
        {
            HttpPostedFileBase _upfile = Request.Files["Filedata"];
            if (_upfile == null)
            {
                return Json(new RetObj().Error("请选择要上传文件"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                string fileExt = Utils.GetFileExt(_upfile.FileName);
                string fileName = Utils.GetRamCode() + "." + fileExt;
                string uploadName = "/upload/" + GetUpLoadPath() + "/";
                string uploadFile = Utils.GetMapPath(uploadName);
                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(uploadFile))
                {
                    Directory.CreateDirectory(uploadFile);
                }
                _upfile.SaveAs(uploadFile + fileName);

                return Json(new RetObj(new { name = fileName, path = uploadName + fileName, thumb = "", ext = fileExt }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                 return Json(new RetObj().Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 按年月存储
        /// </summary>
        /// <returns></returns>
        private string GetUpLoadPath()
        {
          
            return DateTime.Now.ToString("yyyyMM");
        }
    }
}