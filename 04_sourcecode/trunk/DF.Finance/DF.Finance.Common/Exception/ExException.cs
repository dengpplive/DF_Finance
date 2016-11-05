using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class ExException : ApplicationException
    {
        public string Messages = "";
        public int Code = 0;
        public bool Success = false;
        public object Object = null;

        public ExException(int Code = -1, string Message = "", object Object = null)
            : base(string.Format(Message))
        {

            if (Code == 0)
            {
                this.Success = true;
            }
            else
            {
                this.Success = false;
            }

            this.Object = Object;
            this.Code = Code;
            this.Messages = Message;
        }
    }
}
