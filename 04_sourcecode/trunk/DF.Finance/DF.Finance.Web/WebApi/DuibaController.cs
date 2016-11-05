using DF.Finance.BLL;
using DF.Finance.Common;
using DF.Finance.Model;
using DF.Finance.WebCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace DF.Finance.Web.WebApi
{
    public class DuibaController : BaseWebApiController
    {
        /// <summary>
        /// 获得兑吧 URL
        /// </summary>
        /// <returns></returns>
        [HttpGet, WebApiIsLogon]
        public RetObj GetIndexUrl()
        {
            string userId = Utils.GetCookie("userId");
            var model = new UserBLL().GetUserInfo(int.Parse(userId));
            string url = DuibaCommon.GetIndexUrl(userId, model.Point);
           // CreateGetHttpResponse(url);
           
           // Utils.HttpGet(url);
            return new RetObj(url);
        }
        /// <summary>
        /// 扣除积分接口
        /// </summary>
        [HttpGet]
        public HttpResponseMessage PointsDeduction()
        {
            //记录日志 用于追踪错误 调试成功后删除
            //LogHelper.WriteLog(HttpContext.Current.Request.ToJSON());
            Hashtable hs = DuibaCommon.ParseCreditConsume(HttpContext.Current.Request);

            DBOrder model = new DBOrder()
            {
                DBOrderNum = hs["orderNum"].ToString(),
                DBCredits = int.Parse(hs["credits"].ToString()),
                UsreId = int.Parse(hs["uid"].ToString()),
                DBJson = hs.ToJSON()

            };
            var thePoints = 0;
            var ret = new RetObj();
            var id = new DBOrderBLL().Add(model);
            if (id != null)
            {
                var userModel = new UserBLL().GetUserInfo(model.UsreId);
                //构建失败json
                if (userModel == null)
                {
                    ret.Code = RetObj.ERR;
                    ret.Msg = "用户不存在！";
                }
                else
                {
                    //直接转换成负数
                    var point = 0 - model.DBCredits;
                    //积分不足，构建失败json
                    thePoints = userModel.Point + point;
                    if (thePoints < 0)
                    {
                        ret.Code = RetObj.ERR;
                        ret.Msg = "余额不足！不与承兑";
                    }
                    else
                    {
                        //开始扣除积分
                        var pointModel = new UserPointLog()
                        {
                            Remark = "兑换礼品扣除积分：" + point,
                            UserName = userModel.UserName,
                            UserId = userModel.Id,
                            SourceType = 3,
                            SourceId = int.Parse(id.ToString()),
                            Value = point
                        };
                        var rn = new UserPointLogBLL().CompleteAdd(pointModel);
                        //构建失败json
                        if (!rn)
                        {
                            ret.Code = RetObj.ERR;
                            ret.Msg = "修改积分失败";
                            model.Id = int.Parse(id.ToString());
                            model.Status = 1;
                            model.Remark = ret.Msg;
                            //开始修改订单状态
                            new DBOrderBLL().Update(model, new List<string>() { "Status", "Remark" });
                        }
                    }
                }
            }
            else
            {
                //构建失败json
                ret.Code = RetObj.ERR;
                ret.Msg = "添加数据失败";
            }
            if (ret.Code == RetObj.ERR)
            {
                return new HttpResponseMessage { Content = new StringContent(new { status = "fail", errorMessage = ret.Msg, credits = thePoints }.ToJSON(), System.Text.Encoding.UTF8, "application/json") };
            }
            return new HttpResponseMessage { Content = new StringContent(new { status = "ok", errorMessage = "", bizId = id, credits = thePoints }.ToJSON(), System.Text.Encoding.UTF8, "application/json") };

        }
        /// <summary>
        /// 兑换结果通知
        /// </summary>
        [HttpGet]
        public string ResultsNotice()
        {
            Hashtable hs = DuibaCommon.ParseCreditConsume(HttpContext.Current.Request);
            var model = new BLL.DBOrderBLL().GetModelByDBOrderNum(hs["orderNum"].ToString());
            if (model == null)
            {
                return "ok";
            }
            if (!bool.Parse(hs["success"].ToString()))
            {
                if (model.Status == 0)
                {
                    var userModel = new UserBLL().GetUserInfo(model.UsreId);
                    //开始扣除积分
                    var pointModel = new UserPointLog()
                    {
                        Remark = "兑换礼品失败，返还积分：" + model.DBCredits,
                        UserName = userModel.UserName,
                        UserId = userModel.Id,
                        SourceType = 3,
                        SourceId = model.Id,
                        Value = model.DBCredits
                    };
                    model.Remark = pointModel.Remark;
                    var rn = new UserPointLogBLL().CompleteAdd(pointModel);

                }
            }
            model.Status = 1;
            //开始修改订单状态
            new DBOrderBLL().Update(model, new List<string>() { "Status", "Remark" });
            return "ok";
        }
        private static void CreateGetHttpResponse(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            request.BeginGetResponse(null, null);//异步
            //return request.GetResponse() as HttpWebResponse;
        }

    }
}
