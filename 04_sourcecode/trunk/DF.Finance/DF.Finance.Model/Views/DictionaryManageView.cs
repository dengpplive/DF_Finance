using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 后台字典显示
    /// </summary>
    public class DictionaryManageView
    {
        private List<SysDictionary> _SysDictionaryList = new List<SysDictionary>();
        /// <summary>
        /// 系统字典数据列表
        /// </summary>
        public List<SysDictionary> SysDictionaryList
        {
            get
            {
                return _SysDictionaryList;
            }
            set
            {
                _SysDictionaryList = value;
            }
        }
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Total { get; set; }
    }
}
