// 作    者:jim.li
// 功    能:用于分页的数据包装
// 创建日期:2016-04-11

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{
    /// <summary>
    /// 用于分页的数据包装
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class WebPageData<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 总记录数
        /// </summary>
        public Int64 Total;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {

                return (int)(Total + PageSize - 1) / PageSize;
            }
        }

        /// <summary>
        /// 当前页开始记录序号
        /// </summary>
        public Int64 PageRecordStartNum
        {
            get
            {
                return (CurrentPage - 1) * PageSize + 1;
            }
        }
        /// <summary>
        /// 当前页结束记录序号
        /// </summary>
        public Int64 PageRocordEndNum
        {
            get
            {
                int nEndNum = CurrentPage * PageSize;
                if (nEndNum > this.Total)
                {
                    return this.Total;
                }
                else
                {
                    return nEndNum;
                }
            }
        }

        /// <summary>
        /// 数据列表内容
        /// </summary>
        public IEnumerable<T> Rows;
    }
}