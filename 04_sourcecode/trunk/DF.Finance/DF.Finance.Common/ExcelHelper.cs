using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace DF.Finance.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// Excel to DataSet
        /// </summary>
        /// <param name="FilePath">文件保存的相对路径</param>
        /// <returns></returns>
        public static DataSet excelDs(string FilePath) 
        {
            DataSet excelDs = new DataSet();
            string connString = ExcelHelper.GetConnString(ExcelHelper.exToDB(), FilePath);
            OleDbConnection excelConn = new OleDbConnection(connString);
            excelConn.Open();
            OleDbDataAdapter ExcelDA = new OleDbDataAdapter("SELECT * FROM  [Sheet1$]", excelConn);
            try
            {
                ExcelDA.Fill(excelDs, "ex_test");
            }
            catch (Exception err)
            {
                //
                excelDs = null;
            }
            finally
            {
                excelConn.Close();
                excelConn = null;
            }
            return excelDs;
        }

        /// <summary>
        /// 获取Excel链接字符串
        /// </summary>
        /// <param name="exToDB"></param>
        /// <param name="FilePath">文件保存的相对路径</param>
        /// <returns></returns>
        public static string GetConnString(string exToDB, string FilePath)
        {
            return string.Format(exToDB, getPath(FilePath));
        }
       
        /// <summary>
        /// 获取文件的绝对路径
        /// </summary>
        /// <param name="FilePath">文件存放的相对地址</param>
        /// <returns></returns>
        public static string getPath(string FilePath)
        {
            return Utils.GetMapPath("../") + FilePath;

        }

        /// <summary>
        /// execl读取配置
        /// </summary>
        /// <returns></returns>
        public static string exToDB()
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            return cmdText;
        }
    }
}
