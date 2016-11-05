using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    public class RandomPwd
    {
        /// <summary>
        /// 字符源
        /// </summary>
        private static readonly string[] source = { "7", "8", "9", "4", "5", "0", "1", "2", "3", "6", "A", "B", "C", "U", "E", "F", "G","H","I","C","K","L","K","A","O","P","Q","R","R","T","U","V","W","Y","Y","I"};
        static Random rand = new Random();
        public static string Pwd
        {
            get;
            set;
        }

        public static string BuildPwd(int codeLength)
        {
            string pwd = CreateCode(codeLength);
            Pwd = pwd;
            return pwd;
        }

        public static string CreateCode(int codeLength)
        {
            string[] strArr = source;

            var codeArray = new string[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                codeArray[i] = strArr[rand.Next(0, strArr.Length)];
                rand.NextDouble();
            }

            return string.Concat(codeArray);
        }
    }
}
