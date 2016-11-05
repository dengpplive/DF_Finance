using DF.Finance.DAL;
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
    /// 字典
    /// </summary>
    public class SysDictionaryBLL : BaseBLL<SysDictionary>
    {
        SysDictionaryDAL _dal = new SysDictionaryDAL();
        /// <summary>
        /// 该类型下是否存在同名的名称
        /// </summary>
        /// <param name="sysDictionary"></param>
        /// <returns></returns>
        public bool Exist(SysDictionary sysDictionary)
        {
            return _dal.GetList(string.Format("  DeleteFlag=0 and  Name ='{0}' and DicType={1} ", sysDictionary.Name, sysDictionary.DicType), null).SingleOrDefault() != null ? true : false;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="sysDictionary"></param>
        /// <returns></returns>
        public bool LogicalDelete(string Ids)
        {
            return _dal.Update<SysDictionary>(string.Format(" set  DeleteFlag=1 where Id in({0})", Ids)) > 0;
        }
        /// <summary>
        /// 获取父级下面的子节点数据列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public List<SysDictionary> GetListByParentId(int parentId)
        {

            return _dal.GetList(string.Format(" ParentId={0} and  DeleteFlag=0", parentId), " SortId asc ");

        }

        /// <summary>
        /// 根据Val和分类类型获取他的下级列表
        /// </summary>
        /// <param name="val">val</param>
        /// <param name="dicType">分类类型</param>
        /// <returns></returns>
        public List<SysDictionary> GetListByVal(string code, int dicType)
        {
            return _dal.GetList(string.Format(" DeleteFlag=0 DicType={0} and  ParentId=(select top 1 Id from SysDictionary where Val='{1}') ", dicType, code), " SortId asc ");
        }
        /// <summary>
        /// 单个typeCode获取字典数据
        /// </summary>
        /// <param name="typeCode">typeCode</param>
        /// <returns></returns>

        public List<SysDictionary> GetListByTypeCode(string typeCode)
        {
            return _dal.GetList(string.Format(" TypeCode='{0}'", typeCode), " SortId asc ");
        }
        /// <summary>
        /// 多个typeCode获取字典数据
        /// </summary>
        /// <param name="typeCodes">typeCodes</param>
        /// <returns></returns>
        public List<SysDictionary> GetListByTypeCodes(List<string> typeCodes)
        {
            var result = new List<SysDictionary>();
            if (typeCodes != null && typeCodes.Count > 0)
            {
                var strList = new List<string>();
                typeCodes.ForEach(p => strList.Add("'" + p + "'"));
                result = _dal.GetList(string.Format(" TypeCode in ({0}) and DeleteFlag=0 and isHide=0 ", string.Join(",", strList.ToArray())), " SortId asc ");
            }
            return result;
        }
        /// <summary>
        /// 多个typeCode获取字典数据
        /// </summary>
        /// <param name="typeCodes">typeCodes</param>
        /// <returns></returns>
        public List<SysDictionary> GetListByTypeCodes(List<string> typeCodes, int parentId)
        {
            var result = new List<SysDictionary>();
            if (typeCodes != null && typeCodes.Count > 0)
            {
                var strList = new List<string>();
                typeCodes.ForEach(p => strList.Add("'" + p + "'"));
                result = _dal.GetList(string.Format(" TypeCode in ({0}) and ParentId={1} and DeleteFlag=0 and isHide=0  ", string.Join(",", strList.ToArray()), parentId), " SortId asc ");
            }
            return result;
        }

        /// <summary>
        /// 根据字典分类类型Code获取子项数据
        /// </summary>
        /// <param name="dicType">字典分类code</param>
        /// <param name="parentId">父Id</param>
        /// <returns>返回结果列表</returns>
        public List<SysDictionary> GetDicByTypeCode(string typeCode, int parentId = 0)
        {
            return _dal.GetDicByDicType(typeCode, parentId);
        }



        /// <summary>
        /// 根据字典类型获取子项数据
        /// </summary>
        /// <param name="dic">请求参数</param>
        /// <param name="dicType">字典类型Id</param>
        /// <param name="parentId">父Id</param>
        /// <returns>返回结果列表</returns>
        public DictionaryManageView GetDicListByDicType(Dictionary<string, string> dic, SysDictionaryType type, int parentId = 0, bool isPager = true)
        {
            var result = new DictionaryManageView();
            var allList = _dal.GetDicTypeList(type.Id);
            GetChilds(allList, result, type.Id, 0, parentId);
            if (isPager)
            {
                int skip = int.Parse(dic["offset"]);
                int limit = int.Parse(dic["limit"]);
                result.Total = allList.Count + 1;
                result.SysDictionaryList = result.SysDictionaryList.Skip(skip).Take(limit).ToList();
            }
            result.SysDictionaryList.Insert(0, new SysDictionary()
            {
                Id = 0,
                DicType = type.Id,
                Name = type.TypeName,
                ParentId = 0,
                Level = 0,
                Val = type.TypeCode
            });
            return result;
        }
        /// <summary>
        /// 获取所有的子集
        /// </summary>
        /// <param name="allList">所有的待筛选的项集合</param>
        /// <param name="result">返回的结果</param>
        /// <param name="dicType">分类类型id</param>
        /// <param name="parentId">父级Id</param>
        private void GetChilds(List<SysDictionary> allList, DictionaryManageView result, int dicType, int Level, int parentId = 0)
        {
            Level++;
            var list = allList.Where(p => p.DicType == dicType)
                .Where(p => p.ParentId == parentId).OrderBy(p => p.SortId).ToList();
            list.ForEach(p =>
            {
                p.Level = Level;
                result.SysDictionaryList.Add(p);
                GetChilds(allList, result, dicType, Level, p.Id);
            });
        }




    }
}
