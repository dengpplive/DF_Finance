using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class StringFilter
    {
        /// <summary>
        ///电话展示规则，130****0000 
        ///11位 130****0000 
        ///13位 86130****0000
        ///14位 +86130****0000
        /// </summary>
        /// <returns></returns>
        public static string Tel(string tel)
        {
            if (!string.IsNullOrEmpty(tel))
            {
                string token = "****";
                if (tel.Length == 0) { tel = ""; }
                else if (tel.Length == 11) { tel = tel.Substring(0, 3) + token + tel.Substring(7, 4); }
                else if (tel.Length == 13) { tel = tel.Substring(2, 3) + token + tel.Substring(9, 4); }
                else if (tel.Length == 14) { tel = tel.Substring(3, 3) + token + tel.Substring(10, 4); }
            }
            return tel;
        }

        /// <summary>
        /// 姓名展示规则，等于两个字展示  姓+星号，大于两个字展示   姓+星号+最后一个字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Name(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string token = "*";
                name = name.Trim();
                if (name.Length == 0) { name = ""; }
                else if (name.Length == 2)
                {
                    name = name.Substring(0, 1) + token;
                }
                else
                {
                    name = name.Substring(0, 1) + token + name.Substring(name.Length - 1, 1);
                }
            }
            return name;
        }
    }
}
