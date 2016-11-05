using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class SysDictionaryDAL : BaseDAL<SysDictionary>
    {
        /// <summary>
        /// 根据字典类型获取子项数据
        /// </summary>
        /// <param name="dicType">字典类型Id</param>
        /// <param name="parentId">父Id</param>
        /// <returns>返回结果列表</returns>
        public List<SysDictionary> GetDicType(int dicType, int parentId = 0)
        {
            return db.Query<SysDictionary>("select * from SysDictionary where DeleteFlag=0 DicType=@0 and ParentId=@1 order by SortId asc,Id asc  ", dicType, parentId).ToList();
        }

        /// <summary>
        /// 根据字典类型获取所有子项数据
        /// </summary>
        /// <param name="dicType">字典类型Id</param>        
        /// <returns>返回结果列表</returns>
        public List<SysDictionary> GetDicTypeList(int dicType)
        {
            return db.Query<SysDictionary>("select * from SysDictionary where DicType=@0 and DeleteFlag=0  order by SortId asc,Id asc ", dicType).ToList();
        }

        /// <summary>
        /// 根据字典类型获取子项数据
        /// </summary>
        /// <param name="dicType">字典分类code</param>
        /// <param name="parentId">父Id</param>
        /// <returns>返回结果列表</returns>
        public List<SysDictionary> GetDicByDicType(string code, int parentId = 0)
        {
            return db.Query<SysDictionary>("select * from SysDictionary where DeleteFlag=0 and  DicType=(select top 1 Id from SysDictionaryType where TypeCode=@0) and ParentId=@1 order by SortId asc ,Id asc  ", code, parentId).ToList();
        }

    }
}
