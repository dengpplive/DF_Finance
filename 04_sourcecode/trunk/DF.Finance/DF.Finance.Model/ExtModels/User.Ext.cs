using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class User
    {
        /// <summary>
        /// 提醒的消息数量
        /// </summary>
        [ResultColumn]
        public int RemindCount { get; set; }
        public UserSale UserSale { get; set; }

        public UserGroup UserGroup { get; set; }

        public UserAgent UserAgent { get; set; }

        public UserClient UserClient { get; set; }

        public List<UserLoginLog> UserLoginLogs { get; set; }

        public List<UserPointLog> UserPointLogs { get; set; }
    }
}
