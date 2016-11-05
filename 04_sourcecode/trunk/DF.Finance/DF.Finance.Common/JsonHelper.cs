using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;

namespace DF.Finance.Common
{
    public class JsonHelper
    {
        public static string JsonOk(string data)
        {
            return JsonHelper.JsonMsg("ok", "", data);
        }


        public static string JsonFail(string msg)
        {
            return JsonHelper.JsonMsg("fail", msg, "{}");
        }

        /// <summary>
        /// 生成 Ajax消息 字符串
        /// </summary>
        /// <param name="state"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <param name="nextUrl"></param>
        /// <returns></returns>
        public static string JsonMsg(string state, string msg, string data, string nextUrl = null)
        {
            //{"state":"err","msg":"出错啦~~","data":[{},{}],"nextUrl":"Login.aspx"}
            string strMsg = "{\"state\":\"" + state + "\",\"msg\":\"" + msg.Replace('"', ' ')
                .Replace('\'', ' ').Replace("\r\n", "") + "\",\"data\":" + (data == null ? "null" : data)
                + ",\"nextUrl\":" + (nextUrl == null ? "null" : "\"" + nextUrl + "\"") + "}";

            return strMsg;
            //直接输出 数据 到 浏览器
            //System.Web.HttpContext.Current.Response.Write(strMsg);
        }


        public static string ToJson(Object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JavaScriptConverter[] converters = { new EntityObjectConverter() };
            serializer.RegisterConverters(converters);

            string json = serializer.Serialize(obj);
            return json;
        }

        public static string ToJson(string state, Object obj)
        {
            var obj1 = new
            {
                state = state,
                data = obj
            };
            return ToJson(obj1);
        }

        public static string ToJson<T>(List<T> list, int iPageSize, int iCurrentPage, int iTotalRecords, string state = "ok")
        {
            var obj = new
            {
                state = state,
                page = iCurrentPage,
                pageSize = iPageSize,
                records = iTotalRecords,
                rows = list,
            };

            return JsonHelper.ToJson(obj);

        }

        public static string ToJson(object list, int iPageSize, int iCurrentPage, int iTotalRecords, string state = "ok")
        {
            var obj = new
            {
                state = state,
                page = iCurrentPage,
                pageSize = iPageSize,
                records = iTotalRecords,
                rows = list,
            };

            return JsonHelper.ToJson(obj);

        }

        public static string ToJson(object list, int iPageSize, int iCurrentPage, int iTotalRecords, object data, string state = "ok")
        {
            var obj = new
            {
                state = state,
                page = iCurrentPage,
                pageSize = iPageSize,
                records = iTotalRecords,
                rows = list,
                data = data
            };

            return JsonHelper.ToJson(obj);

        }

        public static string ToJson(DataTable dt, int iPageSize, int iCurrentPage, int iTotalRecords, object data, string state = "ok")
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    row.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                rows.Add(row);
            }

            var obj = new
            {
                state = state,
                page = iCurrentPage,
                pageSize = iPageSize,
                records = iTotalRecords,
                rows = rows,
                data = data
            };

            return JsonHelper.ToJson(obj);

        }

        public static string ToJson(DataTable dt, int iPageSize, int iCurrentPage, int iTotalRecords, string state = "ok")
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    row.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                rows.Add(row);
            }

            var obj = new
            {
                state = state,
                page = iCurrentPage,
                pageSize = iPageSize,
                records = iTotalRecords,
                rows = rows,
            };

            return JsonHelper.ToJson(obj);

        }

        public static T ToObject<T>(string json)
        {
            //JsonSerializer js = JsonSerializer.Create(new JsonSerializerSettings());
            //T obj = JsonConvert.DeserializeObject<T>(json);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T obj = serializer.Deserialize<T>(json);

            return obj;
        }

        /// <summary>
        /// Get the json of current page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="queryAll">the total result from database</param>
        /// <param name="iCurrentPage"></param>
        /// <param name="iPageSize"></param>
        /// <param name="orderKey">how to order the query. e.g. e=>e.ID</param>
        /// <returns></returns>
        public static string ToListJson<T, TKey>(IQueryable<T> queryAll, int iCurrentPage, int iPageSize, Func<T, TKey> orderKey)
        {
            int iTotalRecords = queryAll.Count();

            var queryResult = queryAll.OrderBy(orderKey).Skip((iCurrentPage - 1) * iPageSize).Take(iPageSize);

            List<T> list = queryResult.ToList<T>();

            int iPageCount = Convert.ToInt32(System.Math.Ceiling((double)iTotalRecords / iPageSize));

            string json = JsonHelper.ToJson<T>(list, iPageCount, iCurrentPage, iTotalRecords);

            return json;
        }


        /// <summary>
        /// Get the json of current page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query">the total result from database</param>
        /// <param name="iCurrentPage"></param>
        /// <param name="iPageSize"></param>
        /// <param name="orderKey">how to order the query. e.g. e=>e.ID</param>
        /// <returns></returns>
        public static string ToJson<T>(IEnumerable<T> query, int iCurrentPage, int iPageSize)
        {
            int iTotalRecords = query.Count();

            var queryResult = query.Skip((iCurrentPage - 1) * iPageSize).Take(iPageSize);

            List<T> list = queryResult.ToList<T>();

            int iPageCount = Convert.ToInt32(System.Math.Ceiling((double)iTotalRecords / iPageSize));

            string json = JsonHelper.ToJson<T>(list, iPageCount, iCurrentPage, iTotalRecords);

            return json;
        }

        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                byte[] b = Encoding.UTF8.GetBytes(jss.Serialize(obj));
                return Encoding.UTF8.GetString(b);
            }
            catch (Exception ex)
            {

                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        /// <summary> 
        /// 数据表转键值对集合
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
                 = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary> 
        /// 数据集转键值对数组字典 
        /// </summary> 
        /// <param name="dataSet">数据集</param> 
        /// <returns>键值对数组字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));

            return result;
        }

        /// <summary> 
        /// 数据表转JSON 
        /// </summary> 
        /// <param name="dataTable">数据表</param> 
        /// <returns>JSON字符串</returns> 
        public static string DataTableToJSON(DataTable dt)
        {
            return ObjectToJSON(DataTableToList(dt));
        }

        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        /// <summary> 
        /// 将JSON文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }

        /// <summary> 
        /// 将JSON文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据行的字典</returns>
        public static Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }
    }
}
