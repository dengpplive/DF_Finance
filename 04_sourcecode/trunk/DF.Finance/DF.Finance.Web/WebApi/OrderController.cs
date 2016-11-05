using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace DF.Finance.Web.WebApi
{
    public class OrderController : BaseWebApiController
    {
        [HttpGet, WebApiIsLogon]
        public RetObj InitData()
        {
            //获取区域信息 area_city
            //获取放贷金融公司 lending_company
            //获取贷款产品 loan_products
            //获取贷款利息类型 rate_type
            //放款环节 lending_segment
            //贷款期限 loan_term
            //汽车品牌 car_management
            var typeCodeList = new List<string>() { "lending_company", "loan_products", "rate_type", "lending_segment", "loan_term", "area_city", "car_management" };
            var dicList = new SysDictionaryBLL().GetListByTypeCodes(typeCodeList);
            //实际交易方 
            var cmList = new CustomerManagementBLL().GetList(" DeleteFlag=0 ", "").ToList();
            return new RetObj(new
            {
                CMList = cmList.Select(p => new { Name = p.CompanyName, p.Id }),
                InitProvinceList = ListDic(dicList, "area_city"),
                LendingCompanyList = ListDic(dicList, "lending_company"),
                LoanProductsList = ListDic(dicList, "loan_products"),
                RateTypeList = ListDic(dicList, "rate_type"),
                LendingSegmentList = ListDic(dicList, "lending_segment"),
                LoanTermList = ListDic(dicList, "loan_term"),
                CarList=ListDic(dicList,"car_management")

            });
        }
        private List<Model.SysDictionary> DicListByTypeCode(List<Model.SysDictionary> dicList, string typeCode)
        {
            return dicList.Where(p => p.TypeCode == typeCode).ToList();
        }
        private IEnumerable<object> ListDic(List<Model.SysDictionary> dicList, string typeCode)
        {
            return DicListByTypeCode(dicList, typeCode).Select(p => new { p.Name, p.ParentId, p.Id });
        }
        /// <summary>
        /// 录入资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, WebApiIsLogon]
        public RetObj EntryInformation(Model.Order model)
        {
            OrderBLL bll = new OrderBLL();
            bll.InputData(model, GetUserId(), DTRequest.GetIP());
            return new RetObj();
        }


    }
}
