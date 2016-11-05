using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("Users")]
    public class UserController : BaseAdminController
    {
        List<UserGroup> Groups = new UserGroupBLL().GetList(" IsLock=0 ").ToList();
        // GET: /User/
        public ActionResult Index()
        {
            ViewBag.Groups = Groups;
            return View();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public JsonResult Search()
        {
            var dic = Utils.GetRequest();
            UserBLL UserBLL = new UserBLL();
            var Users = UserBLL.GetPage(dic);
            var total = Users.TotalItems;
            var rows = Users.Items;
            var strId = string.Join(",", rows.Select(p => p.GroupId).ToArray());
            if (strId != "" && strId != null) 
            {
                var Grouplist = new UserGroupBLL().GetList(string.Format(" id in({0})", strId));
                foreach (var m in Users.Items)
                {
                    m.UserGroup = Grouplist.Where(p => p.Id == m.GroupId).FirstOrDefault<UserGroup>();
                }
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
         [HttpPost, AdminRole(ActionType = "Add|Edit")]
        public JsonResult AddOrEdit(User user)
        {
            UserBLL userBll = new UserBLL();
            if (user.Id > 0)
            {
                user.PassWord = Encrypt.Encode(user.PassWord, user.Salt);
                userBll.Update(user);
            }
            else
            {
                userBll.Add(user);
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int Id)
        {
            ViewBag.Groups = Groups;
            UserBLL userBll = new UserBLL();
            var model = Id > 0 ? userBll.GetModel(Id) : new User();
            model.PassWord = Encrypt.Decode(model.PassWord, model.Salt);
            return View(model);
        }

        /// <summary>
        /// 删除用户,逻辑删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string ids)
        {
            UserBLL bll = new UserBLL();
            if (ids != null && ids.Split(',').Length > 0)
            {
                bll.Delete(ids);
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload() 
        {
            return View();
        }

        /// <summary>
        /// 文件保存的相对路径
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        [HttpGet, AdminRole(ActionType = "Upload")]
        public JsonResult Up(string FilePath) 
        {
            //string FileName = Utils.GetFileName(FilePath);//文件名
            //string FileExt = Utils.GetFileExt(FileName);//文件扩展名，不含.
            bool IsExist = Utils.FileExists(FilePath);
            if (IsExist)
            {
                RetObj RetObj = new UserBLL().UpExcel(FilePath);
                return Json(RetObj, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(new RetObj(0,"文件不存在，请仔细核对文件的上传目录"), JsonRequestBehavior.AllowGet);
            }
        }
    }
}