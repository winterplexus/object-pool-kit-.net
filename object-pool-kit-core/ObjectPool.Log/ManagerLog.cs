//
//  ManagerLog.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//        
using System;
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

            logger.Info("{message}");
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
                logger.Info($"manager: {message}");
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace($"manager: {message}");
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
                logger.Info($"pool: {message}");
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace($"pool: {message}");
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
                logger.Info($"counters: {message}");
            }
            if (logLevel == LogLevel.Trace)
            {
                logger.Trace($"counters: {message}");
            }
        }
    }
}