//
//  ManagerLog.cs
//
//  Copyright (c) Code Construct System 2018-2024
//        
using System;
using NLog;

namespace ObjectPool.Log
{
    public static class ManagerLog
    {
        public static void WriteMessage(string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            var logger = LogManager.GetLogger("Manager");

            logger.Info("{message}");
        }

        public static void WriteManagerMessage(string message, LogLevel logLevel)
        {
            ArgumentNullException.ThrowIfNull(message);

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
            ArgumentNullException.ThrowIfNull(message);

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
            ArgumentNullException.ThrowIfNull(message);

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