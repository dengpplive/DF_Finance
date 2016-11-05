using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class PageSizeException:ApplicationException
    {

        public PageSizeException(int PageSize)
            : base(string.Format("请求页数不能超过{0}",PageSize))
        {
        }
        
    }
}
