using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Configuration;
using log4net.Config;
using log4net;

namespace QINGUO.Common
{
    /// <summary>
    /// 网站log4net日志记录网站异常信息
    /// </summary>
    public class SharePresentationLog : Exception, ILogAction
    {
        private static ILog logger = null;
        /// <summary>
        /// 初始化错误处理器
        /// </summary>
        public SharePresentationLog()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(HttpContext.Current.Server.MapPath("/Log/config/Log4NetConfig.config")));
            logger = LogManager.GetLogger(ConfigurationManager.AppSettings["ApplicationLoger"]);
        }
        /// <summary>
        /// 初始化错误处理器，传入错误信息
        /// </summary>
        /// <param name="message">错误信息描述</param>
        public SharePresentationLog(string message)
            : base(message)
        {

        }
        /// <summary>
        /// 使用特定信息创建错误
        /// </summary>
        /// <param name="messageFormat">错误信息显示格式</param>
        /// <param name="args">错误处理参数.</param>
        public SharePresentationLog(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {

        }

        #region 调试信息
        /// <summary>
        /// 调试应用
        /// </summary>
        /// <param name="message">自定义错误信息(如果不传此参数可以在本类构造函数中传递,或则传递Exception异常类型)</param>
        public void Debug(object message = null)
        {
            if (message == null)
            {
                logger.Debug(Message);
            }
            else
            {
                logger.Debug(message);
            }
        }
        /// <summary>
        /// 调试应用
        /// </summary>
        /// <param name="ex">调试异常信息类</param>
        /// <param name="message">自定义错误信息(如果不传此参数可以在本类构造函数中传递,或则传递Exception异常类型)</param>
        public void Debug(Exception ex, object message = null)
        {
            if (message != null)
            {
                logger.Debug(message, ex);
            }
            else
            {
                logger.Debug(Message, ex);
            }
        }
        /// <summary>
        /// 调试应用
        /// </summary>
        /// <param name="format">格式化错误消息字符串</param>
        /// <param name="args">可变参数</param>
        public void DebugFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }
        #endregion

        #region 系统输出信息
        /// <summary>
        /// 一般信息输出
        /// </summary>
        /// <param name="message">自定义信息输出</param>
        public void Info(object message = null)
        {
            if (message != null)
            {
                logger.Info(message);
            }
            else
            {
                logger.Info(Message);
            }
        }
        /// <summary>
        /// 一般信息输出
        /// </summary>
        /// <param name="ex">自定义信息输出异常类</param>
        /// <param name="message">自定义信息输出</param>
        public void Info(Exception ex, object message = null)
        {
            if (message != null)
            {
                logger.Info(message, ex);
            }
            else
            {
                logger.Info(Message, ex);
            }
        }
        /// <summary>
        /// 系统输入消息
        /// </summary>
        /// <param name="format">格式化字符</param>
        /// <param name="args">可变参数</param>
        public void InfoFormat(string format, params object[] args)
        {
            logger.FatalFormat(format, args);
        }
        #endregion

        #region 警告信息
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="message">自定义警告信息</param>
        public void Warn(object message)
        {
            if (message != null)
            {
                logger.Warn(message);
            }
            else
            {
                logger.Warn(Message);
            }
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="ex">警告信息异常类</param>
        /// <param name="message">自定义警告信息</param>
        public void Warn(Exception ex, object message = null)
        {
            if (message != null)
            {
                logger.Warn(message, ex);
            }
            else
            {
                logger.Warn(Message, ex);
            }
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="format">警告信息格式化字符串</param>
        /// <param name="args">参数</param>
        public void WarnFormat(string format, params object[] args)
        {
            logger.WarnFormat(format, args);
        }
        #endregion

        #region 错误信息
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message">自定义错误消息</param>
        public void Error(object message)
        {
            if (message != null)
            {
                logger.Error(message);
            }
            else
            {
                logger.Error(Message);
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="ex">自定义错误异常类</param>
        /// <param name="message">自定义错误消息</param>
        public void Error(Exception ex, object message = null)
        {
            if (message != null)
            {
                logger.Error(message, ex);
            }
            else
            {
                logger.Error(Message, ex);
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="format">错误信息格式化</param>
        /// <param name="args">可变参数</param>
        public void ErrorFormat(string format, params object[] args)
        {
            logger.ErrorFormat(format, args);
        }
        #endregion

        #region 严重错误
        /// <summary>
        /// 严重错误 
        /// </summary>
        /// <param name="message">自定义错误消息</param>
        public void Fatal(object message = null)
        {
            if (message != null)
            {
                logger.Fatal(message);
            }
            else
            {
                logger.Fatal(Message);
            }
        }
        /// <summary>
        ///  严重错误 
        /// </summary>
        /// <param name="message">自定义错误消息</param>
        /// <param name="ex">异常信息类</param>
        public void Fatal(Exception ex, object message = null)
        {
            if (message != null)
            {
                logger.Fatal(message, ex);
            }
            else
            {
                logger.Fatal(Message, ex);
            }
        }
        /// <summary>
        /// 严重错误
        /// </summary>
        /// <param name="format">严重错误格式化字符串</param>
        /// <param name="args">可变参数</param>
        public void FatalFormat(string format, params object[] args)
        {
            logger.FatalFormat(format, args);
        }
        #endregion

    }
}
