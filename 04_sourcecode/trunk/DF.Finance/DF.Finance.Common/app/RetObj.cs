using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Finance.Common
{

    /// <summary>
    /// json 处理返回对象,便于客户端进行处理
    /// </summary>
    public class RetObj
    {

        /// <summary>
        /// 成功操作
        /// </summary>
        public const int OK = 1;
        /// <summary>
        /// 失败操作
        /// </summary>
        public const int ERR = 0;
        /// <summary>
        /// 没有操作权限
        /// </summary>
        public const int NO_ACCESS = -1;
        /// <summary>
        /// 登录超时
        /// </summary>
        public const int OVER_TIME = -2;

        /// <summary>
        /// 返回值
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// 默认构造操作成功的返回
        /// </summary>
        public RetObj()
        {
            Code = RetObj.OK;
            Msg = "OK";
        }
        /// <summary>
        /// 以指定的数据构造操作的返回结果
        /// </summary>
        /// <param name="data"></param>
        public RetObj(dynamic data)
        {
            this.Code = OK;
            this.Data = data;
            this.Msg = "OK";
        }
        /// <summary>
        /// 以指定的code和msg构造操作的返回结果
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">消息</param>
        public RetObj(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }
        public RetObj(Exception ex)
        {
            Exception(ex);
        }
        /// <summary>
        /// 设置返回的错误信息
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public RetObj Error(string msg)
        {
            this.Code = ERR;
            this.Msg = msg;
            return this;
        }
        public RetObj Exception(Exception ex)
        {
            LogHelper.WriteLog("api", ex);
            return Error("服务器异常！");
        }
        /// <summary>
        /// 设置数据值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public RetObj SetData(dynamic data)
        {
            this.Code = OK;
            this.Data = data;
            this.Msg = "OK";
            return this;
        }
        /// <summary>
        /// 设置无权限反馈信息
        /// </summary>
        /// <param name="msg">信息内容</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public RetObj NoAccess(string msg, dynamic data = null)
        {
            this.Msg = msg;
            this.Code = NO_ACCESS;
            this.Data = data;
            return this;
        }
        /// <summary>
        /// 登录超时
        /// </summary>
        /// <returns></returns>
        public RetObj OverTime()
        {
            this.Code = OVER_TIME;
            this.Msg = "登录超时！";
            return this;
        }
    }
}
