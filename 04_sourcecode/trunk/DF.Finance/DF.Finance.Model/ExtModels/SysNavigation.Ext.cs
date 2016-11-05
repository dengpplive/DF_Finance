using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class SysNavigation
    {
        /// <summary>
        /// 层级样式
        /// </summary>
        public int ClassLayer { get; set; }
        /// <summary>
        /// 旧父级菜单
        /// </summary>
        public int OldParentId { get; set; }
       
        private string _procType = "edit";
        /// <summary>
        /// 操作类型
        /// </summary>
        public string ProcType
        {
            get { return _procType; }
            set { _procType = value; }
        }
        /// <summary>
        /// 二级导航，因模板只支持二级
        /// </summary>
        public List<SysNavigation> NavigationList { get; set; }
    }
}
