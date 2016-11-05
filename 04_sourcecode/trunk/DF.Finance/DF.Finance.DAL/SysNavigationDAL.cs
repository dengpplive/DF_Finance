using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class SysNavigationDAL : BaseDAL<SysNavigation>
    {
        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <returns>DataTable</returns>
        public List<SysNavigation> GetList(int parentId)
        {
            List<SysNavigation> list = base.GetList("", " SortId asc ,Id desc ");
            if (list == null)
            {
                return null;
            }
            List<SysNavigation> newData = new List<SysNavigation>();
            GetChilds(list, newData, parentId, 0);
            return newData;
        }

        #region 私有方法===============================
        /// <summary>
        /// 从内存中取得所有下级类别列表（自身迭代）
        /// </summary>
        private void GetChilds(List<SysNavigation> oldData, List<SysNavigation> newData, int parentId, int classLayer)
        {
            classLayer++;
            IEnumerable<SysNavigation> pList = oldData.Where(p => p.ParentId == parentId);
            foreach (var item in pList)
            {
                item.ClassLayer = classLayer;
                newData.Add(item);
                this.GetChilds(oldData, newData, item.Id, classLayer);
            }
        }

        /// <summary>
        /// 获取父类下的所有子类ID
        /// </summary>
        private void GetChildIds(DataTable dt, int parent_id, ref string ids)
        {
            DataRow[] dr = dt.Select("parent_id=" + parent_id);
            for (int i = 0; i < dr.Length; i++)
            {
                ids += dr[i]["id"].ToString() + ",";
                //调用自身迭代
                this.GetChildIds(dt, int.Parse(dr[i]["id"].ToString()), ref ids);
            }
        }
        #endregion

    }
}
