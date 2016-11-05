using DF.Finance.Common;
using DF.Finance.DAL;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 兑吧订单
    /// </summary>
    public class DBOrderBLL : BaseBLL<DBOrder>
    {

        public DBOrder GetModelByDBOrderNum(string DBOrderNum)
        {
            return new DBOrderDAL().GetModelByDBOrderNum(DBOrderNum);
        }
    }
}
