using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.DBUtility.PetaPoco;
namespace DF.Finance.Model
{
    public partial class ActRecord
    {
        /// <summary>
        /// 参与用户名称
        /// </summary>
        [ResultColumn]
        public string UserName { get; set; }
    }
}
