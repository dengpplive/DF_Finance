using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    /// <summary>
    /// 字典分类管理
    /// </summary>
    public class SysDictionaryTypeBLL : BaseBLL<SysDictionaryType>
    {
        /// <summary>
        /// 字典分类列表
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="order">排序参数</param>
        /// <returns></returns>
        public Page<SysDictionaryType> GetSysDictionaryTypeList(Dictionary<string, string> dic, string order = null)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(dic["TypeName"]))
            {
                sbWhere.AppendFormat(" and TypeName like '%{0}%' ", dic["TypeName"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["TypeCode"]))
            {
                sbWhere.AppendFormat(" and TypeCode='{0}' ", dic["TypeCode"].Replace("'", ""));
            }
            if (!string.IsNullOrEmpty(dic["IsSys"]))
            {
                sbWhere.AppendFormat(" and IsSys={0} ", dic["IsSys"].Replace("'", ""));
            }
            return this.GetPage(int.Parse(dic["offset"]), int.Parse(dic["limit"]), sbWhere.ToString(), order);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="typeName">类型</param>
        /// <returns></returns>
        public bool Exist(SysDictionaryType sysDictionaryType)
        {
            return dal.GetList(string.Format(" TypeName ='{0}' and TypeCode='{1}'",
                sysDictionaryType.TypeName, sysDictionaryType.TypeCode), null).SingleOrDefault() != null ? true : false;
        }

        /// <summary>
        /// 获取所有的字典分类
        /// </summary>
        /// <returns></returns>
        public List<SysDictionaryType> GetList()
        {
            return dal.GetList(null, null);
        }
        /// <summary>
        /// 删除字典分类
        /// </summary>
        /// <param name="ids">ids使用","隔开</param>
        /// <returns></returns>
        public bool DeleteByIds(string ids)
        {
            return Delete(string.Format(" where Id in({0})", ids));
        }


        /// <summary>
        /// 所有的分类字典数据
        /// </summary>
        /// <returns></returns>
        public DictionaryView GetDicData()
        {
            SysDictionaryBLL bll = new SysDictionaryBLL();
            DictionaryView dv = new DictionaryView();
            var typeList = dal.GetList(null, null).Where(p => p.IsSys == 0).ToList();
            typeList.ForEach(p =>
            {
                dv.DicList.Add(new Item()
                {
                    DictionaryType = p,
                    DictionaryList = bll.GetDicByTypeCode(p.TypeCode)
                });
            });
            return dv;
        }
    }
}
