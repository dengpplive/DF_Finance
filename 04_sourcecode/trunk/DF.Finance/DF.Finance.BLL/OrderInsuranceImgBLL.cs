using DF.Finance.DAL;
using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.BLL
{
    public class OrderInsuranceImgBLL : BaseBLL<OrderInsuranceImg>
    {
        OrderInsuranceImgDAL _dal = new OrderInsuranceImgDAL();
        public bool AddOrEdit(int InsuranceId, List<string> ImgPath, List<string> remark) 
        {

            return _dal.AddOrEdit(InsuranceId, ImgPath, remark);
        }

        public List<OrderInsuranceImg> GetByInsuranceId(int InsuranceId) 
        {
            return _dal.GetList(string.Format(" InsuranceId={0}", InsuranceId), "");
        }
        public bool DeleteById(int InsuranceId)
        {
            return _dal.DeleteById(InsuranceId);
        }
    }
}
