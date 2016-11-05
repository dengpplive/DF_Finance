using DF.Finance.Common;
using DF.Finance.Common.Assert;
using DF.Finance.DAL;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class UserBLL : BaseBLL<User>
    {
        protected UserDAL _dal = new UserDAL();
        public Page<User> GetPage(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(dic["UserName"]))
            {
                sbWhere.AppendFormat(" and UserName like '%{0}%' ", dic["UserName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["IDCard"]))
            {
                sbWhere.AppendFormat(" and IDCard like '%{0}%' ", dic["IDCard"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["Mobile"]))
            {
                sbWhere.AppendFormat(" and Mobile like '%{0}%' ", dic["Mobile"].Replace("'", ""));
            }
            if (int.Parse(dic["GroupId"]) != 0)
            {
                sbWhere.AppendFormat(" and GroupId ='{0}' ", dic["GroupId"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["S_Time"]))
            {
                sbWhere.AppendFormat(" and  LoginTime >='{0}' ", dic["S_Time"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["E_Time"]))
            {
                sbWhere.AppendFormat(" and  LoginTime <='{0} 23:59:59' ", dic["E_Time"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString());
        }

        /// <summary>
        ///  上传Excel
        /// </summary>
        /// <param name="FilePath">文件保存的相对路径</param>
        /// <returns></returns>
        public RetObj UpExcel(string FilePath)
        {
            //将数据写入数据库
            DataSet excelDs = ExcelHelper.excelDs(FilePath);
            if (excelDs != null && excelDs.Tables[0].Rows.Count != 0)
            {
                return _dal.UpExcel(excelDs.Tables[0]);
            }
            return new RetObj(0, "Excel中没有数据，请仔细检查导入的Excel文件");
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids">删除的Ids</param>
        /// <returns></returns>
        public bool Delete(string ids)
        {
            return _dal.Update<User>(string.Format(" set  Status=1 where Id in({0})", ids)) > 0;
        }
        /// <summary>
        /// 获取指定组的会员用户
        /// </summary>
        /// <param name="groupId">组id</param>
        /// <returns></returns>
        public List<User> GetUserListByGroupId(int groupId)
        {
            return _dal.GetList(string.Format(" GroupId={0} ", groupId), null);
        }

        #region 前端
        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="verCode">发送短信的验证码</param>        
        /// <param name="groupId">类型 1.金融业代 2.销售顾问 3.贷款客户</param>
        /// <param name="dealersId">销售顾问 所属的 车行编号 </param>
        /// <returns></returns>
        public RetObj UserRegister(User user, string verCode, int groupId, int dealersId = 0)
        {
            //数据验证
            AssertUtil.IsNotNull<User>(user, "注册用户信息不能为空！");
            AssertUtil.NotNullOrWhiteSpace(user.Mobile, "用户手机号不能为空！");
            AssertUtil.NotNullOrWhiteSpace(user.PassWord, "登录密码不能为空！");
            AssertUtil.NotNullOrWhiteSpace(verCode, "注册手机验证码不能为空！");
            //手机验证码校验
            var smsCodeBLL = new SmsCodeBLL();
            var IsValid = smsCodeBLL.IsValid(user.Mobile, verCode);
            AssertUtil.IsTrue(IsValid, "注册手机验证码已失效，请重试");
            //该用户的手机号是否已注册
            AssertUtil.IsFalse(IsExistUser(user.Mobile), "用户" + user.Mobile + " 已注册，请直接登陆");
            //密码加密
            user.Salt = Encrypt.UniquePWDKey();
            user.PassWord = Encrypt.Encode(user.PassWord, user.Salt);
            user.GroupId = groupId;
            user.RegIp = IPHelper.GetIP();
            user.RegTime = System.DateTime.Now;
            //添加数据库
            var id = this.Add(user);
            //添加销售顾问扩展表
            if (dealersId > 0 && groupId == 2)
            {
                var bll = new UserSaleBLL();
                if (!bll.IsExistDealersId(dealersId))
                {
                    bll.Add(new UserSale()
                    {
                        DealersId = dealersId,
                        UserId = int.Parse(id.ToString())
                    });
                }
            }
            return new RetObj(user);
        }

        /// <summary>
        /// 会员登录的统一入口
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="pwdRequire">是否需要验证密码</param>
        /// <returns></returns>
        public RetObj Login(string mobile, string pwd, int groupId, bool pwdRequire = true)
        {
            var list = _dal.GetList(string.Format(" Mobile='{0}' or UserName='{0}' ", mobile), "");
            var user = (list != null && list.Count > 0) ? list.FirstOrDefault() : null;
            if (user == null)
                return new RetObj().Error("手机号不存在");
            if (user.Status == 3)
                return new RetObj().Error("用户已锁定");
            if (user.GroupId != groupId)
                return new RetObj().Error("手机号不存在,用户类型错误");
            if (pwdRequire)
            {
                string oldPwd = Encrypt.Encode(pwd, user.Salt);
                if (oldPwd != user.PassWord)
                {
                    return new RetObj().Error("用户密码错误");
                }
            }
            //查询提醒数目
            string sql = @"select COUNT(1) from Remind where IHaveRead=0 and IsLock=0 and UserId=" + user.Id;
            int RemindCount = this.ExecuteScalar<int>(sql);
            user.RemindCount = RemindCount;
            return new RetObj(user);
        }

        /// <summary>
        /// 是否存在该用户
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public bool IsExistUser(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;
            var list = _dal.GetList(string.Format(" Mobile='{0}' or UserName='{0}' ", mobile), "");
            var user = (list != null && list.Count > 0) ? list.FirstOrDefault() : null;
            if (user == null) return false;
            return true;
        }
        /// <summary>
        ///修改手机号
        /// </summary>
        /// <param name="userId">当前登录用户的Id</param>
        /// <param name="verCode">新手机号的验证码</param>
        /// <param name="newMobile">新手机号</param>
        /// <returns></returns>
        public RetObj UpdateMobile(int userId, string verCode, string newMobile)
        {
            //用户验证
            var user = _dal.GetModel(userId);
            if (user == null)
                return new RetObj().Error("当前用户请先登录");
            //手机验证码校验
            var smsCodeBLL = new SmsCodeBLL();
            var IsValid = smsCodeBLL.IsValid(newMobile, verCode, 2);
            AssertUtil.IsTrue(IsValid, "手机验证码已失效，请重试");
            //修改手机号
            var updateState = this.Update<User>(" set Mobile=@0 where Id=@1", newMobile, userId);
            return new RetObj(updateState);
        }
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="userId">当前登录用户的Id</param>
        /// <param name="oldPwd">原密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public RetObj UpdatePwd(int userId, string oldPwd, string newPwd)
        {
            var user = _dal.GetModel(userId);
            if (user == null)
                return new RetObj().Error("当前用户请先登录");
            if (Encrypt.Encode(oldPwd, user.Salt) != user.PassWord)
                return new RetObj().Error("输入原密码错误");
            string strNewPwd = Encrypt.Encode(newPwd, user.Salt);

            var updateState = this.Update<User>(" set PassWord=@0 where Id=@1", strNewPwd, user.Id);
            return new RetObj(updateState);
        }
        /// <summary>
        /// 销售顾问-修改邮箱
        /// </summary>
        /// <param name="userId">当前登录用户的Id</param>
        /// <param name="oldEmail">原绑定邮箱</param>
        /// <param name="newEmail">新邮箱</param>
        /// <returns></returns>
        public RetObj UpdateEmail(int userId, string oldEmail, string newEmail)
        {
            var user = this.GetModel(userId);
            if (user == null)
                return new RetObj().Error("当前用户请先登录");
            if (!string.IsNullOrEmpty(user.Email) && user.Email != oldEmail) return new RetObj().Error("原绑定邮箱错误或者不存在");

            var updateState = this.Update<User>(" set Email=@0 where Id=@1", newEmail, user.Id);
            return new RetObj(updateState);
        }


        /// <summary>
        /// 获取金融业代-个人中心数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType">用户类型 1.金融业代 2.销售顾问 3.客户</param>
        /// <returns></returns>
        public PersonCenterView GetPersonCenterData(int userId, int userType = 1)
        {
            var result = new PersonCenterView();
            string sql = @" select *,
                         (select COUNT(*) from Remind where IHaveRead=0 and IsLock=0 and UserId=Users.Id) as RemindCount 
                         from Users where GroupId=" + userType + " and Id=" + userId;
            var list = this.GetListExecute(sql);
            if (list != null && list.Count > 0)
            {
                var model = list.FirstOrDefault();
                result.Mobile = model.Mobile;
                result.UserName = model.UserName;
                result.Address = model.Area;
                result.RemindCount = model.RemindCount;
            }
            return result;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public User GetUserInfo(int userId)
        {
            string sql = @" select *,
                         (select COUNT(*) from Remind where IHaveRead=0 and IsLock=0 and UserId=Users.Id) as RemindCount 
                         from Users where  Id=" + userId;
            var list = this.GetListExecute(sql);
            if (list != null && list.FirstOrDefault() != null)
                return list.FirstOrDefault();

            return null;
        }
        /// <summary>
        /// 根据车行编号获取所属销售代表
        /// </summary>
        /// <param name="DealersId"></param>
        /// <returns></returns>
        public List<User> GetUserSalesByDealersId(int DealersId)
        {
            return new UserDAL().GetUserSalesByDealersId(DealersId);
        }
        #endregion

    }
}
