using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class MoreThanOneException:ApplicationException
    {
        public string ParamName = "";

        public MoreThanOneException()
            : base(string.Format("相应数据已存在,不能再添加"))
        {

        }
        public MoreThanOneException(string ParamName)
            :base(string.Format("{0}相应数据已存在,不能再添加", ParamName))
        {
            this.ParamName = ParamName;
            //this.Message = string.Format("{0}数据已存在,不能再添加", ParamName);

        }
        public MoreThanOneException(string Messager, Exception  Inner):base(Messager,Inner)
        {

        }
    }
}
