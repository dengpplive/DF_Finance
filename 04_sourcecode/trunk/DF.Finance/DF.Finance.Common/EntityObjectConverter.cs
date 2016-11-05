using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Data.Objects.DataClasses;

namespace DF.Finance.Common
{
    public class EntityObjectConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                string str = "";
                foreach (var item in dictionary)
                {
                    string seprator = (str == "") ? "" : ",";
                    str += String.Format("{0}\"{1}\"=\"{2}\"", seprator, item.Key, item.Value.ToString());
                }

                return serializer.Deserialize(str, type);
            }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            EntityObject e = obj as EntityObject;

            if (e != null)
            {
                PropertyInfo[] pis = e.GetType().GetProperties();
                foreach (var pi in pis)
                {
                    if (pi.IsDefined(typeof(EdmScalarPropertyAttribute), false))
                    {
                        result[pi.Name] = pi.GetValue(e, null);
                    }
                }

            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(EntityObject) }; }
        }
    }
}
