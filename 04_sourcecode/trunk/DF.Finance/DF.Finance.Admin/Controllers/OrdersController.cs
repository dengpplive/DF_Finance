using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.Model.Enums;
using DF.Finance.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DF.Finance.Admin.Controllers
{
    [RoleNavName("Orders")]
    public class OrdersController : BaseAdminController
    {
        OrderBLL Orderbll = new OrderBLL();

        #region ====视图====
        // GET: Orders
        public ActionResult Index(int status=0)
        {
            ViewBag.status = status;
            ViewBag.CustomerManagements =new CustomerManagementBLL().GetList(" DeleteFlag=0 ","").ToList();//实际交易方
            ViewBag.loan_products = new SysDictionaryBLL().GetDicByTypeCode("loan_products", 0);//贷款产品
            return View();
        }
        
        /// <summary>
        /// 订单详情页视图
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            Orderbll.Exist(Id);//订单是否存在
            ViewBag.CheckInStatus = new SysDictionaryBLL().GetDicByTypeCode("checkIn_action", 0);
            var model = Orderbll.GetOrder(Id);//订单全部信息
            ViewBag.OrderActionEnum = OrderStatusEnumExtension.ToNextAdminString((OrderStatusEnum)(model.Status));
            
            return View(model);
        }

        /// <summary>
        /// 保险录入视图
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult Insurance(int orderId = 0)
        {
            Orderbll.Exist(orderId);//订单是否存在
            ViewBag.orderId = orderId;
            var OrderInsurance = new OrderInsuranceBLL().GetByOrderId(orderId);//订单保险信息
            if (OrderInsurance != null) 
            {
                OrderInsurance.OrderInsuranceImgs = new OrderInsuranceImgBLL().GetByInsuranceId(OrderInsurance.Id);//保险资料图片集合
            }
            return View(OrderInsurance);
        }

        /// <summary>
        /// 放款资料视图
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult Billing(int orderId = 0)
        {
            var OrderExtAttribute = new OrderExtAttributeBLL().GetByOrderId(orderId);//订单扩展表信息
            ViewBag.BillingManagementts = new BillingManagementtBLL().GetList();
            return View(OrderExtAttribute);
        }

        /// <summary>
        /// 登记证操作视图
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult CheckIn(int orderId = 0) 
        {
            Orderbll.Exist(orderId);//订单是否存在
            ViewBag.CheckInStatus = new SysDictionaryBLL().GetDicByTypeCode("checkIn_action", 0);
            var OrderExtAttribute = new OrderExtAttributeBLL().GetByOrderId(orderId);//订单扩展表信息
            return View(OrderExtAttribute);
        }

        /// <summary>
        /// 状态回滚视图
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult Rollback(int orderId = 0)
        {
            Orderbll.Exist(orderId);//订单是否存在
            var order = Orderbll.GetModel(orderId);
            OrderStatusEnum CurrentEnum = (OrderStatusEnum)order.Status;//订单当前状态的枚举值
            List<OrderStatusEnum> OrderStatusEnums = OrderStatusEnumExtension.ToPreviousStatusEnum(CurrentEnum).ToList();
            ViewBag.OrderStatusEnums = OrderStatusEnums;
            return View(order);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>      
        [HttpPost, AdminRole(ActionType = "Delete")]
        public JsonResult Delete(string Ids)
        {
            OrderBLL bll = new OrderBLL();
            string[] strArr = Ids.Split(',');
            if (strArr.Length > 0)
            {
                if (strArr.Length == 1 && int.Parse(strArr[0]) == 0) throw new AdminMessageException("请选择需要删除的字典数据");
                //逻辑删除
                bll.LogicalDelete(Ids);
                //添加日志
                this.OpertionLog(DFEnums.ActionEnum.Delete, "订单管理ids:[" + Ids + "]");
            }
            else
            {
                throw new AdminMessageException("请选择需要删除的字典数据");
            }
            return Json(new RetObj(1, "删除成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ====数据接口====

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Search()
        {
            var dic = Utils.GetRequest();
            var Orders = Orderbll.GetPage(dic);
            var total = Orders.TotalItems;
            var rows = Orders.Items;
            var strId = string.Join(",", rows.Select(p => p.Id).ToArray());
            if (strId != "" && strId != null)
            {
                var OrderExtAttributes = new OrderExtAttributeBLL().GetList(string.Format(" OrderId in({0})", strId));
                foreach (var m in Orders.Items)
                {
                    m.OrderExtAttribute = OrderExtAttributes.Where(p => p.OrderId == m.Id).FirstOrDefault<OrderExtAttribute>();
                }
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存或更新订单扩展表的数据
        /// </summary>
        /// <param name="OrderExtAttribute"></param>
        /// <param name="EnumStr">当前订单操作状态的枚举值</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost,AdminRole(ActionType="Edit")]
        public JsonResult UpdateOrderExtAttribute(OrderExtAttribute OrderExtAttribute) 
        {
            if (OrderExtAttribute.OrderId == 0)
            {
                return Json(new RetObj().Error("参数错误，非法操作"), JsonRequestBehavior.AllowGet);
            }
            else 
            {
                OrderExtAttributeBLL OrderExtAttributebll = new OrderExtAttributeBLL();
                if (OrderExtAttribute.Id > 0)
                {
                    OrderExtAttributebll.Update(OrderExtAttribute);
                }
                else
                {
                    OrderExtAttributebll.Add(OrderExtAttribute);

                }
                return Json(new RetObj(), JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="EnumStr">当前订单操作状态的枚举值</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult Update(int orderId, OrderActionEnum EnumStr, string remark="")
        {
            #region 验证订单是否存在
            Orderbll.Exist(orderId);
            #endregion

            #region 验证非法登录
            if (CurrentUser == null)
            {
                throw new AdminMessageException("用户未登录");
            }
            #endregion

            #region 参数初始化
            Order order = Orderbll.GetOrder(orderId);//订单全部信息
            OrderStatusEnum sourceStatus = (OrderStatusEnum)order.Status;//订单的当前状态
            OrderStatusEnum targetStatus = OrderStatusEnumExtension.GetNextStatus(sourceStatus, EnumStr);//订单目标状态
            #endregion

            #region 验证提交的订单信息是否完整,否，则返回错误的提示信息
            if (Orderbll.ValidateData(order, targetStatus) != "success") 
            {
                return Json(new RetObj().Error(Orderbll.ValidateData(order, targetStatus)), JsonRequestBehavior.AllowGet);
            } 
            #endregion

            #region 检测订单当前状态是否允许转换到目标状态
            if (OrderStatusEnumExtension.CheckToStatus(sourceStatus, targetStatus)) 
            {
                var flag = false;
                if (orderId > 0)
                {
                    try
                    {
                        //更新订单表订单状态
                        Orderbll.UpdateRemark(orderId, (int)targetStatus, remark);

                        //订单操作记录
                        new OrderRecordBLL().AddRecord(CurrentUser, orderId, (int)sourceStatus, (int)targetStatus, remark);

                        //如果订单的操作状态是放款资料录入，则更新实际交易方的最后放款日期
                        if (EnumStr == OrderActionEnum.放款资料录入) 
                        {
                            //更新实际交易方的最后放款日期
                            CustomerManagement CustomerManagement = new CustomerManagementBLL().GetModel(order.DealersId);
                            CustomerManagement.LastLoanTime = order.OrderExtAttribute.LoanTime;
                            new CustomerManagementBLL().Update(CustomerManagement);
                        }
                        flag = true;

                    }
                    catch (Exception ex)
                    {
                        flag = false;
                    }
                }
                if (!flag)
                {
                    return Json(new RetObj().Error("系统错误，请联系管理员"), JsonRequestBehavior.AllowGet);
                }
                return Json(new RetObj(), JsonRequestBehavior.AllowGet);
            }
            return Json(new RetObj().Error("非法操作"), JsonRequestBehavior.AllowGet);
            #endregion
        }

        /// <summary>
        /// 更新订单主表的备注信息，此接口只适用于节点1和节点2的审核
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Edit")]
        public JsonResult OrderRemark(string remark,int orderId=0)
        {
            Orderbll.Exist(orderId);//订单是否存在
            Order order = Orderbll.GetOrder(orderId);
            if (order != null)
            {
                //更新订单表订单备注信息
                Orderbll.UpdateRemark(orderId, order.Status, remark);//只保存备注，不更新状态，不记录操作记录
                return Json(new RetObj(), JsonRequestBehavior.AllowGet);
            }
            return Json(new RetObj().Error("系统不存在当前订单，请联系管理员"), JsonRequestBehavior.AllowGet);
           
        }

        /// <summary>
        /// 保险信息录入或修改
        /// </summary>
        /// <param name="OrderInsurance"></param>
        /// <returns></returns>
        [HttpPost,AdminRole(ActionType="Edit")]
        public JsonResult UpdateOrderInsurance(OrderInsurance orderInsurance) 
        {
            //1.初始化数据
            List<string> Paths = new List<string>();
            List<string> Remarks = new List<string>();
            if (orderInsurance.photo_name.Length != 0) 
            {
                Paths = orderInsurance.photo_name.Substring(0, orderInsurance.photo_name.Length - 1).Split('|').ToList();//图片地址集合
            }
            if (orderInsurance.photo_remark.Length != 0) 
            {
                Remarks = orderInsurance.photo_remark.Substring(0, orderInsurance.photo_remark.Length - 1).Split('|').ToList();//图片备注集合
            }
            int InsuranceId=0;

            try 
            {
                //判断订单状态是否已到达可操作保险信息（放款资料录入之后就可以操作）
                Order order =Orderbll.GetOrder(orderInsurance.OrderId);
                if (order.Status < 5 && order.Status == 91 && order.Status == 92) 
                {
                    return Json(new RetObj().Error("非法操作"), JsonRequestBehavior.AllowGet);
                }
                //2.增加或更新保险信息
                OrderInsuranceBLL InsuranceBll = new OrderInsuranceBLL();
                OrderInsuranceImgBLL InsuranceImgBll=new OrderInsuranceImgBLL();
                List<OrderInsuranceImg> Imgs = new List<OrderInsuranceImg>();
                var model = new OrderInsuranceBLL().GetList(string.Format(" OrderId={0}", orderInsurance.OrderId), "");//判断当前订单是否已存在保险信息
                if (model.Count!=0)//更新
                {
                    orderInsurance.UpdateName = CurrentUser.UserName;
                    orderInsurance.UpdateUserId = CurrentUser.Id;
                    orderInsurance.UpdateTime = DateTime.Now;
                    InsuranceBll.Update(orderInsurance);

                    InsuranceId = model.FirstOrDefault().Id;//确保Id无误
                    Imgs = InsuranceImgBll.GetByInsuranceId(InsuranceId);//保险资料图片集合
                }
                else//添加
                {
                    orderInsurance.AddUserId = CurrentUser.Id;
                    orderInsurance.AddName = CurrentUser.UserName;
                    orderInsurance.AddTime = DateTime.Now;
                    object id = InsuranceBll.Add(orderInsurance);
                    InsuranceId = int.Parse(id.ToString());
                }
                //3.保存或更新保险资料图片,如果新的图片都添加成功了，则删除老的图片集合
                if (Paths.Count != 0) 
                {
                    if (InsuranceImgBll.AddOrEdit(InsuranceId, Paths, Remarks))
                    {
                        //4、删除老图片
                        //if (Imgs.Count != 0) 
                        //{
                        //    string fullpath = string.Empty;
                        //    foreach (var m in Imgs) 
                        //    {
                        //        if (Utils.FileExists(m.ImgPath)) //如果文件存在，则删除
                        //        {
                        //            Utils.DeleteFile(m.ImgPath);
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch(Exception ex) 
            {
                return Json(new RetObj().Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
            return Json(new RetObj(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单状态回滚，只能从当前这一步往上回滚
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="PreviousEnum">要扭转的订单状态枚举值（往上回滚）</param>
        /// <returns></returns>
        [HttpPost, AdminRole(ActionType = "Rollback")]
        public JsonResult RollBack(int orderId, OrderStatusEnum PreviousEnum) 
        { 
            Orderbll.Exist(orderId);//订单是否存在
            Order order = Orderbll.GetOrder(orderId);
            OrderStatusEnum CurrentEnum = (OrderStatusEnum)order.Status;//订单当前状态的枚举值
            try
            {
                if (OrderStatusEnumExtension.CheckToPrevious(CurrentEnum, PreviousEnum))//判断是否可以往上回滚订单状态
                {
                    //1.更新订单状态
                    Orderbll.UpdateRemark(orderId, (int)PreviousEnum, "");

                    //2.记录订单操作日志
                    new OrderRecordBLL().AddRecord(CurrentUser, orderId, (int)CurrentEnum, (int)PreviousEnum, "订单状态回滚");

                    return Json(new RetObj(), JsonRequestBehavior.AllowGet);
                }
                
                    return Json(new RetObj().Error("不允许从" + CurrentEnum + "状态回滚到" + PreviousEnum), JsonRequestBehavior.AllowGet);
                
            }
            catch(Exception ex)
            {
                return Json(new RetObj().Error(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}