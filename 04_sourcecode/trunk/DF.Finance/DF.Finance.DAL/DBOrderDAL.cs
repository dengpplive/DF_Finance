﻿using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class DBOrderDAL : BaseDAL<DBOrder>
    {
        public DBOrder GetModelByDBOrderNum(string DBOrderNum)
        {
            return db.SingleOrDefault<DBOrder>(string.Format("select * from DBOrder where DBOrderNum='{0}'", DBOrderNum));
        }
    }
}
