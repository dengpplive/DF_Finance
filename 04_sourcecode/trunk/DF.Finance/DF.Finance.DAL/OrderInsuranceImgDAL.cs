using DF.Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.DAL
{
    public class OrderInsuranceImgDAL : BaseDAL<OrderInsuranceImg>
    {
        public bool AddOrEdit(int InsuranceId, List<string> ImgPath, List<string> remark)
        {
            try
            {
                using (var scope = db.GetTransaction())
                {
                    DeleteById(InsuranceId);//删除旧图片
                    for (var i = 0; i < ImgPath.Count; i++) 
                    {
                        OrderInsuranceImg model = new OrderInsuranceImg();
                        model.InsuranceId = InsuranceId;
                        model.ImgPath = ImgPath[i].ToString();
                        model.Remark = remark[i].ToString();
                        base.Add(model);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除图片(数据库)
        /// </summary>
        /// <param name="InsuranceId"></param>
        /// <returns></returns>
        public bool DeleteById(int InsuranceId)
        {
            return base.Delete("delete from OrderInsuranceImg where InsuranceId=@0", InsuranceId) > 0;
        }
    }
}
