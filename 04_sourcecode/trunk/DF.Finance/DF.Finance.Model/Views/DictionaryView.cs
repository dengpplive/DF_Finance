using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model.Views
{
    /// <summary>
    /// 字典分类下面的字典数据
    /// </summary>
    public class DictionaryView
    {
        private List<Item> _DicList = new List<Item>();
        public List<Item> DicList
        {
            get
            {
                return _DicList;
            }
            set
            {
                _DicList = value;
            }
        }
    }
    /// <summary>
    /// 字典数据
    /// </summary>
    public class Item
    {
        public SysDictionaryType DictionaryType { get; set; }

        public List<SysDictionary> DictionaryList { get; set; }
    }
}
