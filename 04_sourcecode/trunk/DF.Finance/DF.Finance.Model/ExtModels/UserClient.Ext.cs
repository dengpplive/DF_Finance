using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class UserClient
    {
        public User User { get; set; }
        /// <summary>
        /// 抽奖次数
        /// </summary>
        [Ignore]
        public int PrizeCount { get; set; }
    }
}
