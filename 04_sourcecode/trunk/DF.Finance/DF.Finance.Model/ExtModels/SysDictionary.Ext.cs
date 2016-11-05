using DF.Finance.DBUtility.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Model
{
    public partial class SysDictionary
    {
        /// <summary>
        /// 中奖人数
        /// </summary>
        [Ignore]
        public int PrizeCount { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Ignore]
        public int Level { get; set; }
        /// <summary>
        /// 旧父级菜单
        /// </summary>
        public int OldParentId { get; set; }

        /// <summary>
        /// 1添加 0修改
        /// </summary>
        [Ignore]
        public int OperType { get; set; }

        private List<SysDictionary> _ChildList = new List<SysDictionary>();
        /// <summary>
        /// 子集
        /// </summary>
        [Ignore]
        public List<SysDictionary> ChildList
        {
            get
            {
                return _ChildList;
            }
            set { _ChildList = value; }
        }

    }
}
