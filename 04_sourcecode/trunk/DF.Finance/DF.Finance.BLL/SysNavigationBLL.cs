using DF.Finance.DAL;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class SysNavigationBLL : BaseBLL<SysNavigation>
    {
        protected SysNavigationDAL _dal = new SysNavigationDAL();
        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <returns>DataTable</returns>
        public List<SysNavigation> GetList(int parentId)
        {
            
           return _dal.GetList(parentId);
        }
    }
}
