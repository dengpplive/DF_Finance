using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class JsonRet
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public object Object { get; set; }
        public object Contents { get; set; }

        public JsonRet(int Code = -1, int Count = 0, object Object = null)
        {
            if (Code == 0)
            {
                this.Success = true;
            }
            else
            {
                this.Success = false;
            }
            this.Code = Code;
            this.Object = Object;
            this.Count = Count;
        }

        public JsonRet(int Code = -1, string Message = "", object Object = null, object Contents = null)
        {
            if (Code == 0)
            {
                this.Success = true;
            }
            else
            {
                this.Success = false;
            }
            this.Code = Code;
            this.Message = Message;
            this.Object = Object;
            this.Contents = Contents;
            this.Count = Count;
        }

        public JsonRet(int Code = -1, int Count = 0, string Message = "", object Object = null, object Contents = null)
        {
            if (Code == 0)
            {
                this.Success = true;
            }
            else
            {
                this.Success = false;
            }
            this.Code = Code;
            this.Message = Message;
            this.Object = Object;
            this.Contents = Contents;
            this.Count = Count;
        }
    }
}
