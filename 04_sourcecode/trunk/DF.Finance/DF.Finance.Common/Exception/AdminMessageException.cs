using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
   public class AdminMessageException: Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AdminMessageException(string message):base(message)
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AdminMessageException(string message, Exception innerException): base(message, innerException)
        {

        }
    
    }
}
