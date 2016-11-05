using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 抽奖结果页面结果数据
    /// </summary>
    public partial class PrizeView
    {
        /// <summary>
        /// 抽奖活动
        /// </summary>
        public Activity activity { get; set; }
        /// <summary>
        /// 参与抽奖的用户手机号
        /// </summary>
        public List<string> tels = new List<string>();
        /// <summary>
        /// 中奖信息
        /// </summary>
        public List<ActWinning> winners = new List<ActWinning>();
        /// <summary>
        /// 奖项信息
        /// </summary>
        public List<AwardGrade> awardgrades = new List<AwardGrade>();

    }
    public class AwardGrade
    {
        /// <summary>
        /// 奖项 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 奖项名称
        /// </summary>
        public string typeName { get; set; }
        /// <summary>
        /// 中奖数
        /// </summary>
        public int WinningCount { get; set; }
    }
    public class ActWinning
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 奖项 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 奖项名称
        /// </summary>
        public string typeName { get; set; }
    }

    public class Tels
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 奖项类型
        /// </summary>
        public int PrizeClass { get; set; }

    }
}
