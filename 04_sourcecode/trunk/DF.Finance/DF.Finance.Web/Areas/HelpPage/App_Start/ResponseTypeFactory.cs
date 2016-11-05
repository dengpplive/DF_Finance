using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF.Finance.Web.Areas.HelpPage
{
    public class ResponseTypeFactory
    {
        private static Dictionary<string, Type> _responseTypes = new Dictionary<string, Type>()
        {
           
        };

        private static string OptApiId(string apiId)
        {
            return apiId.Split('?')[0].ToUpper();
        }

        public static bool Contains(string apiId)
        {
            apiId = OptApiId(apiId);
            return _responseTypes.ContainsKey(apiId);
        }

        public static Type Get(string apiId)
        {
            apiId = OptApiId(apiId);
            if (Contains(apiId))
            {
                return _responseTypes[apiId];
            }
            return null;
        }
    }
}