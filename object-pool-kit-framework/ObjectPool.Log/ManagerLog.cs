//
//  ManagerLog.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//        
using System;
using System.Globalization;
using NLog;

namespace ObjectPool.Log
{
    public static class ManagerLog
    {
        public static void WriteMessage(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var logger = LogManager.GetLogger("Manager");

            logger.Info(CultureInfo.InvariantCulture, "{0}", message);
        }

        public static void WriteManagerMessage(string message, LogLevel logLevel)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var logger = LogManager.GetLogger("Manager");

            if (logLevel == LogLevel.Info)
            {
                logger.Info(CultureInfo.InvariantCulture, "manager: {0}", message);
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace(CultureInfo.InvariantCulture, "manager: {0}", message);
            }
        }

        public static void WritePoolMessage(string message, LogLevel logLevel)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var logger = LogManager.GetLogger("Manager");

            if (logLevel == LogLevel.Info)
            {
                logger.Info(CultureInfo.InvariantCulture, "pool: {0}", message);
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace(CultureInfo.InvariantCulture, "pool: {0}", message);
            }
        }

        public static void WriteCountersMessage(string message, LogLevel logLevel)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var logger = LogManager.GetLogger("Manager");

            if (logLevel == LogLevel.Info)
            {
                logger.Info(CultureInfo.InvariantCulture, "counters: {0}", message);
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace(CultureInfo.InvariantCulture, "counters: {0}", message);
            }
        }
    }
}