using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DF.Finance.Common
{
    public class LogManager
    {
        

        public static log4net.ILog TraceLogger
        {
            get {
                if (_TraceLogger==null)
                {
                    _TraceLogger = log4net.LogManager.GetLogger("TraceLogger");
                }

                return _TraceLogger;
            }

        } private static log4net.ILog _TraceLogger;


        public static log4net.ILog ErrorDBLogger
        {
            get {
                if (_ErrorDBLogger==null)
                {
                    _ErrorDBLogger = log4net.LogManager.GetLogger("ErrorDBLogger");
                }

                return _ErrorDBLogger;
            }

        } private static log4net.ILog _ErrorDBLogger;

        
    }

    public static class LoggerEx
    {

        public static object FormatMessage(object message, Exception ex = null)
        {
            object msg = "";
            System.Web.HttpContext ctx = System.Web.HttpContext.Current;
            if (ctx != null)
            {
                string ip = ctx.Request.UserHostAddress;
                string url = ctx.Request.Url.ToString();
                string refer = "";
                if (ctx.Request.UrlReferrer != null) refer = ctx.Request.UrlReferrer.ToString();

                if (ex == null)
                {
                    msg = string.Format("{0}@@ip={1}@@url={2}@@referer={3}", message, ip, url, refer);
                }
                else
                {
                    msg = string.Format("{0} {1}@@ip={2}@@url={3}@@referer={4}", message, ex.Message, ip, url, refer);
                }

            }
            else
            {

                if (ex == null)
                {
                    msg = message;
                }
                else
                {
                    msg = message + " " + ex.Message;
                }
            }

            return msg;
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void DebugEx(this ILog logger, object message)
        {
            message = FormatMessage(message);
            logger.Debug(message);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void DebugEx(this ILog logger, object message, Exception ex)
        {
            message = FormatMessage(message,ex);
            logger.Debug(message,ex);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void FatalEx(this ILog logger, object message)
        {
            message = FormatMessage(message);
            logger.Fatal(message);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void FatalEx(this ILog logger, object message, Exception ex)
        {
            message = FormatMessage(message, ex);
            logger.Fatal(message, ex);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void InfoEx(this ILog logger, object message)
        {
            message = FormatMessage(message);
            logger.Info(message);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void InfoEx(this ILog logger, object message, Exception ex)
        {
            message = FormatMessage(message, ex);
            logger.Info(message, ex);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void ErrorEx(this ILog logger, object message)
        {
            message = FormatMessage(message);
            logger.Error(message);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void ErrorEx(this ILog logger, object message, Exception ex)
        {
            message = FormatMessage(message, ex);
            logger.Error(message, ex);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void WarnEx(this ILog logger, object message)
        {
            message = FormatMessage(message);
            logger.Warn(message);
        }
        /// <summary>
        /// FATAL（致命错误）、ERROR（一般错误）、WARN（警告）、INFO（一般信息）、DEBUG（调试信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void WarnEx(this ILog logger, object message, Exception ex)
        {
            message = FormatMessage(message, ex);
            logger.Warn(message, ex);
        }

    }


}
