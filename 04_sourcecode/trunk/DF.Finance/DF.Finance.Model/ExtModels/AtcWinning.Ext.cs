using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.DBUtility.PetaPoco;
namespace DF.Finance.Model
{
    public partial class AtcWinning
    {
        /// <summary>
        /// 中奖用户名称
        /// </summary>
        [ResultColumn]
        public string UserName { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        [ResultColumn]
        public string Title { get; set; }
        /// <summary>
        /// 奖项
        /// </summary>

        [ResultColumn]
        public int PrizeClass { get; set; }
    }
}
