using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举描述特性值
        /// </summary>
        /// <param name="_enum">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetEnumDesc(this Object _enum)
        {
            Type type = _enum.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue必须是一个枚举值", "_enum");
            }

            //使用反射获取该枚举的成员信息
            MemberInfo[] memberInfo = type.GetMember(_enum.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //返回枚举值得描述信息
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //如果没有描述特性的值，返回该枚举值得字符串形式
            return _enum.ToString();
        }

        public static string GetDescByValue<T>(string value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("EnumerationValue必须是一个枚举值", typeof(T).ToString());

            object obj = Enum.Parse(typeof(T), value);
            return GetEnumDesc(obj);
        }

        public static Dictionary<string, string> GetAllKeyValues<T>()
        {
            Type tType = typeof(T);
            if (!tType.IsEnum)
                throw new ArgumentException("EnumerationValue必须是一个枚举值", typeof(T).ToString());

            var result = new Dictionary<string, string>();
            string[] arr = Enum.GetNames(tType);
            for (int i = 0; i < arr.Length; i++)
            {
                result[Enum.Format(tType, Enum.Parse(tType, arr[i]), "d")] = GetDescByValue<T>(arr[i]);
            }
            return result;
        }

        static Hashtable hs = new Hashtable();
        /// <summary>
        /// 将指定枚举类型的项以键值对的形式列举出来
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<T?, string>> GetItemList<T>() where T : struct
        {
            var result = hs[typeof(T?)] as List<KeyValuePair<T?, string>>;
            if (result == null)
            {
                var fs = typeof(T).GetFields().Select(p => new
                {
                    p,
                    att = p.GetCustomAttributes(false).FirstOrDefault(q => q is DescriptionAttribute) as DescriptionAttribute
                }).Where(p => p.att != null).ToList();


                result = new List<KeyValuePair<T?, string>>();

                foreach (var f in fs)
                {
                    T t = (T)Enum.Parse(typeof(T), f.p.GetValue(null).ToString());
                    result.Add(new KeyValuePair<T?, string>(t, f.att.Description));
                }
                hs[typeof(T)] = result;
            }
            return result;
        }
        /// <summary>
        /// 获取枚举的Description值
        /// </summary>
        /// <param name="source">枚举对象</param>
        /// <returns>枚举描述</returns>
        public static string ToDesc(this Enum source)
        {
            if (source == null) throw new ArgumentException("source");
            var type = source.GetType();
            if (Enum.IsDefined(type, source))
            {
                var field = type.GetField(Enum.GetName(type, source));
                if (field != null)
                {
                    if (Attribute.IsDefined(field, typeof(DescriptionAttribute)))
                    {
                        var descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return source.ToString();
        }
    }
}
