using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class UserPointLogDAL : BaseDAL<UserPointLog>
    {
        /// <summary>
        /// 添加数据 带事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CompleteAdd(UserPointLog model)
        {
            try
            {
                using (var scope = db.GetTransaction())
                {
                    object id = base.Add(model);
                    if (id != null)
                    {
                        new UserDAL().Update<User>(string.Format(" SET [Point] =Point+{0} WHERE Id={1}", model.Value, model.UserId));
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
               
                return false;
            }
            return true;
        }
    }
}
