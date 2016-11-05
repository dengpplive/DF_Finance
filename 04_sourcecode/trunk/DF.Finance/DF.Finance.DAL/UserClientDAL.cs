using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class UserClientDAL : BaseDAL<UserClient>
    {
        /// <summary>
        /// 获取审批通过的用户列表
        /// </summary>
        /// <returns></returns>
        public List<UserClient> GetInvolvedUserList()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" select UserClient.*,Users.* from dbo.UserClient ");
            sbSQL.Append(" inner join dbo.Users on UserClient.UserId=Users.Id ");
            sbSQL.Append(" where (UserClient.ApprovedTime is not null and CONVERT(VARCHAR(10),UserClient.ApprovedTime,120)<>'1900-01-01') and UserClient.ApprovedTime<GETDATE()  and Users.Status=0 ");
            return db.Fetch<UserClient, User>(sbSQL.ToString());
        }
    }
}
