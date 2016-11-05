using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
   public partial class OrderInsurance
    {
       public List<OrderInsuranceImg> OrderInsuranceImgs { get; set; }

       /// <summary>
       /// 保单资料图片地址
       /// </summary>
       public string photo_name { get; set; }

       /// <summary>
       /// 保单资料图片备注
       /// </summary>
       public string photo_remark { get; set; }
    }
}
