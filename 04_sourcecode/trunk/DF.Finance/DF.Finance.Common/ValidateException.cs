using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class ValidateException : Exception
    {
        public ValidateException(string message)
            : base(message)
        {

        }
    }

    public static class Throw
    {
        public static void Ex(string message)
        {
            throw new ValidateException(message);
        }
    }
}
