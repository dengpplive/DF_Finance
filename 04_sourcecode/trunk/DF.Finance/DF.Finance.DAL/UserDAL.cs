using DF.Finance.Common;
using DF.Finance.DBUtility.PetaPoco;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class UserDAL : BaseDAL<User>
    {
        public RetObj UpExcel(DataTable ExcelDt)
        {
            try
            {
                using (var scope = db.GetTransaction()) 
                {
                    for(int i = 0; i < ExcelDt.Rows.Count; i++)
                    {
                        User model = new User();
                        if (ExcelDt.Rows[i]["姓名"].ToString() != "" && ExcelDt.Rows[i]["手机号"].ToString() != "" && ExcelDt.Rows[i]["身份证"].ToString() != "" && ExcelDt.Rows[i]["审批通过日期"].ToString() != "")
                        {
                            model.UserName = ExcelDt.Rows[i]["姓名"].ToString();
                            model.Salt = "12345678";
                            model.PassWord = Encrypt.Encode(ExcelDt.Rows[i]["身份证"].ToString().Substring(ExcelDt.Rows[i]["身份证"].ToString().Length - 6, 6), model.Salt);
                            model.Mobile = ExcelDt.Rows[i]["手机号"].ToString();
                            model.IDCard = ExcelDt.Rows[i]["身份证"].ToString();
                            model.RegTime = DateTime.Now;
                            model.RegIp = "127.0.0.1";
                            model.GroupId = 3;
                            object id = base.Add(model);//1
                            if (id != null) 
                            {
                                UserClient UserClient = new UserClient();
                                UserClient.UserId = int.Parse(id.ToString());
                                UserClient.ApprovedTime = DateTime.Parse(ExcelDt.Rows[i]["审批通过日期"].ToString());
                                new UserClientDAL().Add(UserClient);
                            }
                        }
                        
                    }
                    scope.Complete();
                }
                return new RetObj();
            }
            catch(Exception ex) 
            {
                return new RetObj(0, ex.Message);
            }
        }
        /// <summary>
        /// 根据车行编号获取所属销售代表
        /// </summary>
        /// <param name="DealersId"></param>
        /// <returns></returns>
        public List<User> GetUserSalesByDealersId(int DealersId)
        {

            return db.Query<User>("SELECT a.* from dbo.Users as a,dbo.UserSales as b where a.Id=b.UserId and b.DealersId=@0  ", DealersId).ToList();
        }
    }
}
