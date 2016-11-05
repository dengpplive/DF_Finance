using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Finance.Common
{
    /// <summary>  
    /// 双TOP二分法生成分页SQL类(支持MSSQL、ACCESS)
    /// </summary>  
    public static class PagingHelper
    {
        /// <summary>
        /// 获取分页SQL语句，排序字段需要构成唯一记录
        /// </summary>
        /// <param name="_recordCount">记录总数</param>
        /// <param name="_pageSize">每页记录数</param>
        /// <param name="_pageIndex">当前页数</param>
        /// <param name="_safeSql">SQL查询语句</param>
        /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
        /// <returns>分页SQL语句</returns>
        //public static string CreatePagingSql(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField)
        //{
        //    //重新组合排序字段，防止有错误  order by f1 asc,f2 desc;
        //    string[] arrStrOrders = _orderField.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    StringBuilder sbOriginalOrder = new StringBuilder(); //原排序字段
        //    StringBuilder sbReverseOrder = new StringBuilder(); //与原排序字段相反，用于分页

        //    for (int i = 0; i < arrStrOrders.Length; i++)    
        //    {
        //        arrStrOrders[i] = arrStrOrders[i].Trim();  //去除前后空格 
        //        if (i != 0)
        //        {
        //            sbOriginalOrder.Append(", ");              
        //            sbReverseOrder.Append(", ");                
        //        }
        //        sbOriginalOrder.Append(arrStrOrders[i]);

        //        int index = arrStrOrders[i].IndexOf(" "); //判断是否有升降标识     
        //        if (index > 0)
        //        {
        //            //替换升降标识，分页所需
        //            bool flag = arrStrOrders[i].IndexOf(" DESC", StringComparison.OrdinalIgnoreCase) != -1;
        //            sbReverseOrder.AppendFormat("{0} {1}", arrStrOrders[i].Remove(index), flag ? "ASC" : "DESC");
        //        }
        //        else
        //        {
        //            sbReverseOrder.AppendFormat("{0} DESC", arrStrOrders[i]);
        //        }
        //    }

        //    //计算总页数
        //    _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
        //    int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

        //    //检查当前页数
        //    if (_pageIndex < 1)
        //    {
        //        _pageIndex = 1;
        //    }
        //    else if (_pageIndex > pageCount)
        //    {
        //        _pageIndex = pageCount;
        //    }

        //    StringBuilder sbSql = new StringBuilder();
        //    //第一页时，直接使用Limit n，而不进行分页查询
        //    if (_pageIndex == 1)
        //    {
        //        sbSql.AppendFormat("SELECT * FROM ({0}) AS T ", _safeSql);
        //        if (!string.IsNullOrEmpty(sbOriginalOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
        //        sbSql.AppendFormat(" LIMIT {0}", _pageSize);
        //    }
        //    //最后一页时，减少一个TOP
        //    else if (_pageIndex == pageCount)
        //    {
        //        sbSql.Append(" SELECT * FROM ");
        //        sbSql.Append(" ( ");
        //        sbSql.AppendFormat("SELECT * FROM ({0}) AS T ", _safeSql);
        //        if (!string.IsNullOrEmpty(sbReverseOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
        //        sbSql.AppendFormat(" LIMIT {0}", _recordCount - _pageSize * (_pageIndex - 1));
        //        sbSql.Append(" ) AS T ");
        //        if (!string.IsNullOrEmpty(sbOriginalOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
        //    }
        //    //前半页数时的分页
        //    else if (_pageIndex <= (pageCount / 2 + pageCount % 2) + 1)
        //    {
        //        sbSql.Append(" SELECT * FROM ");
        //        sbSql.Append(" ( ");
        //        sbSql.AppendFormat(" SELECT * FROM ");
        //        sbSql.Append(" ( ");
        //        sbSql.AppendFormat(" SELECT *  FROM ({0}) AS T ", _safeSql);
        //        if (!string.IsNullOrEmpty(sbOriginalOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
        //        sbSql.AppendFormat(" LIMIT {0}", _pageSize * _pageIndex);
        //        sbSql.Append(" ) AS T");
        //        if (!string.IsNullOrEmpty(sbReverseOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
        //        sbSql.AppendFormat(" LIMIT {0}", _pageSize);
        //        sbSql.Append(" ) AS T ");
        //        if (!string.IsNullOrEmpty(sbOriginalOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
        //    }
        //    //后半页数时的分页
        //    else
        //    {
        //        sbSql.Append(" SELECT * FROM ( ");
        //        sbSql.AppendFormat(" SELECT * FROM ({0}) AS T ", _safeSql);
        //        if (!string.IsNullOrEmpty(sbReverseOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
        //        sbSql.AppendFormat(" LIMIT {0}", ((_recordCount % _pageSize) + _pageSize * (pageCount - _pageIndex)));
        //        sbSql.Append(" ) AS T ");
        //        if (!string.IsNullOrEmpty(sbOriginalOrder.ToString()))
        //            sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString(), _pageSize);
        //        sbSql.AppendFormat(" LIMIT {0}", _pageSize);
        //    }
        //    return sbSql.ToString();
        //}

        ///// <summary>
        ///// 获取记录总数SQL语句
        ///// </summary>
        ///// <param name="_n">限定记录数</param>
        ///// <param name="_safeSql">SQL查询语句</param>
        ///// <returns>记录总数SQL语句</returns>
        //public static string CreateTopnSql(int _n, string _safeSql)
        //{
        //    return string.Format(" SELECT * FROM ({1}) AS T LIMIT {0}", _n, _safeSql);
        //}

        ///// <summary>
        ///// 获取记录总数SQL语句
        ///// </summary>
        ///// <param name="_safeSql">SQL查询语句</param>
        ///// <returns>记录总数SQL语句</returns>
        //public static string CreateCountingSql(string _safeSql)
        //{
        //    return string.Format(" SELECT COUNT(1) AS RecordCount FROM ({0}) AS T ", _safeSql);
        //}

        /// <summary>
        /// 获取分页SQL语句，默认row_number为关健字，所有表不允许使用该字段名
        /// </summary>
        /// <param name="_recordCount">记录总数</param>
        /// <param name="_pageSize">每页记录数</param>
        /// <param name="_pageIndex">当前页数</param>
        /// <param name="_safeSql">SQL查询语句</param>
        /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
        /// <returns>分页SQL语句</returns>
        public static string CreatePagingSql(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField)
        {
            //计算总页数
            _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
            int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

            //检查当前页数
            if (_pageIndex < 1)
            {
                _pageIndex = 1;
            }
            else if (_pageIndex > pageCount)
            {
                _pageIndex = pageCount;
            }
            //拼接SQL字符串，加上ROW_NUMBER函数进行分页
            StringBuilder newSafeSql = new StringBuilder();
            newSafeSql.AppendFormat("SELECT ROW_NUMBER() OVER(ORDER BY {0}) as row_number,", _orderField);
            newSafeSql.Append(_safeSql.Substring(_safeSql.ToUpper().IndexOf("SELECT") + 6));

            //拼接成最终的SQL语句
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM (");
            sbSql.Append(newSafeSql.ToString());
            sbSql.Append(") AS T");
            sbSql.AppendFormat(" WHERE row_number between {0} and {1}", ((_pageIndex - 1) * _pageSize) + 1, _pageIndex * _pageSize);

            return sbSql.ToString();
        }

        /// <summary>
        /// 获取记录总数SQL语句
        /// </summary>
        /// <param name="_safeSql">SQL查询语句</param>
        /// <returns>记录总数SQL语句</returns>
        public static string CreateCountingSql(string _safeSql)
        {
            return string.Format(" SELECT COUNT(1) AS RecordCount FROM ({0}) AS T ", _safeSql);
        }
    }
}
