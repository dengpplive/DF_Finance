using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    /// <summary>
    /// 2016-03-24 jim 添加 
    /// app处理异常类
    /// </summary>
    public class AppMessageException: Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AppMessageException(string message):base(message)
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AppMessageException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
