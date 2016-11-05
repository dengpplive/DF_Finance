using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using System.Diagnostics;


namespace DF.Finance.Common
{

    /// <summary>
    /// 简单的日志记录类, 记录到TXT文档
    /// </summary>
    public static class Log
    {
        private static object lockobj = new object();

        private static void WriteToFile(string filename, string content)
        {
            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine(content);
                sw.Close();
            }
        }

        private static string GetDateTime()
        {
            return "[" + DateTime.Now.ToString("HH:mm:ss") + "] ";
        }

        /// <summary>
        /// 用于普通日志记录，记录的信息将在程序目录下根据日期建立文本文件“log_20090408.txt”
        /// </summary>
        /// <param name="msg"></param>
        public static void Output(string msg)
        {
            lock (lockobj)
            {
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "log\\log_data", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                Log.WriteToFile(filePath + @"\log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", GetDateTime() + msg);
            }
        }


        /// <summary>
        /// 用于错误日志输出，与Output方法类似，不但输出日志信息，还输出当前的文件名、行、列号以及当前执行的方法名，以便进行分析。
        /// </summary>
        /// <param name="msg"></param>
        public static void OutputError(string msg)
        {
            lock (lockobj)
            {
                StackTrace st = new StackTrace(true);

                StackFrame sf = st.GetFrame(1);
                StringBuilder sb = new StringBuilder(GetDateTime());
                sb.Append("ERROR:");
                sb.AppendLine(msg);
                sb.AppendLine("Error from:");
                sb.AppendLine(sf.GetFileName());
                sb.Append("Line:");
                sb.AppendLine(sf.GetFileLineNumber().ToString());
                sb.Append("Column:");
                sb.AppendLine(sf.GetFileColumnNumber().ToString());
                sb.Append("Method:");
                sb.Append(sf.GetMethod().Name);
                // string filename=Directory.GetCurrentDirectory() + @"\log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "log\\log_err", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string filename = filePath + @"\log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                Log.WriteToFile(filename, sb.ToString());

            }
        }
    }

}
