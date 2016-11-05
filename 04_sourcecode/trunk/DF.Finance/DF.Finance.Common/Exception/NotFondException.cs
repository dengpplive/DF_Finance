using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class NotFondException:ApplicationException
    {

        public NotFondException(): base(string.Format("所需实例不存在"))
        {
        }
        
    }
}
