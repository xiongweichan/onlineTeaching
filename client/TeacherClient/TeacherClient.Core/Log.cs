using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeacherClient.Core
{
    class LogSingleton
    {
        private static ILog instance;
        private static object _lock = new object();

        private LogSingleton()
        {

        }

        public static ILog GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); ;
                    }
                }
            }
            return instance;
        }
    }
    // LogSingleton可以不存在
    public static class Log
    {
        private static ILog log = LogSingleton.GetInstance();
        public static void Debug(object message)
        {
            if (log.IsDebugEnabled)
                log.Debug(message);
        }
        public static void Debug(object message, Exception exception)
        {
            if (log.IsDebugEnabled)
                log.Debug(message, exception);
        }
        public static void DebugFormat(string format, params object[] args)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(format, args);
        }
        public static void DebugFormat(string format, object arg0)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(format, arg0);
        }
        public static void DebugFormat(string format, object arg0, object arg1)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(format, arg0, arg1);
        }
        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(format, arg0, arg1, arg2);
        }
        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(provider, format, args);
        }
        public static void Info(object message)
        {
            if (log.IsInfoEnabled)
                log.Info(message);
        }
        public static void Info(object message, Exception exception)
        {
            if (log.IsInfoEnabled)
                log.Info(message, exception);
        }
        public static void InfoFormat(string format, params object[] args)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(format, args);
        }
        public static void InfoFormat(string format, object arg0)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(format, arg0);
        }
        public static void InfoFormat(string format, object arg0, object arg1)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(format, arg0, arg1);
        }
        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(format, arg0, arg1, arg2);
        }
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(provider, format, args);
        }
        public static void Warn(object message)
        {
            if (log.IsWarnEnabled)
                log.Warn(message);
        }
        public static void Warn(object message, Exception exception)
        {
            if (log.IsWarnEnabled)
                log.Warn(message, exception);
        }
        public static void WarnFormat(string format, params object[] args)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(format, args);
        }
        public static void WarnFormat(string format, object arg0)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(format, arg0);
        }
        public static void WarnFormat(string format, object arg0, object arg1)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(format, arg0, arg1);
        }
        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(format, arg0, arg1, arg2);
        }
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(provider, format, args);
        }
        public static void Error(object message)
        {
            if (log.IsErrorEnabled)
                log.Error(message);
        }
        public static void Error(object message, Exception exception)
        {
            if (log.IsErrorEnabled)
                log.Error(message, exception);
        }
        public static void ErrorFormat(string format, params object[] args)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, args);
        }
        static void ErrorFormat(string format, object arg0)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, arg0);
        }
        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, arg0, arg1);
        }
        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, arg0, arg1, arg2);
        }
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(provider, format, args);
        }
        public static void Fatal(object message)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }
        public static void Fatal(object message, Exception exception)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message, exception);
        }
        public static void FatalFormat(string format, params object[] args)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(format, args);
        }
        public static void FatalFormat(string format, object arg0)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(format, arg0);
        }
        public static void FatalFormat(string format, object arg0, object arg1)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(format, arg0, arg1);
        }
        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(format, arg0, arg1, arg2);
        }
        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(provider, format, args);
        }

    }
}
