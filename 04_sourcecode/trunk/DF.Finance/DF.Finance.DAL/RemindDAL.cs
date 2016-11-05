using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class RemindDAL : BaseDAL<Remind>
    {
       
        public List<Remind> GetRemindList(RemindRule model, string viewId)
        {
            string sql = string.Format("select max(AddTime) as AddTime ,ViewId,RemindType from  dbo.Remind WHERE RemindType={0} and ViewId in({1}) group by ViewId,RemindType", model.RemindType, viewId);
            return base.GetListExecute(sql);
        }
        public void UpdateIHaveRead(string id)
        {
            string sql = string.Format("update [Remind] SET [IHaveRead] = 1  WHERE Id in({0})", id);
            db.Execute(sql);
        }
    }
}
